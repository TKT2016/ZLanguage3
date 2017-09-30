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
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class SectionConstructorDefault : SectionBase
    {
        public ContextProc ProcContext;

        public SectionConstructorDefault()
        {
            
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
                methodAttributes = MethodAttributes.Public ;//| MethodAttributes.Virtual;
                callingConventions = CallingConventions.HasThis;
            }
            var argTypes = new Type[] { };
            ConstructorBuilder constructorBuilder = classBuilder.DefineConstructor(methodAttributes, callingConventions, argTypes);
            ProcContext.EmitContext.SetBuilder(constructorBuilder);
            ProcContext.EmitContext.ILout = constructorBuilder.GetILGenerator();
        }

        public void AnalyBody()
        {
       
        }

        public void EmitBody()
        {
            ILGenerator IL = this.ProcContext.EmitContext.ILout;
            if (!this.ProcContext.IsStatic)
            {
                EmitHelper.EmitCallBaseConstructorZero(IL, this.ProcContext.ClassContext.BaseZType.SharpType);
            }
            if (this.ProcContext.ClassContext.InitPropertyMethod != null)
                EmitHelper.CallDynamic(IL, this.ProcContext.ClassContext.InitPropertyMethod);
            ProcContext.EmitContext.ILout.Emit(OpCodes.Ret);
        }
    }
}
