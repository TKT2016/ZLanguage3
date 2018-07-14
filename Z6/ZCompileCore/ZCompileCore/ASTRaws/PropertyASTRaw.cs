using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class PropertyASTRaw
    {
        public LexTokenText NameToken;
        public ExpRaw ValueExp;
    }
}
