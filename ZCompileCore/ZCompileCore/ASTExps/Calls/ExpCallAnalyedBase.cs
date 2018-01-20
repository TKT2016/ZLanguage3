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
using ZCompileDesc.Utils;

namespace ZCompileCore.ASTExps
{
    public class ExpCallAnalyedBase:Exp
    {
        protected ZMethodCall ExpProcDesc;
        protected Exp SrcExp;
        protected List<Exp> ArgExps { get; set; }

        public override Exp[] GetSubExps()
        {
            return SrcExp.GetSubExps();
        }

        protected List<Exp> AnalyArgLambda(ZLParamInfo[] defArgs, List<Exp> expArgs)
        {
            if (defArgs.Length != expArgs.Count) throw new CCException();
            List<Exp> newExpArgs = new List<Exp>();
            int size = defArgs.Length;
            for (int i = 0; i < size; i++)
            {
                var defArg = defArgs[i];
                var expArg = expArgs[i];
                if (defArg.GetIsGenericParam() == false && ZTypeUtil.IsFn(defArg.ZParamType))
                {
                    throw new CCException("参数不能同时是泛型参数和lambda参数");
                }
                if (defArg.GetIsGenericParam() == false)
                {
                    newExpArgs.Add(expArg);
                }
                else if (ZTypeUtil.IsFn(defArg.ZParamType))
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
