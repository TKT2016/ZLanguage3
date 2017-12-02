
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpTypeSingleBase : ExpTypeBase
    {
        protected LexToken VarToken;
        protected string VarName;

        public override LexToken GetMainToken()
        {
            return VarToken;
        } 

        public override string ToString()
        {
            return VarToken.GetText();
        }
    }
}
