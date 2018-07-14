using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAMethodInfo : ZAProcInfo
    {
        public abstract AccessAttrEnum GetAccessAttr();
        public abstract bool GetIsStatic();
        //public abstract ZAClassInfo GetZAClass();
        public abstract ZType GetZRetType();
        //public abstract ZAParamInfo[] GetZParams();
        //public abstract ZAMethodDesc[] GetZDescs();
        public abstract string GetSharpName();
    }
}
