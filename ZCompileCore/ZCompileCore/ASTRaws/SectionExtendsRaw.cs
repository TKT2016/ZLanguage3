using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class SectionExtendsRaw : SectionRaw
    {
        public LexTokenText KeyToken;
        public LexTokenText BaseTypeToken;
    }
}
