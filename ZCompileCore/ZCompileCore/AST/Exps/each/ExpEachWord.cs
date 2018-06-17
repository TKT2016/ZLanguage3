using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.AST.Exps
{
    public class ExpEachWord:Exp
    {
        public LexToken EachToken { get; set; }

        public ExpEachWord(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }
        public override CodePosition Position
        {
            get {return EachToken.Position; }
        }

        public override string ToString()
        {
            return "每一个";
        }
    }
}
