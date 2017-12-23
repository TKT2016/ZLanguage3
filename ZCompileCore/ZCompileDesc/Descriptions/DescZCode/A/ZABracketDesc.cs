using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZABracketDesc : IDesc
    {
        public abstract string ToZCode();
        //public abstract ZAParamInfo[] GetZParams();
        public abstract int GetParametersCount();
    }
}
