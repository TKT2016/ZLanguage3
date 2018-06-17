using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Utils;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    /// <summary>
    /// Z枚举型(成员只有枚举值)
    /// </summary>
    public class ZLEnumInfo : ZLType, IZLObj
    {
        public bool IsRuntimeType { get { return false; } }
        public ZEnumAttribute MarkAttribute { get; protected set; }
        public ZLEnumItemInfo[] EnumElements { get; protected set; }
        public Type MarkType { get; protected set; }
        public Type SharpType { get; protected set; }
        public AccessAttrEnum AccessAttribute { get; protected set; }
        public AccessAttrEnum GetAccessAttr() { return AccessAttribute; }
        public bool IsStruct { get { return false; } }
        public ZLEnumInfo(Type type)
        {
            MarkType = type;
            MarkAttribute = AttributeUtil.GetAttribute<ZEnumAttribute>(type);
            if (MarkAttribute.ForType == null)
            {
                SharpType = type;
            }
            else
            {
                SharpType = MarkAttribute.ForType;
            }

            EnumElements = GetEnumElements(MarkType, SharpType);
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(type);
        }

        public ZLEnumItemInfo SearchValue(string zname)
        {
            foreach (ZLEnumItemInfo item in EnumElements)
            {
                if (item.HasZName(zname))
                    return item;
            }
            return null;
        }

        protected ZLEnumItemInfo[] GetEnumElements(Type defType, Type sharpType)
        {
            List<ZLEnumItemInfo> flist = new List<ZLEnumItemInfo>();

            FieldInfo[] fields = ZClassUtil.GetEnumItems(defType);
            foreach (FieldInfo field in fields)
            {
                if(AttributeUtil.HasAttribute<ZCodeAttribute>(field))
                {
                    FieldInfo sharpField = this.SharpType.GetField(field.Name);
                    ZLEnumItemInfo exField = new ZLEnumItemInfo(this, field,sharpField);
                    flist.Add(exField);
                }
            }
            return flist.ToArray();
        }

        public bool ZEquals(ZType zclass)
        {
            if (zclass is ZLEnumInfo)
            {
                ZLEnumInfo z2 = (zclass as ZLEnumInfo);
                if (z2.SharpType == this.SharpType) return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this.MarkType.Name + "-" + this.SharpType.Name;
        }

        public virtual string ZTypeName { get { return MarkType.Name; } }
        public virtual bool IsMarkSelf { get { return MarkType == SharpType; } }
    }
}
