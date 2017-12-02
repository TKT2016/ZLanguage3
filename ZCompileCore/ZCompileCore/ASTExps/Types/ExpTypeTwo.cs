using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileCore.ASTExps;
using ZCompileCore.AST;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 一参数泛型
    /// </summary>
    public class ExpTypeTwo : ExpTypeBase
    {
        public ZType VarZtype1 { get;private set; }
        public ZType VarZtype2 { get; private set; }
        public ZType VarZtypeCreated { get; private set; }
        public LexToken VarToken1 { get;private set; }
        public LexToken VarToken2 { get;private set; }

        public ExpTypeTwo(LexToken varToken1, LexToken varToken2, ZType varZtype1, ZType varZtype2, ZType varZtypeCreated)
        {
            VarToken1 = varToken1;
            VarToken2 = varToken2;
            VarZtype1 = varZtype1;
            VarZtype2 = varZtype2;
            VarZtypeCreated = varZtypeCreated;
            this.RetType = varZtypeCreated;
        }

        public override LexToken GetMainToken()
        {
            return VarToken2;
        }

        public override Exp Analy( )
        {
            return this;
        }
        

        #region 覆盖

        //public override string ToString()
        //{
        //    return string.Join("", TypeTokens.Select(p=>p.GetText()));
        //}

        //public override CodePosition Position
        //{
        //    get
        //    {
        //        return TypeTokens[0].Position;
        //    }
        //}
        #endregion
    }
}
