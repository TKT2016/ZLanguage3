using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc;
using ZCompileDesc.Utils;

using ZCompileCore.AST.Exps;
using ZCompileCore.AST;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 二参数泛型
    /// </summary>
    public class ExpTypeThree : ExpTypeBase
    {
        public ZType VarZtype1 { get;private set; }
        public ZType VarZtype2 { get; private set; }
        public ZType VarZtype3 { get; private set; }
        public ZType VarZtypeCreated { get; private set; }
        public LexToken VarToken1 { get;private set; }
        public LexToken VarToken2 { get;private set; }
        public LexToken VarToken3 { get; private set; }

        public ExpTypeThree(ContextExp expContext,LexToken varToken1, 
            LexToken varToken2, LexToken varToken3, ZType varZtype1, 
            ZType varZtype2, ZType varZtype3, ZType varZtypeCreated)
            : base(expContext)
        {
            VarToken1 = varToken1;
            VarToken2 = varToken2;
            VarToken3 = varToken3;
            VarZtype1 = varZtype1;
            VarZtype2 = varZtype2;
            VarZtype3 = varZtype3;
            VarZtypeCreated = varZtypeCreated;
            this.RetType = varZtypeCreated;
        }

        public override LexToken GetMainToken()
        {
            return VarToken3;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            IsAnalyed = true;
            return this;
        }

        #region 覆盖

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        #endregion
    }
}
