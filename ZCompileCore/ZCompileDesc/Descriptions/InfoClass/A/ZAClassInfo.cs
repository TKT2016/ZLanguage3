using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAClassInfo : ZType
    {
        public abstract bool IsRuntimeType { get; }
        public abstract AccessAttrEnum GetAccessAttr();
        public abstract string GetZClassName();
        public abstract bool GetIsStatic();
        public abstract ZAClassInfo GetBaseZClass();
        public abstract ZAPropertyInfo[] GetZPropertys();
        public abstract ZAFieldInfo[] GetZFields();
        public abstract ZAConstructorInfo[] GetZConstructors();
        public abstract ZAMethodInfo[] GetZMethods();

        public abstract bool IsStruct { get; }

        public string ZTypeName
        {
            get
            {
                return GetZClassName();
            }
        }

    }
}
