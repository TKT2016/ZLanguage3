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
    public abstract class ProcConstructorBase : ProcAST
    {
        public ContextConstructor ConstructorContext { get; protected set; }

        public abstract void EmitName();
        public abstract void EmitBody();
        public override ContextProc GetContextProc()
        {
            return ConstructorContext;
        }
        public abstract void AnalyExpDim();
        protected void EmitCallInitPropertyMethod(ILGenerator IL)
        {
            if (this.ClassContext.InitPropertyMethod != null)
            {
                bool isStatic = (this.ClassContext.IsStatic());
                var method = this.ClassContext.InitPropertyMethod;
                if (!isStatic)
                {
                    IL.Emit(OpCodes.Ldarg_0);
                }
                IL.Emit(OpCodes.Call, method);
            }
        }

        protected void EmitCallSuper(ILGenerator IL)
        {
            bool isStatic = (this.ClassContext.IsStatic());
            if (!isStatic)
            {
                ConstructorInfo superConstruct = this.ClassContext.GetSuperZType().SharpType.GetConstructor(Type.EmptyTypes);
                if (superConstruct != null)
                {
                    IL.Emit(OpCodes.Ldarg_0);
                    IL.Emit(OpCodes.Call, superConstruct);
                }
            }
        }
    }
}
