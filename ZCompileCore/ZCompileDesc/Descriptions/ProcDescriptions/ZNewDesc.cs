using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Descriptions
{
    public class ZNewDesc:ICallDesc
    {
        public virtual ZBracketCallDesc ArgsBracket { get; protected set; }

        public ZNewDesc(ZBracketCallDesc zvalue)
        {
            ArgsBracket = zvalue;
        }

        public string ToZCode()
        {
            return ArgsBracket.ToZCode();
        }

    }
}
