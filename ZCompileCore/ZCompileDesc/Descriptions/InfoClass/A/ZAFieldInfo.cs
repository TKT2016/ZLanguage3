using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAFieldInfo:IIdent
    {
        public abstract AccessAttrEnum GetAccessAttr();
        public abstract bool GetIsStatic();
        public abstract bool GetCanRead();
        public abstract bool GetCanWrite();
        public abstract string[] GetZFieldZNames();
        public abstract ZType GetZFieldType();
        public abstract ZAClassInfo GetZAClass();
        public string ZName { get { return GetZFieldZNames()[0]; } }
        //public bool IsStruct { get { return ZTypeUtil.IsStruct(this.GetZType()); } }
        public ZType GetZType() { return GetZFieldType(); }

        public virtual bool HasZName(string zname)
        {
            foreach (var item in GetZFieldZNames())
            {
                if (item == zname)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
