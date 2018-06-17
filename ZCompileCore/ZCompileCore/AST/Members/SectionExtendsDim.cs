using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;

namespace ZCompileCore.AST
{
    public class SectionExtendsDim
    {
        public SectionExtendsRaw Raw;
        //private DimAST dimAST;

        public SectionExtendsDim(SectionExtendsRaw raw)
        {
            Raw = raw;
        }
    }
}
