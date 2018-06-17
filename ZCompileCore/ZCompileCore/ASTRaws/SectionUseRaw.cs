using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class SectionUseRaw : SectionRaw
    {
        public LexTokenText KeyToken;
        public List<LexTokenText> Parts = new List<LexTokenText>();
        

    }
}
