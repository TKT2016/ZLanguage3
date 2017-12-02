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
    public abstract class SectionConstructorBase : SectionClassBase
    {
        public ContextProc ProcContext;

        public abstract void SetContext(ContextClass classContext);

        protected void EmitCallInitPropertyMethod(ILGenerator IL)
        {
            if (this.ProcContext.ClassContext.InitPropertyMethod != null)
            {
                bool isStatic = (this.ProcContext.ClassContext.IsStatic());
                var method = this.ProcContext.ClassContext.InitPropertyMethod;
                if (!isStatic)
                {
                    IL.Emit(OpCodes.Ldarg_0);
                }
                IL.Emit(OpCodes.Call, method);
            }
        }

        protected void EmitCallSuper(ILGenerator IL)
        {
            bool isStatic = (this.ProcContext.ClassContext.IsStatic());
            if (!isStatic)
            {
                ConstructorInfo superConstruct = this.ProcContext.ClassContext.GetSuperZType().SharpType.GetConstructor(Type.EmptyTypes);
                if (superConstruct != null)
                {
                    IL.Emit(OpCodes.Ldarg_0);
                    IL.Emit(OpCodes.Call,superConstruct);
                }
            }
        }
    }
}
