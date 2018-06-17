using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST.Exps
{
    public class LambdaProcMethod // : ProcAST
    {
        //public ContextMethod MethodContext { get; protected set; }
        public ZCLocalVar RetSymbol;
        public ContextNestedClass NestedClassContext;
        public ContextMethod NestedMethodContext;
        public ContextExp NestedExpContext;
        public Exp ActionExp ;

        //public void AnalyBody()
        //{
        //    //this.Body.Analy();
        //}

        public void AnalyExpDim()
        {
            this.ActionExp.AnalyDim();
        }

        public void EmitBody()
        {
            var IL = this.NestedMethodContext.GetILGenerator();
            List<ZCLocalVar> localList = this.NestedMethodContext.LocalManager.LocalVarList;
            BuilderUtil.EmitLocalVar(NestedMethodContext, false , IL, localList);
            //EmitLocalVar(IL, localList);
            ActionExp.Emit();
            //if (!ZTypeUtil.IsVoid(this.RetZType))
            //{
            //    IL.Emit(OpCodes.Ldloc_0);
            //}
            if (this.RetSymbol == null)
            {
                if (!ZTypeUtil.IsVoid(this.ActionExp.RetType))
                {
                    IL.Emit(OpCodes.Pop);
                }
            }
            else
            {
                EmitHelper.StormVar(IL, this.RetSymbol.VarBuilder);
            }
            IL.Emit(OpCodes.Ret);
            //CreateNestedType();
        }

        //public override ContextProc GetContextProc()
        //{
        //    return NestedMethodContext;
        //}

        //protected override ContextClass ClassContext
        //{
        //    get { return this.NestedClassContext; }
        //}
    }
}
