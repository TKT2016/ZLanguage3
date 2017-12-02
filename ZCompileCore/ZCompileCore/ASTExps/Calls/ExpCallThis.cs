using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;
using System.Reflection;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.AST
{
    public class ExpCallThis : ExpCallAnalyedBase
    {
        //ZMethodDesc SearchedProcDesc;
        ZMethodInfo ZMethod;
        public ExpCallThis(ContextExp context, ZCallDesc expProcDesc, ZMethodInfo searchedMethod, Exp srcExp, List<Exp> argExps)
        {
            this.ExpContext = context;
            this.ExpProcDesc = expProcDesc;
            this.ZMethod = searchedMethod;
            this.SrcExp = srcExp;
            this.ArgExps = argExps;
        }

        public override Exp Analy( )
        {
            RetType = this.ZMethod.RetZType;
            return this;
        }

        public override void Emit()
        {
            EmitSubject();
            EmitArgsThis(ZMethod, ArgExps);
            EmitHelper.CallDynamic(IL, ZMethod.SharpMethod);
            EmitConv();
        }

        protected void EmitArgsThis(ZMethodInfo zdesc, List<Exp> expArgs)
        {
            var paramArr = zdesc.DefArgs.ToArray();
            List<Exp> expArgsNew = CallAjuster.AdjustExps(paramArr, expArgs);
            EmitArgsExp(paramArr, expArgs.ToArray());
        }

        protected void EmitArgsExp(ZParam[] paramInfos, Exp[] args)
        {
            var size = paramInfos.Length;

            for (int i = 0; i < size; i++)
            {
                Exp argExp = args[i];
                var parameter = paramInfos[i];
                argExp.RequireType = parameter.ZParamType;
                argExp.Emit();
            }
        }

        private void EmitSubject()
        {
            if(IsNested && this.ClassContext.NestedOutFieldSymbol!=null)
            {
                IL.Emit(OpCodes.Ldarg_0);
                EmitSymbolHelper.EmitLoad(IL, this.ClassContext.NestedOutFieldSymbol);// EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
            }
            else if (ZMethod.IsStatic==false)
            {
                IL.Emit(OpCodes.Ldarg_0);
            }
            else
            {
                //...
            }
        }

    }
}
