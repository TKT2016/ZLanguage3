using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZNewCall  : ACall
    {
        public string ClassName { get;private set; }
        public ZBracketCall ZDesc { get; private set; }

        public ZNewCall(string className, ZBracketCall zvalue)
        {
            ZDesc = zvalue;
            ClassName = className;
        }

        public override string ToZCode()
        {
            return ZDesc.ToZCode();
        }

    }
}
