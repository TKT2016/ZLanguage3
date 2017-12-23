using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCFieldInfo : ZAFieldInfo, ICompling
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return AccessAttrEnum.Public; }
        public override bool GetIsStatic() { return IsStatic; }
        public override bool GetCanRead() { return true; }
        public override bool GetCanWrite() { return true; }
        public override string[] GetZFieldZNames() { return new string[] { ZPropertyZName }; }
        public override ZType GetZFieldType() { return this.ZPropertyType; }
        public override ZAClassInfo GetZAClass() { return this.ZClass; }

        #endregion

        public FieldBuilder FieldBuilder { get; set; }
        public bool IsStatic { get; set; }
        public string ZPropertyZName { get; set; }
        public ZCClassInfo ZClass { get; set; }
        public ZAClassInfo ZPropertyType { get; set; }
        public bool HasDefaultValue { get; set; }

        public override string ToString()
        {
            return "ZCFieldInfo(" + ZPropertyZName + ")";
        }
    }
}
