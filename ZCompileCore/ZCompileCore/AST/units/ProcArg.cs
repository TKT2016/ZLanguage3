using System;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class ProcArg : SectionPartProc
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
            //if (ArgText == "物体W")
            //{
            //    Console.WriteLine("物体W");
            //}
            if (this.ProcContext.ContainArgtext(ArgText))
            {
                _isexist = true;
                ErrorF(ArgToken.Position, "参数'{0}'重复", ArgText);
            }
            else
            {
                this.ProcContext.AddArgtext(ArgText);
            }
        }

        public override void AnalyType()
        {
            if (_isexist) return;
            //if (ArgText == "物体W")
            //{
            //    Console.WriteLine("物体W");
            //}
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            string[] names = contextiu.GetArgSegementer().Cut(ArgText);
            if (names.Length != 2) throw new CCException();
            ArgZTypeName = names[0];
            ArgName = names[1];
            if (this.ProcContext.ContainArg(ArgName))
            {
                _isexist = true;
                CodePosition argPos = new CodePosition(ArgToken.Line, ArgToken.Col + ArgZTypeName.Length);
                ErrorF(argPos, "参数'{0}'重复", ArgName);
            }
            else
            {
                ZType[] ztypes = contextiu.SearchZTypesByClassNameOrDimItem(ArgZTypeName);
                ArgZType = ztypes[0];
                _argSymbol = new SymbolArg(ArgName, ArgZType);
                this.ProcContext.AddArg(_argSymbol);
            }
        }
        SymbolArg _argSymbol;
        public override void AnalyBody()
        {
            return;
        }

        ParameterBuilder ParamBuilder;
        public override void EmitName()
        {
            if (_isexist) return;
            //var methodBuilder = this.ProcContext.EmitContext.CurrentMethodBuilder;
            ParamBuilder = this.ProcContext.DefineParameter(this.GetArgIndex(), ArgName);
            //ZParam zparam = this.GetZParam();
            //if (!zparam.IsGeneric)
            //{
            //    ParamBuilder = methodBuilder.DefineParameter(this.GetArgIndex(), ParameterAttributes.None, ArgName);
            //}
        }

        public override void EmitBody()
        {
            return;
        }

        //private int _ArgIndex;
        public void SetArgIndex(int i)
        {
            //_ArgIndex = i;
            this._argSymbol.ArgIndex = i;
        }

        public int GetArgIndex()
        {
            return this._argSymbol.ArgIndex;
        }

        public void SetContext(ContextProc procContext)
        {
            this.ProcContext = procContext;
            this.ClassContext = this.ProcContext.ClassContext;
            this.FileContext = this.ClassContext.FileContext;
        }


        ZParam _ZParam;
        public ZParam GetZParam()
        {
            if (_isexist) return null;
            if (_ZParam == null)
            {
                _ZParam = new ZParam(this.ArgName, this.ArgZType);
            }
            return _ZParam;
        }

        public override string ToString()
        {
            return string.Format("({0})", ArgToken.GetText());
        }
    }
}
