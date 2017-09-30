using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZProcDescBase
    {
        public List<object> Parts { get;protected set; }
        public int PartsCount
        {
            get
            {
                return Parts.Count;
            }
        }

        public abstract string ToZCode();

        public override string ToString()
        {
            return ToZCode();
        }
    }
}
