using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Lex;

namespace ZCompileCore.AST
{
    public class ExpEachWord:Exp
    {
        public Token EachToken { get; set; }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }
        public override CodePosition Postion
        {
            get {return EachToken.Position; }
        }

        public override string ToString()
        {
            return "每一个";
        }
    }
}
