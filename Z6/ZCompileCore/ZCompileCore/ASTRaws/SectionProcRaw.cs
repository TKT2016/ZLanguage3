using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class SectionProcRaw:SectionRaw
    {
        public ProcNameRaw NamePart;
        public LexTokenText RetToken;
        public StmtBlockRaw Body;

        public bool IsConstructor
        {
            get{
                return NamePart.IsConstructor();
            }
        }
    }
}
