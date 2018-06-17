using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class ExpRaw
    {
        public List<LexToken> RawTokens = new List<LexToken>();

        public override string ToString()
        {
            var texts = RawTokens.Select(p => p.Text);
            string str = string.Join("", texts);
            return str;
        } 
    }
}
