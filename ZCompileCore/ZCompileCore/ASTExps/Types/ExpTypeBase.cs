
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpTypeBase:Exp
    {
        public abstract LexToken GetMainToken();
        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }
    }
}
