using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.AST;
using ZCompileCore.ASTExps;

namespace ZCompileCore.Parsers
{
    public class ExpTape : ArrayTape<Exp>
    {
        ContextFile fileContext;

        public ExpTape(List<Exp> tokens, ContextFile fileContext)
            : base(tokens.ToArray(),null)
        {
            this.fileContext = fileContext;
        }

        //public void error(Token tok, string str)
        //{
        //   this.fileContext.Errorf(tok.Position, str);
        //}

        //public void error(string str)
        //{
        //    error(this.Current, str);
        //}
    }
}
