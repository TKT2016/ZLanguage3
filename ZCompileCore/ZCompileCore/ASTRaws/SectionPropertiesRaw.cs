using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class SectionPropertiesRaw : SectionRaw
    {
        public LexTokenText KeyToken;
        public List<PropertyASTRaw> Properties = new List<PropertyASTRaw>();
    }
}
