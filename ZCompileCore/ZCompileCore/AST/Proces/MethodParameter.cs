using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class MethodParameter
    {
        private MethodName MethodNameAST;
        private ProcNameRaw.ProcParameter ParameterRaw;
        private string ArgText;
        private bool _IsExist;
        private string ArgZTypeName;
        private string ArgName;
        private ZType ArgZType;
        private ZCParamInfo _ZCParam;
        private ContextMethod MethodContext;
        private ContextFile FileContext;

        public MethodParameter(MethodName methodNameAST, ProcNameRaw.ProcParameter raw)
        {
            MethodNameAST = methodNameAST;
            ParameterRaw = raw;

            MethodContext = this.MethodNameAST.MethodAST.MethodContext;
            FileContext = this.MethodNameAST.MethodAST.ASTClass.FileContext;
        }

        public void Analy()
        {
            ArgText = ParameterRaw.ParameterToken.Text;
            if (MethodContext.HasParameter(ArgText))
            {
                _IsExist = true;
                FileContext.Errorf(ParameterRaw.ParameterToken.Position, "参数'{0}'重复", ArgText);
            }
            else
            {
                AnalyType();
            }
        }

        private void AnalyType()
        {
            if (_IsExist) return;
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            string[] names = contextiu.GetArgSegementer().Cut(ArgText);
            if (names.Length != 2) throw new CCException();
            ArgZTypeName = names[0];
            ArgName = names[1];
            if (this.MethodContext.HasParameter(ArgName))
            {
                _IsExist = true;
                CodePosition argPos = new CodePosition(ParameterRaw.ParameterToken.Line, ParameterRaw.ParameterToken.Col + ArgZTypeName.Length);
                FileContext.Errorf(argPos, "参数'{0}'重复", ArgName);
            }
            else
            {
                ZType[] ztypes = contextiu.SearchZTypesByClassNameOrDimItem(ArgZTypeName);
                ArgZType = ztypes[0];
                //_argSymbol = new SymbolArg(ArgName, ArgZType);
                //this.ProcContext.AddParameter(_argSymbol);
                //_argSymbol = new ZCParamInfo(ArgName, ArgZType);
                //ProcContext.AddParameter(_argSymbol);
                _ZCParam = MethodContext.AddParameterName(ArgName);
                _ZCParam.ZParamType = ArgZType;
            }
        }

        public ZCParamInfo GetZParam()
        {
            //if (_isexist) return null;
            //if (_ZParam == null)
            //{
            //    _ZParam = new ZCParamInfo() { ZParamName =ArgName, ZParamType =ArgZType };//(this.ArgName, this.ArgZType);
            //}
            return _ZCParam;
        }

        public void EmitName()
        {
            //if (_isexist) return;
            this.MethodContext.DefineParameter(_ZCParam);
        }
    }
}
