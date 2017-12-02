using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Tools;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class SectionConstructorDef : SectionConstructorBase
    {
        //public List<ProcArg> Args = new List<ProcArg>();
        public StmtBlock Body;
        ZConstructorDesc constructorDesc;
        ZBracketDefDesc zbracket;
        ProcBracket BracketAST;
        List<ProcArg> Args;// = new List<ProcArg>();

		public SectionConstructorDef(SectionProc proc)
        {
            BracketAST = (ProcBracket)(proc.NamePart.NameTerms[0]);
            //foreach (var item in bracketp.Args)
            //{
            //    Args.Add(item);
            //}
            Args = BracketAST.Args;
            this.Body = proc.Body;
            zbracket = new ZBracketDefDesc();
            constructorDesc = new ZConstructorDesc(zbracket);
        }

        public override void AnalyText()
        {
            BracketAST.AnalyText();
        }

        public override void AnalyType()
        {
            BracketAST.AnalyType();
        }

        public override void AnalyBody()
        {
            Body.Analy();
        }

        public override void EmitName()
        {
            var classBuilder = this.ClassContext.GetTypeBuilder();
            bool isStatic = this.ProcContext.IsStatic();
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
            var argTypes = this.constructorDesc.ParamsBracket.GetParamTypes();
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ProcContext.SetBuilder(constructorBuilder);
            //ProcContext.EmitContext.ILout = constructorBuilder.GetILGenerator();

            var normalArgs = this.constructorDesc.ParamsBracket.GetParamNormals();
            int start_i = isStatic ? 0 : 1;
            for (var i = 0; i < normalArgs.Length; i++)
            {
                constructorBuilder.DefineParameter(i + start_i, ParameterAttributes.None, normalArgs[i].ZParamName);
            }
        }

        public override void EmitBody()
        {
            var IL = Body.IL;
            EmitCallSuper(IL);
            EmitCallInitPropertyMethod(IL);
            Body.Emit();
            IL.Emit(OpCodes.Ret);
        }

        public override void SetContext(ContextClass classContext)
        {
            this.ClassContext = classContext;
            this.FileContext = this.ClassContext.FileContext;
            this.ProcContext = new ContextProc(this.ClassContext,true);
            BracketAST.SetContext(this.ProcContext);
            Body.ProcContext = this.ProcContext;
        }

        public override string ToString()
        {
            return BracketAST.ToString();
        }
    }
}
