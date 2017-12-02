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
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class SectionConstructorDefault : SectionConstructorBase
    {
        public SectionConstructorDefault()
        {
            
        }

        public override void AnalyText()
        {
            return;
        }

        public override void AnalyType()
        {
            return;
        }

        public override void AnalyBody()
        {
            return;
        }

        public override void EmitName()
        {
            bool isStatic = this.ProcContext.IsStatic();
            //var classContext = this.FileContext.ClassContext;
            var classBuilder = this.ClassContext.GetTypeBuilder();

            //var classBuilder = this.ProcContext.ClassContext.EmitContext.ClassBuilder;
            MethodAttributes methodAttributes;
            CallingConventions callingConventions;
            //bool isSstatic = (this.ProcContext.IsStatic);
            if (isStatic)
            {
                methodAttributes = MethodAttributes.Private | MethodAttributes.Static;
                callingConventions = CallingConventions.Standard;
            }
            else
            {
                methodAttributes = MethodAttributes.Public;//| MethodAttributes.Virtual;
                callingConventions = CallingConventions.HasThis;
            }
            var argTypes = new Type[] { };
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ProcContext.SetBuilder(constructorBuilder);
            //ProcContext.EmitContext.ILout = constructorBuilder.GetILGenerator();
        }

        public override void EmitBody()
        {
            ILGenerator IL = this.ProcContext.GetILGenerator();//.EmitContext.ILout;
            EmitCallSuper(IL);
            EmitCallInitPropertyMethod(IL);
            IL.Emit(OpCodes.Ret);
        }

        public override void SetContext(ContextClass classContext)
        {
            this.ClassContext = classContext;
            this.FileContext = this.ClassContext.FileContext;
            this.ProcContext = new ContextProc(this.ClassContext,true);
        }

        public override string ToString()
        {
            return "( )";
        }
    }
}
