using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ACall : ICall
    {
        public abstract string ToZCode();
        //public abstract object[] GetParts();
        //public abstract int GetPartCount();

        public override string ToString()
        {
            return ToZCode();
        }
    }
}
