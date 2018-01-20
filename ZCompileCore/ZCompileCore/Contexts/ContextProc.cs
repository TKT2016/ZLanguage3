using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;

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
        public ContextClass ClassContext { get; protected set; }
        private string _KeyContext;
        private static int ProcIndex = 0;

        public abstract bool HasParameter(string name);
        public abstract ZCParamInfo GetParameter(string name);
        public abstract ZCParamInfo GetParameter(int i);
        public abstract int GetParametersCount();
        public abstract ILGenerator GetILGenerator();
        //public abstract ParameterBuilder DefineParameter(int position, string strParamName);
        public virtual void DefineParameter(ZCParamInfo zcparam)
        {
            zcparam.DefineParameter();
        }
        public abstract string CreateNestedClassName();
        public abstract ZCParamInfo AddParameterName(string paramName);

        public bool ContainsVarName(string name)
        {
            if (this.HasParameter(name)) return true;
            if (this.IsDefLocal(name)) return true;
            if (this.IsThisField(name)) return true;
            if (this.IsThisProperty(name)) return true;
            if (this.IsSuperField(name)) return true;
            if (this.IsSuperProperty(name)) return true;
            if (this.IsUsedProperty(name)) return true;
            if (this.IsUsedEnumItem(name)) return true;
            return false;
        }

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

        public ContextProc(ContextClass classContext)
        {
            this.ClassContext = classContext;
            ProcIndex++;
            _KeyContext = ProcIndex.ToString();
        }

        protected Dictionary<string, ZCLocalVar> localDefDict = new Dictionary<string, ZCLocalVar>();

        public bool IsDefLocal(string name)
        {
            return localDefDict.ContainsKey(name);
        }

        public ZCLocalVar GetDefLocal(string name)
        {
            return localDefDict[name];
        }


        #region create index :localvar ,arg, each
        int LoacalVarIndex = -1;
        public List<string> LoacalVarList = new List<string>();
        public int CreateLocalVarIndex(string name)
        {
            LoacalVarIndex++;
            LoacalVarList.Add(name);
            return LoacalVarIndex;
        }

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
        //public List<string> ArgList = new List<string>();
        public int CreateEachIndex()
        {
            EachIndex++;
            //ArgList.Add(name);
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

        public void AddLocalVar(ZCLocalVar localSymbol)
        {
            this.ProcSegmenter.AddWord(localSymbol.ZName);
            localDefDict.Add(localSymbol.ZName, localSymbol);
        }
    }
}
