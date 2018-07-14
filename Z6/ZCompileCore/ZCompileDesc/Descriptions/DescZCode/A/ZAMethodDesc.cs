using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAMethodDesc : IDesc
    {
        public abstract int GetPartCount();
        public abstract object[] GetParts();
        public abstract string[] GetTextParts();
        public abstract ZAParamInfo[] GetZParams();
        public abstract ZABracketDesc[] GetZBrackets();
        //public abstract ZAMethodInfo GetZMethod();
        public abstract string ToZCode();
    }
}
