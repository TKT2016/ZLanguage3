using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public abstract class ProcAST
    {
        public ClassAST ASTClass { get; protected set; }
        protected SectionProcRaw Raw;
        protected StmtBlock Body;
        public ZType RetZType { get; protected set; }

        public virtual void AnalyBody()
        {
            //this.Body.Analy();
            throw new CCException();
        }

        public abstract ContextProc GetContextProc();
        protected virtual void EmitLocalVar(ILGenerator IL,List<ZCLocalVar> localList)
        {
            BuilderUtil.EmitLocalVar(this.GetContextProc(), this.ClassContext.IsStatic(), IL, localList);
            //localList.Reverse();
            //for (int i = 0; i < localList.Count; i++)
            //{
            //    ZCLocalVar varSymbol = localList[i];
            //    varSymbol.VarBuilder = IL.DeclareLocal(ZTypeUtil.GetTypeOrBuilder(varSymbol.GetZType()));
            //    varSymbol.VarBuilder.SetLocalSymInfo(varSymbol.ZName);
            //}
            //for (int i = 0; i < localList.Count; i++)
            //{
            //    ZCLocalVar varSymbol = localList[i];
            //    if(varSymbol.IsNestedClassInstance)
            //    {
            //        LocalBuilder lanmbdaLocalBuilder = this.GetContextProc().NestedInstance.VarBuilder;
            //        ConstructorBuilder newBuilder = this.GetContextProc().GetNestedClassContext().DefaultConstructorBuilder;
            //         IL.Emit(OpCodes.Newobj, newBuilder);
            //        EmitHelper.StormVar(IL, lanmbdaLocalBuilder);
            //        if(! this.ClassContext.IsStatic() )
            //        {                 
            //            ILGeneratorUtil.LoadLocal(IL, lanmbdaLocalBuilder);
            //            IL.Emit(OpCodes.Ldarg_0);
            //            EmitSymbolHelper.EmitStorm(IL, this.GetContextProc().GetNestedClassContext().MasterClassField);
            //        }
            //    }
            //}
        }

        protected Type CreateNestedType()
        {
            if (this.GetContextProc().GetNestedClassContext() != null)
            {
                var classBuilder = this.GetContextProc().GetNestedClassContext().GetTypeBuilder();
                var NestedType = classBuilder.CreateType();
                return NestedType;
            }
            return null;
        }

        protected virtual ContextClass ClassContext
        {
            get { return this.ASTClass.ClassContext; }
        }
    }
}
