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
using ZCompileCore;
using ZCompileCore.AST;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 错误的类型
    /// </summary>
    public class ExpTypeNone : ExpTypeUnsure
    {
        Exp SrcExp;

        public ExpTypeNone(ContextExp expContext, Exp srcExp)
            : base(expContext)
        {
            this.SrcExp = srcExp;
            //this.ExpContext = srcExp.ExpContext;
            AnalyCorrect = false;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            AnalyCorrect = false;
            IsAnalyed = true;
            return this;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override void Emit()
        {
            throw new CCException();
        }
    }
}
