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
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class ExpCallAnalyedBase:Exp
    {
        protected ZCallDesc ExpProcDesc;
        protected Exp SrcExp;
        protected List<Exp> ArgExps { get; set; }

        public override Exp[] GetSubExps()
        {
            return SrcExp.GetSubExps();
        }

        protected List<Exp> AnalyArgLambda(List<ZParam> defArgs, List<Exp> expArgs)
        {
            if (defArgs.Count != expArgs.Count) throw new CompileCoreException();
            List<Exp> newExpArgs = new List<Exp>();
            int size = defArgs.Count;
            for (int i = 0; i < size; i++)
            {
                var defArg = defArgs[i];
                var expArg = expArgs[i];
                if (defArg.IsGeneric ==false )
                {
                    if (ZLambda.IsFn(defArg.ZParamType.SharpType))
                    {
                        ExpNewLambda newLambdaExp = new ExpNewLambda(expArg, defArg.ZParamType);
                        newLambdaExp.SetContext(this.ExpContext);
                        Exp exp2 = newLambdaExp.Analy();
                        newExpArgs.Add(exp2);
                    }
                    else
                    {
                        newExpArgs.Add(expArg);
                    }
                }
            }
            return newExpArgs;
        }

        //protected void AnalyArgLambda(List<ZParam> defArgs, List<Exp> expArgs)
        //{
        //    for (int i = 0; i < ExpProcDesc.CallArgs.Count; i++)
        //    {
        //        var procArg = defArgs[i];
        //        if (procArg.IsGeneric == false)
        //        {
        //            if (ZLambda.IsFn(procArg.ZParamType.SharpType))
        //            {
        //                Exp exp = expArgs[i];
        //                ExpNewLambda newLambdaExp = new ExpNewLambda(exp, procArg.ZParamType);
        //                newLambdaExp.SetContext(this.ExpContext);
        //                newLambdaExp.Analy();
        //            }
        //        }
        //    }
        //}

        #region 辅助
        public override string ToString()
        {
            return SrcExp.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return SrcExp.Postion; ;
            }
        }
        #endregion
    }
}
