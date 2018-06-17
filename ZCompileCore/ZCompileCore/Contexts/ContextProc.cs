using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.CommonCollections;

using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;

using ZCompileNLP;
using System.Diagnostics;
using ZLangRT.Utils;
using System.Reflection;

namespace ZCompileCore.Contexts
{
    public abstract class ContextProc
    {
        public bool IsNested { get; protected set; }
        public bool IsConstructor { get; protected set; }
        public bool IsNormal { get { return !(IsConstructor || IsNested); } }

        public ContextProc()
        {
            LocalManager = new ProcLocalManager(this);
        }

        public ContextProc(ContextClass classContext):this()
        {
            this.ClassContext = classContext;
            ProcIndex++;
            //_KeyContext = ProcIndex.ToString();
        }

        public ContextClass ClassContext { get; protected set; }
        private ContextNestedClass _NestedClassContext;
        public ZCLocalVar NestedInstance { get; protected set; }

        public ContextNestedClass GetNestedClassContext()
        {
            return _NestedClassContext;
        }

        public ContextNestedClass CreateNestedClassContext()
        {
            if(_NestedClassContext==null )
            {
                _NestedClassContext = new ContextNestedClass(this);
                _NestedClassContext.SetClassName(this.CreateNestedClassName());
                var nestedClassInstanceName = _NestedClassContext.ClassName + "_0";

                var packageName = this.ClassContext.FileContext.ProjectContext.ProjectModel.ProjectPackageName;
                string fullName = packageName + "." + _NestedClassContext.ClassName;
                TypeAttributes typeAttrs = TypeAttributes.NestedPrivate | TypeAttributes.Sealed;
                TypeBuilder NestedClassBuilder = this.ClassContext.SelfCompilingType.ClassBuilder.DefineNestedType(fullName, typeAttrs);
                _NestedClassContext.SetTypeBuilder(NestedClassBuilder);

                ZCClassInfo ztype = _NestedClassContext.SelfCompilingType;
                if (NestedInstance == null)
                {
                    NestedInstance = new ZCLocalVar(nestedClassInstanceName, ztype, true) { IsNestedClassInstance = true };
                    this.LocalManager.Add(NestedInstance);
                }

                if(!this.IsStatic())
                {
                   ZCFieldInfo  zf2= _NestedClassContext.SelfCompilingType.DefineFieldPublic
                       (ContextNestedClass.MasterClassFieldName,this.ClassContext.SelfCompilingType);
                   _NestedClassContext.MasterClassField = zf2;
                }

                if(this.ArgList.Count>0)
                {
                    foreach(var arg in this.ArgList)
                    {
                        ZCParamInfo zp = this.GetParameter(arg);
                        ZCFieldInfo zf3 = _NestedClassContext.SelfCompilingType.DefineFieldPublic
                       (arg, zp.GetZClass());
                        _NestedClassContext.MasterArgDict.Add(arg,zf3);
                    }
                }
                /* 生成内部类默认构造函数 */
                {
                    ConstructorBuilder NewBuilder = NestedClassBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { });
                    var il = NewBuilder.GetILGenerator();
                    il.Emit(OpCodes.Ret);
                    ZCConstructorInfo zcc = new ZCConstructorInfo(this._NestedClassContext.SelfCompilingType) { ConstructorBuilder = NewBuilder };
                    this._NestedClassContext.SelfCompilingType.AddConstructord(zcc);
                    this._NestedClassContext.DefaultConstructorBuilder = NewBuilder;
                }
            }
            return _NestedClassContext;
        }

        //private string _KeyContext;
        private static int ProcIndex = 0;

        public abstract bool HasParameter(string name);
        public abstract ZCParamInfo GetParameter(string name);
        public abstract ZCParamInfo GetParameter(int i);
        public abstract int GetParametersCount();
        public abstract ILGenerator GetILGenerator();

        public virtual void DefineParameter(ZCParamInfo zcparam)
        {
            zcparam.DefineParameter();
        }
        public abstract string CreateNestedClassName();
        public abstract ZCParamInfo AddParameterName(string paramName);

        public bool ContainsVarName(string name)
        {
            if (this.HasParameter(name)) return true;
            if (this.LocalManager.IsDefLocal(name)) return true;
            if (this.IsThisField(name)) return true;
            if (this.IsThisProperty(name)) return true;
            if (this.IsSuperField(name)) return true;
            if (this.IsSuperProperty(name)) return true;
            if (this.IsUsedProperty(name)) return true;
            if (this.IsUsedEnumItem(name)) return true;
            return false;
        }

        #region 判读是否是
        public bool IsThisProperty(string name)
        {
            ZCClassInfo zct = this.ClassContext.GetZCompilingType();//.ThisCompilingType;
            foreach (ZCPropertyInfo member in zct.ZPropertys)
            {
                if (member.HasZName(name))
                    return true;
            }
            return false;
        }

        public bool IsThisField(string name)
        {
            return false;
        }

        public bool IsSuperProperty(string name)
        {
            ZLClassInfo zbase = this.ClassContext.GetSuperZType();
            if (zbase == null) return false;
            ZLPropertyInfo zmember = zbase.SearchProperty(name);
            return zmember != null;
        }

        public bool IsSuperField(string name)
        {
            ZLClassInfo zbase = this.ClassContext.GetSuperZType();
            if (zbase == null) return false;
            var zmember = zbase.SearchField(name);
            return zmember != null;
        }

        public bool IsUsedEnumItem(string name)
        {
            ContextImportUse contextiu = this.ClassContext.FileContext.ImportUseContext;
            return contextiu.IsUseEnumItem(name);
        }

        public bool IsUsedField(string name)
        {
            ContextImportUse importUseContext = this.ClassContext.FileContext.ImportUseContext;
            return importUseContext.IsUsedField(name);
        }

        public bool IsUsedProperty(string name)
        {
            ContextImportUse importUseContext = this.ClassContext.FileContext.ImportUseContext;
            return importUseContext.IsUsedProperty(name);
        }

        public bool IsCompilingClassName(string name)
        {
            string thisname = this.ClassContext.GetClassName();
            return name == thisname;
        }

        public bool IsImportClassName(string name)
        {
            ContextImportUse importUseContext = this.ClassContext.FileContext.ImportUseContext;
            return importUseContext.IsImportClassName(name);
        }

        public bool IsThisMethodSingle(string name)
        {
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            //ZClassCompilingType zcc = this.ClassContext.ThisCompilingType;
            var methods = this.ClassContext.SearchThisProc(calldesc);
            return methods.Length > 0;
        }

        public bool IsSuperMethodSingle(string name)
        {
            if (this.IsStatic()) return false;
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            var methods = this.ClassContext.SearchSuperProc(calldesc);
            return methods.Length > 0;
        }

        public bool IsUseMethodSingle(string name)
        {
            ContextImportUse importUseContext = this.ClassContext.FileContext.ImportUseContext;
            //ContextUse cu = this.ClassContext.FileContext.UseContext;
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            var methods = importUseContext.SearchUseMethod(calldesc);
            return methods != null && methods.Length > 0;
        }

        //protected Dictionary<string, ZCLocalVar> localDefDict = new Dictionary<string, ZCLocalVar>();

        //public bool IsDefLocal(string name)
        //{
        //    return localDefDict.ContainsKey(name);
        //}

        //public ZCLocalVar GetDefLocal(string name)
        //{
        //    return localDefDict[name];
        //}

        #endregion

        #region create index :localvar ,arg, each

        public ProcLocalManager LocalManager { get; private set; }

        int ArgIndex = -1;
        public List<string> ArgList = new List<string>();
        public int CreateArgIndex(string name)
        {
            if (ArgIndex == -1)
            {
                if (IsStatic())
                {
                    ArgIndex = 0;
                }
                else
                {
                    ArgIndex = 1;
                }
                ArgList.Add(name);
            }
            else
            {
                ArgIndex++;
                ArgList.Add(name);
            }
            return ArgIndex;
        }

        private int TempIndex = -1;
        public int CreateTempIndex()
        {
            TempIndex++;
            return TempIndex;
        }

        int EachIndex = -1;

        public int CreateEachIndex()
        {
            EachIndex++;
            return EachIndex;
        }

        int RepeatIndex = -1;
        public int CreateRepeatIndex()
        {
            RepeatIndex++;
            return RepeatIndex;
        }

        #endregion

        public bool IsStatic()
        {
            return this.ClassContext.IsStatic();
        }

        private UserWordsSegementer _ProcComplexSegmenter;
        public UserWordsSegementer ProcSegmenter
        {
            get
            {
                if (_ProcComplexSegmenter == null)
                {
                    _ProcComplexSegmenter = this.ClassContext.FileContext.ImportUseContext.GetFileSegementer().Clone();
                }
                return _ProcComplexSegmenter;
            }
        }

        public int AddLocalVar(ZCLocalVar localSymbol)
        {
            int index =  LocalManager.Add(localSymbol);
            if (!localSymbol.IsAutoGenerated)
            {
                this.ProcSegmenter.AddWord(localSymbol.ZName);
                //localDefDict.Add(localSymbol.ZName, localSymbol);
            }
            return index;
        }

        
    }
     
}
