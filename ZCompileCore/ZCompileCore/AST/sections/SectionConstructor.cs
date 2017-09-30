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
    public class SectionConstructor : SectionBase
    {
        public List<ProcArg> Args = new List<ProcArg>();
        public StmtBlock Body;

        public ContextProc ProcContext;
        ZConstructorDesc constructorDesc;
        ZBracketDefDesc zbracket;

		public SectionConstructor(SectionProc proc)
        {
            foreach (var item in proc.NamePart.NameTerms)
            {
                Args.Add(item as ProcArg);
            }
            this.Body = proc.Body;
            zbracket = new ZBracketDefDesc();
            constructorDesc = new ZConstructorDesc(zbracket);
        }

        public void AnalyName(NameTypeParser parser)
        {
           foreach(var arg in Args)
           {
               arg.ProcContext = this.ProcContext;
               arg.Analy(ProcContext,parser);
               ZParam zarg = arg.ZParam;
               zbracket.Add(zarg);
           }
        }

        public void EmitName()
        {
            var classBuilder = this.ProcContext.ClassContext.EmitContext.ClassBuilder;
            MethodAttributes methodAttributes;
            CallingConventions callingConventions;
            bool isSstatic = (this.ProcContext.IsStatic);
            if (isSstatic)
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
            ProcContext.EmitContext.SetBuilder(constructorBuilder);
            ProcContext.EmitContext.ILout = constructorBuilder.GetILGenerator();

            var normalArgs = this.constructorDesc.ParamsBracket.GetParamNormals();
            int start_i = isSstatic ? 0 : 1;
            for (var i = 0; i < normalArgs.Length; i++)
            {
                constructorBuilder.DefineParameter(i + start_i, ParameterAttributes.None, normalArgs[i].ZParamName);
            }
        }

        public void AnalyBody()
        {
            Body.ProcContext = this.ProcContext;
            Body.Analy();
        }

        public void EmitBody()
        {
            var IL = Body.IL;
            if (!this.ProcContext.IsStatic)
            {
                EmitHelper.EmitCallBaseConstructorZero(IL, this.ProcContext.ClassContext.BaseZType.SharpType);
            }
            Body.Emit();
            if (this.ProcContext.ClassContext.InitPropertyMethod != null)
                EmitHelper.CallDynamic(IL, this.ProcContext.ClassContext.InitPropertyMethod);
            ProcContext.EmitContext.ILout.Emit(OpCodes.Ret);
        }

    }
}
