using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.ZTypes;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZArg
    {
        public bool IsGeneric { get; set; }
        public bool HasName { get { return !string.IsNullOrWhiteSpace(ZArgName); } }
        public string ZArgName { get; set; }
        public ZType ZArgType { get; set; }

        public ZArg()
        {
 
        }

        public ZArg(string name, ZType ztype)
        {
            ZArgName = name;
            ZArgType = ztype;
        }

        public string ToZCode()
        {
            if (HasName)
            {
                return string.Format("{0}={1}", ZArgName, ZArgType.ZName);
            }
            else
            {
                var ztype = ZArgType.ZName ;
                return ZArgType.ZName;
            }
        }

        public override string ToString()
        {
            return ToZCode();
        }
    }
}
