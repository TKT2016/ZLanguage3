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
using ZCompileKit;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 无法确定的Call
    /// </summary>
    public class ExpCallNone : ExpCallAnalyedBase
    {
        public ExpCallNone(ContextExp context, ZMethodCall expProcDesc, Exp srcExp)
        {
            this.ExpContext = context;
            this.ExpProcDesc = expProcDesc;
            this.SrcExp = srcExp;
            AnalyCorrect = false;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            IsAnalyed = true;

            AnalyCorrect = false;
            return this;
        }

        public override void Emit()
        {
            throw new CCException();
        }
    }
}
