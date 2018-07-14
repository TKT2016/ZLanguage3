
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.AST.Exps
{
    public abstract class ExpTypeBase : Exp
    {
        public abstract LexToken GetMainToken();
        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        //public ExpTypeBase(Exp parentExp)
        //    : base(parentExp)
        //{

        //}

        public ExpTypeBase(ContextExp expContext)
            : base(expContext)
        {

        }
    }
}
