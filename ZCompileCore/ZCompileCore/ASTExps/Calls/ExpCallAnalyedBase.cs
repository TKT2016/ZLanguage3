﻿using System;
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
            if (defArgs.Count != expArgs.Count) throw new CCException();
            List<Exp> newExpArgs = new List<Exp>();
            int size = defArgs.Count;
            for (int i = 0; i < size; i++)
            {
                var defArg = defArgs[i];
                var expArg = expArgs[i];
                if (defArg.IsGenericArg ==false )
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
                else
                {
                    newExpArgs.Add(expArg);
                }
            }
            return newExpArgs;
        }

        #region 辅助
        public override string ToString()
        {
            return SrcExp.ToString();
        }

        public override CodePosition Position
        {
            get
            {
                return SrcExp.Position; ;
            }
        }
        #endregion
    }
}
