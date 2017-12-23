using System;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;

using ZCompileDesc.Descriptions;


namespace ZCompileCore.AST
{
    public class ProcParameter : SectionPartProc
    {
        public LexToken ArgToken;
        //public ContextProc ProcContext;

        string ArgText;

        string ArgName;
        string ArgZTypeName;
        ZType ArgZType;

        bool _isexist = false;
        public override void AnalyText()
        {
            ArgText = ArgToken.GetText();
            if (this.ProcContext.HasParameter(ArgText))
            {
                _isexist = true;
                ErrorF(ArgToken.Position, "参数'{0}'重复", ArgText);
            }
            
        }

        public override void AnalyType()
        {
            if (_isexist) return;
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            string[] names = contextiu.GetArgSegementer().Cut(ArgText);
            if (names.Length != 2) throw new CCException();
            ArgZTypeName = names[0];
            ArgName = names[1];
            if (this.ProcContext.HasParameter(ArgName))
            {
                _isexist = true;
                CodePosition argPos = new CodePosition(ArgToken.Line, ArgToken.Col + ArgZTypeName.Length);
                ErrorF(argPos, "参数'{0}'重复", ArgName);
            }
            else
            {
                ZType[] ztypes = contextiu.SearchZTypesByClassNameOrDimItem(ArgZTypeName);
                ArgZType = ztypes[0];
                //_argSymbol = new SymbolArg(ArgName, ArgZType);
                //this.ProcContext.AddParameter(_argSymbol);
                //_argSymbol = new ZCParamInfo(ArgName, ArgZType);
                //ProcContext.AddParameter(_argSymbol);
                _ZCParam = this.ProcContext.AddParameterName(ArgName);
                _ZCParam.ZParamType = ArgZType;
            }
        }
        //SymbolArg _argSymbol;
        ZCParamInfo _ZCParam;

        public override void AnalyBody()
        {
            return;
        }

        //ParameterBuilder ParamBuilder;
        public override void EmitName()
        {
            if (_isexist) return;
            this.ProcContext.DefineParameter(_ZCParam);
        //    //var methodBuilder = this.ProcContext.EmitContext.CurrentMethodBuilder;
        //    //ParamBuilder = this.ProcContext.DefineParameter(this.GetParameterIndex(), ArgName);
        //    //ZParam zparam = this.GetZParam();
        //    //if (!zparam.IsGeneric)
        //    //{
        //    //    ParamBuilder = methodBuilder.DefineParameter(this.GetArgIndex(), ParameterAttributes.None, ArgName);
        //    //}
        }

        public override void EmitBody()
        {
            return;
        }

        //private int _ArgIndex;
        public void SetParameterIndex(int i)
        {
            //_ArgIndex = i;
            this._ZCParam.ParamIndex = i;
        }

        //public int GetParameterIndex()
        //{
        //    return this._ZCParam.EmitIndex;
        //}

        public void SetContext(ContextProc procContext)
        {
            this.ProcContext = procContext;
            this.ClassContext = this.ProcContext.ClassContext;
            this.FileContext = this.ClassContext.FileContext;
        }

        //ZCParamInfo _ZParam;
        public ZCParamInfo GetZParam()
        {
            if (_isexist) return null;
            //if (_ZParam == null)
            //{
            //    _ZParam = new ZCParamInfo() { ZParamName =ArgName, ZParamType =ArgZType };//(this.ArgName, this.ArgZType);
            //}
            return _ZCParam;
        }

        public override string ToString()
        {
            return string.Format("({0})", ArgToken.GetText());
        }
    }
}
