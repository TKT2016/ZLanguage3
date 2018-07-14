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
        public override string[] GetZFieldZNames() { return new string[] { FieldZName }; }
        public override ZType GetZFieldType() { return this.FieldZType; }
        public override ZAClassInfo GetZAClass() { return this.ZClass; }

        #endregion

        public FieldBuilder FieldBuilder { get; set; }
        public bool IsStatic { get; set; }
        public string FieldZName { get; set; }
        public ZCClassInfo ZClass { get; set; }
        public ZAClassInfo FieldZType { get; set; }
        public bool HasDefaultValue { get; set; }

        public ZCFieldInfo(string name, ZAClassInfo ztype, FieldBuilder builder)
        {
            FieldZName = name;
            FieldZType = ztype;
            FieldBuilder = builder;
        }

        public override string ToString()
        {
            return "ZCFieldInfo(" + FieldZName + ")";
        }
    }
}
