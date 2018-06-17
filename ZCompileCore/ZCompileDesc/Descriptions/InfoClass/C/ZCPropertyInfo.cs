using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCPropertyInfo : ZAPropertyInfo,ICompling
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return AccessAttrEnum.Public; }
        public override bool GetIsStatic() { return IsStatic; }
        public override bool GetCanRead() { return true; }
        public override bool GetCanWrite() { return true; }
        public override string[] GetZPropertyZNames() { return new string[] { ZPropertyZName }; }
        public override ZType GetZPropertyType() { return this.ZPropertyType; }
        public override ZAClassInfo GetZAClass() { return this.ZClass; }

        #endregion

        public PropertyBuilder PropertyBuilder { get; set; }
        public bool IsStatic { get; set; }
        public string ZPropertyZName { get; set; }
        public ZCClassInfo ZClass { get; set; }
        public ZType ZPropertyType { get; set; }
        public bool HasDefaultValue { get; set; }

        

        public override string ToString()
        {
            return "ZCPropertyInfo(" + ZPropertyZName + ")";
        }
    }
}
