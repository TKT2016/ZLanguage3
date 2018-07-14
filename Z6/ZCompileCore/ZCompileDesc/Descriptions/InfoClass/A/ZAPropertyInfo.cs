using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAPropertyInfo:IIdent
    {
        public abstract AccessAttrEnum GetAccessAttr();
        public abstract bool GetIsStatic();
        public abstract bool GetCanRead();
        public abstract bool GetCanWrite();
        public abstract string[] GetZPropertyZNames();
        public abstract ZType GetZPropertyType();
        public abstract ZAClassInfo GetZAClass();
        //public bool IsStruct { get { return ZTypeUtil.IsStruct(this.GetZType()); } }
        public ZType GetZType() {  return GetZPropertyType();  }
        public string ZName { get { return GetZPropertyZNames()[0]; } }

        public virtual bool HasZName(string zname)
        {
            foreach(var item in GetZPropertyZNames())
            {
                if(item==zname)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
