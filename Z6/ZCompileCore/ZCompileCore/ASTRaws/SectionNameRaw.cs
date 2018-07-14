using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class SectionNameRaw : SectionRaw
    {
        public LexTokenText KeyToken { get; set; }
        public LexTokenText NameToken { get; set; }
    }
}
