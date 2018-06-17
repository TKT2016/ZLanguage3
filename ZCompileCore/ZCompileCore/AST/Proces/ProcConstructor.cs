using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ProcConstructor : ProcConstructorBase
    {
        private ProcNameRaw.NameBracket NameBracketRaw;
        private List<ConstructorParameter> ConstructorParameterList;

        public override void AnalyBody()
        {
            if (ConstructorContext==null)
                ConstructorContext = new ContextConstructor(this.ASTClass.ClassContext);
            this.Body.Analy();
        }

        public ProcConstructor(ClassAST classAST,SectionProcRaw raw)
        {
            RetZType = ZLangBasicTypes.ZVOID;
            ASTClass = classAST;
            Raw = raw;
            NameBracketRaw = Raw.NamePart.GetNameBracket();
            ConstructorContext = new ContextConstructor(ASTClass.ClassContext);
            ConstructorParameterList = new List<ConstructorParameter>();
            foreach(var item in NameBracketRaw.Parameters)
            {
                ConstructorParameter cp = new ConstructorParameter(this, item);
                ConstructorParameterList.Add(cp);
            }
            Body = new StmtBlock(this, Raw.Body);
        }

        public ProcConstructor(ClassAST classAST )
        {
            ASTClass = classAST;
        }

        public override void AnalyExpDim()
        {
            this.Body.AnalyExpDim();
        }

        public override void EmitBody()
        {
            ILGenerator IL = this.ConstructorContext.GetILGenerator();
            List<ZCLocalVar> localList = this.ConstructorContext.LocalManager.LocalVarList;
            EmitLocalVar(IL,localList);
            EmitCallSuper(IL);
            EmitCallInitPropertyMethod(IL);
            Body.Emit();
            IL.Emit(OpCodes.Ret);
            CreateNestedType();
        }

        public override void EmitName()
        {
            var classBuilder = this.ConstructorContext.ClassContext.GetTypeBuilder();
            bool isStatic = this.ConstructorContext.IsStatic();
            MethodAttributes methodAttributes;
            CallingConventions callingConventions;

            if (isStatic)
            {
                methodAttributes = MethodAttributes.Private | MethodAttributes.Static;
                callingConventions = CallingConventions.Standard;
            }
            else
            {
                methodAttributes = MethodAttributes.Public;
                callingConventions = CallingConventions.HasThis;
            }
            var argTypes = this.ConstructorContext.ZConstructorInfo.GetParameterTypes();// this.constructorDesc.GetParamTypes();
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ConstructorContext.SetBuilder(constructorBuilder);

            var normalArgs = this.ConstructorContext.ZConstructorInfo.GetNormalParameters();
            int start_i = isStatic ? 0 : 1;
            for (var i = 0; i < normalArgs.Length; i++)
            {
                constructorBuilder.DefineParameter(i + start_i, ParameterAttributes.None, normalArgs[i].GetZParamName());
            }
        }

        public class ConstructorParameter
        {
            private ProcConstructor ConstructorAST;
            private ProcNameRaw.ProcParameter ParameterRaw;
            private string ArgText;
            private bool _IsExist ;
            private string ArgZTypeName;
            private string ArgName;
            private ZType ArgZType;
            private ZCParamInfo _ZCParam;

            public ConstructorParameter(ProcConstructor constructorAST, ProcNameRaw.ProcParameter raw)
            {
                ConstructorAST = constructorAST;
                ParameterRaw = raw;
            }

            public void Analy()
            {
                ArgText = ParameterRaw.ParameterToken.Text;
                if (this.ConstructorAST.ConstructorContext.HasParameter(ArgText))
                {
                    _IsExist = true;
                    ConstructorAST.ASTClass.ClassContext.FileContext.Errorf(ParameterRaw.ParameterToken.Position, "参数'{0}'重复", ArgText);
                }
                else
                {
                    AnalyType();
                }
            }

            private void AnalyType()
            {
                if (_IsExist) return;
                ContextImportUse contextiu = this.ConstructorAST.ASTClass.ClassContext.FileContext.ImportUseContext;
                string[] names = contextiu.GetArgSegementer().Cut(ArgText);
                if (names.Length != 2) throw new CCException();
                ArgZTypeName = names[0];
                ArgName = names[1];
                if (this.ConstructorAST.ConstructorContext.HasParameter(ArgName))
                {
                    _IsExist = true;
                    CodePosition argPos = new CodePosition(ParameterRaw.ParameterToken.Line, ParameterRaw.ParameterToken.Col + ArgZTypeName.Length);
                    ConstructorAST.ASTClass.ClassContext.FileContext.Errorf(argPos, "参数'{0}'重复", ArgName);
                }
                else
                {
                    ZType[] ztypes = contextiu.SearchZTypesByClassNameOrDimItem(ArgZTypeName);
                    ArgZType = ztypes[0];
                    //_argSymbol = new SymbolArg(ArgName, ArgZType);
                    //this.ProcContext.AddParameter(_argSymbol);
                    //_argSymbol = new ZCParamInfo(ArgName, ArgZType);
                    //ProcContext.AddParameter(_argSymbol);
                    _ZCParam = this.ConstructorAST.ConstructorContext.AddParameterName(ArgName);
                    _ZCParam.ZParamType = ArgZType;
                }
            }
        }
    }
}
