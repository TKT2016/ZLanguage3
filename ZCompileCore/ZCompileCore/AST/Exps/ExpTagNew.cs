using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.AST.Exps
{
    public class ExpTagNew:Exp
    {
        public LexTokenText KeyToken{ get; set; }

        //public ExpTagNew()
        //{

        //}

        //public ExpTagNew(Exp parentExp, ExpTypeBase typeExp, LexTokenText keyToken)
        //    : base(parentExp)
        //{
        //    KeyToken = keyToken;
        //}

        //public ExpTagNew(Exp parentExp, LexTokenText keyToken)
        //    : base(parentExp)
        //{
        //    KeyToken = keyToken;
        //}

        public ExpTagNew(ContextExp expContext, LexTokenText keyToken)
            : base(expContext)
        {
            KeyToken = keyToken;
        }

        public override Exp Analy( )
        {
            IsAnalyed = true;
            RetType = null;
            return this;
        }
       
        public override void Emit()
        {
            return;
        }
        
        public override string ToString()
        {
            return KeyToken.Text;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] {  };
        }
    }
}
