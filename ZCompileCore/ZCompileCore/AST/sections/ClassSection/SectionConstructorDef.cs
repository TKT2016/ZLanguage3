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
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class SectionConstructorDef : SectionConstructorBase
    {
        public StmtBlock Body;
        //ZCConstructorDesc constructorDesc;
        ZCBracketDesc zbracket;
        ProcBracket BracketAST;
        List<ProcParameter> Args;// = new List<ProcArg>();
        //ZCConstructorInfo ConstructorInfo;

		public SectionConstructorDef(SectionProc proc)
        {
            BracketAST = (ProcBracket)(proc.NamePart.NameTerms[0]);
            //foreach (var item in bracketp.Args)
            //{
            //    Args.Add(item);
            //}
            Args = BracketAST.Args;
            this.Body = proc.Body;
            zbracket = new ZCBracketDesc();
            //constructorDesc = new ZCConstructorDesc() {ZDesc= zbracket };
            //ConstructorInfo = new ZCConstructorInfo();
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
            var argTypes = this.ProcContext.ZConstructorInfo.GetParameterTypes();// this.constructorDesc.GetParamTypes();
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ProcContext.SetBuilder(constructorBuilder);

            var normalArgs = this.ProcContext.ZConstructorInfo.GetNormalParameters();
            int start_i = isStatic ? 0 : 1;
            for (var i = 0; i < normalArgs.Length; i++)
            {
                constructorBuilder.DefineParameter(i + start_i, ParameterAttributes.None, normalArgs[i].GetZParamName());
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
            this.ProcContext = new ContextConstructor(this.ClassContext);
            BracketAST.SetContext(this.ProcContext);
            Body.ProcContext = this.ProcContext;
        }

        public override string ToString()
        {
            return BracketAST.ToString();
        }
    }
}
