using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Symbols;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZNLP;
using ZCompileDesc.ZMembers;
using System.Diagnostics;
using ZCompileDesc.Compilings;
using ZLangRT.Utils;
using System.Reflection;

namespace ZCompileCore.Contexts
{
    public class ContextProc 
    {
        public ContextClass ClassContext { get; private set; }
        public bool IsConstructor { get;private set; }
        public ZMethodDesc ProcDesc { get;private set; }
        public ZType RetZType { get; set; }
        public string ProcName { get; set; }

        private static int ProcIndex = 0;
        string _KeyContext;
        static int ContextProcIndex = 0;
        public ContextProc(ContextClass classContext,bool isConstructor)
        {
            this.ClassContext = classContext;
            ProcIndex++;
            EmitContext = new ProcEmitContext();
            ContextProcIndex++;
            _KeyContext = ContextProcIndex.ToString();
            IsConstructor = isConstructor;
        }

        Dictionary<string, string> _Argtext = new Dictionary<string, string>();
        public bool ContainArgtext(string text)
        {
            return _Argtext.ContainsKey(text);
        }

        public void AddArgtext(string text)
        {
            _Argtext.Add(text, text);
        }

        //Dictionary<string, string> _Argnames = new Dictionary<string, string>();
        Dictionary<string, SymbolArg> _argDefDict = new Dictionary<string, SymbolArg>();

        public bool ContainArg(string name)
        {
            return _argDefDict.ContainsKey(name);
        }

        public void AddArg(SymbolArg argSymbol)
        {
            _argDefDict.Add(argSymbol.Name, argSymbol);
        }

        public SymbolArg GetDefArg(string name)
        {
            return _argDefDict[name];
        }

        public bool ContainsVarName(string name)
        {
            if (this.ContainArg(name)) return true;
            if (this.IsDefLocal(name)) return true;
            if (this.IsPropertyDef(name)) return true;
            if (this.IsPropertyBase(name)) return true;
            if (this.IsUseProperty(name)) return true;
            if (this.IsUseEnumItem(name)) return true;
            return false;
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

        Dictionary<string, SymbolLocalVar> localDefDict = new Dictionary<string, SymbolLocalVar>();

        

        public bool IsDefLocal(string name)
        {
            return localDefDict.ContainsKey(name);
        }

        public SymbolLocalVar GetDefLocal(string name)
        {
            return localDefDict[name];
        }

        public bool IsPropertyDef(string name)
        {
            //if (name.IndexOf("X速度") != -1)
            //{
            //    Debug.WriteLine("X速度");
            //}
            ZClassCompilingType zct = this.ClassContext.GetZCompilingType();//.ThisCompilingType;
            foreach (var member in zct.ZMembers)
            {
                if (member.ContainsWord(name))
                    return true;
            }
            return false;
        }

        public bool IsPropertyBase(string name)
        {
            ZClassType zbase = this.ClassContext.GetSuperZType();
            if (zbase == null) return false;
            ZMemberInfo zmember = zbase.SearchZMember(name);
            return zmember != null;
        }

        public bool IsUseEnumItem(string name)
        {
            ContextImportUse contextiu = this.ClassContext.FileContext.ImportUseContext;
            return contextiu.IsUseEnumItem(name);
        }

        public bool IsUseProperty(string name)
        {
            ContextImportUse importUseContext = this.ClassContext.FileContext.ImportUseContext;
            return importUseContext.IsUseProperty(name);
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
            ZCallDesc calldesc = new ZCallDesc();
            calldesc.Add(name);
            //ZClassCompilingType zcc = this.ClassContext.ThisCompilingType;
            var methods = this.ClassContext.SearchThisProc(calldesc);
            return methods.Length > 0;
        }

        public bool IsSuperMethodSingle(string name)
        {
            if (this.IsStatic()) return false;
            ZCallDesc calldesc = new ZCallDesc();
            calldesc.Add(name);
            var methods = this.ClassContext.SearchSuperProc(calldesc);
            return methods.Length > 0;
        }

        public bool IsUseMethodSingle(string name)
        {
            ContextImportUse importUseContext = this.ClassContext.FileContext.ImportUseContext;
            //ContextUse cu = this.ClassContext.FileContext.UseContext;
            ZCallDesc calldesc = new ZCallDesc();
            calldesc.Add(name);
            var methods = importUseContext.SearchUseMethod(calldesc);
            return methods != null && methods.Length>0;
        }

        public void AddDefSymobl(SymbolArg argSymbol)
        {
            this.ProcSegmenter.AddWord(argSymbol.Name);
            _argDefDict.Add(argSymbol.Name, argSymbol);
        }

        public void AddDefSymbol(SymbolLocalVar localSymbol)
        {
            this.ProcSegmenter.AddWord(localSymbol.Name);
            localDefDict.Add(localSymbol.Name, localSymbol);
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
                if(IsStatic())
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

        int EachIndex = -1;
        //public List<string> ArgList = new List<string>();
        public int CreateEachIndex( )
        {
            EachIndex ++;
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

        private ProcEmitContext EmitContext { get; set; }
        public bool IsStatic()
        {
            return this.ClassContext.IsStatic();
        }

        private int NestedIndex = 0;
        public string CreateNestedClassName()
        {
            NestedIndex++;
            return (ProcName ?? "") + "Nested" + NestedIndex;
        }

        #region Compiling

        ZMethodCompiling methodCompiling;

        public void SetMethodCompiling(ZMethodCompiling methodCompiling)
        {
            this.methodCompiling = methodCompiling;
            this.ClassContext.AddMethod(methodCompiling);
            this.ProcDesc = methodCompiling.ZDesces[0];
        }

        #endregion

        #region Builder Emit

        public void SetBuilder(MethodBuilder methodBuilder)
        {
            bool isStatic = this.IsStatic();
            this.EmitContext.SetBuilder(methodBuilder);
            if (!IsConstructor)
            {
                this.methodCompiling.SetBuilder(methodBuilder);
                this.ProcDesc.ZMethod =
                    new ZMethodInfo(methodBuilder, isStatic, new ZMethodDesc[] { ProcDesc },
                        AccessAttributeEnum.Public);
                //ZClassCompilingType zct = this.ClassContext.GetZCompilingType();
                //zct.SetMemberBuilder();
            }
        }

        public void SetBuilder(ConstructorBuilder constructorBuilder)
        {
            bool isStatic = this.IsStatic();
            this.EmitContext.SetBuilder(constructorBuilder);
        }

        public ParameterBuilder DefineParameter(int position, string strParamName)
        {
            if(this.EmitContext.CurrentMethodBuilder!=null)
            {
                ParameterBuilder pb = this.EmitContext.CurrentMethodBuilder.DefineParameter(position, ParameterAttributes.None, strParamName);
                return pb;
            }
            else
            {
                ParameterBuilder pb = this.EmitContext.CurrentConstructorBuilder.DefineParameter(position, ParameterAttributes.None, strParamName);
                return pb;
            }
        }

        public ILGenerator GetILGenerator()
        {
            return this.EmitContext.ILout;
        }

        #endregion

        public class ProcEmitContext
        {
            public MethodBuilder CurrentMethodBuilder { get; private set; }
            public ConstructorBuilder CurrentConstructorBuilder { get; private set; }
            //public string ContextKey { get; private set; }

            public ProcEmitContext( )
            {

            }

            public void SetBuilder(MethodBuilder methodBuilder)
            {
                CurrentMethodBuilder = methodBuilder;
                ILout = methodBuilder.GetILGenerator();
            }

            public void SetBuilder(ConstructorBuilder constructorBuilder)
            {
                CurrentConstructorBuilder = constructorBuilder;
                ILout = constructorBuilder.GetILGenerator();
            }

            public ILGenerator ILout { get;private set; }
        }

        
    }
}
