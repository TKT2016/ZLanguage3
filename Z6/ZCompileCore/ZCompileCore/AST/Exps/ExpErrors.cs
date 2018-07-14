using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;

namespace ZCompileCore.AST.Exps
{
    public abstract class ExpError : Exp
    {
        public ExpError(ContextExp expContext)
            : base(expContext)
        {
            RetType = ZLangBasicTypes.ZOBJECT;
            IsAnalyed = true;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }
    }

    public class ExpErrorToken : ExpError
    {
        public LexToken Token { get; private set; }

        public ExpErrorToken(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            Token = token;
        }

        public override Exp Analy()
        {
            Errorf(Token.Position, "无法识别'{0}'", Token.Text);
            return this;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }
    }

    public class ExpErrorType : ExpError
    {
        public LexToken[] Tokens { get; private set; }

        public ExpErrorType(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            Tokens = new LexToken[] { token };
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }
    }
}
