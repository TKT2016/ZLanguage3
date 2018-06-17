using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;

namespace ZCompileCore.AST
{
    public class SectionExtendsEnum
    {
        public SectionExtendsRaw Raw;

        public SectionExtendsEnum(SectionExtendsRaw raw)
        {
            Raw = raw;
        }

    }
}
