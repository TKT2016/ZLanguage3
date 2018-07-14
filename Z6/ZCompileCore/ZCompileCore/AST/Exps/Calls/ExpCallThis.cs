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
using System.Reflection;

namespace ZCompileCore.AST.Exps
{
    public class ExpCallThis : ExpCallAnalyedBase
    {
        ZCMethodInfo ZMethod;
        public ExpCallThis(ContextExp context, ZMethodCall expProcDesc, ZCMethodInfo searchedMethod, Exp srcExp, List<Exp> argExps):base(context)
        {
            this.ExpProcDesc = expProcDesc;
            this.ZMethod = searchedMethod;
            this.SrcExp = srcExp;
            this.ArgExps = argExps;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
           
            RetType = this.ZMethod.RetZType;
            IsAnalyed = true;
            return this;
        }

        public override void Emit()
        {
            EmitLoadMain();
            EmitArgsThis(ZMethod, ArgExps);
            EmitHelper.CallDynamic(IL, ZMethod.MethodBuilder);
            EmitConv();
        }

        protected void EmitArgsThis(ZCMethodInfo zdesc, List<Exp> expArgs)
        {
            var paramArr = zdesc.ZParams;
            List<Exp> expArgsNew = CallAjuster.AdjustExps(paramArr, expArgs);
            EmitArgsExp(paramArr, expArgs.ToArray());
        }

        protected void EmitArgsExp(ZCParamInfo[] paramInfos, Exp[] args)
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
    }
}
