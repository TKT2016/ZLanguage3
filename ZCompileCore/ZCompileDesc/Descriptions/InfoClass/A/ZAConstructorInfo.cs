using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAConstructorInfo : ZAProcInfo
    {
        public abstract AccessAttrEnum GetAccessAttr();
        public abstract bool GetIsStatic();
        //public abstract ZAClassInfo GetZAClass();
        //public abstract ZAParamInfo[] GetZParams();
        //public abstract ZAConstructorDesc GetZDesc();
    }
}
