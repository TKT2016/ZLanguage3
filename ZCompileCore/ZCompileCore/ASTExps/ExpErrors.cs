using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpError : Exp
    {
        public ExpError()
        {
            RetType = ZLangBasicTypes.ZOBJECT;
            IsAnalyed = true;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }
    }

    public class ExpErrorToken : ExpError
    {
        public LexToken Token { get; private set; }

        public ExpErrorToken(LexToken token, ContextExp contextExp)
        {
            Token = token;
            this.ExpContext = contextExp;
        }
    }

    public class ExpErrorType : ExpError
    {
        public LexToken[] Tokens { get; private set; }

        public ExpErrorType(ContextExp contextExp,LexToken token )
        {
            this.ExpContext = contextExp;
            Tokens = new LexToken[] { token };
           
        }
    }
}
