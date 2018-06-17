using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLEnumItemInfo : ICompleted//,ZMemberInfo
    {
        public ZLEnumInfo ZEnum { get; private set; }
        public FieldInfo MarkField { get; private set; }
        public FieldInfo SharpField { get; private set; }

        public ZLEnumItemInfo(ZLEnumInfo zenum, FieldInfo fieldInfo)
            : this(zenum,fieldInfo, fieldInfo)
        {
            
        }

        public ZLEnumItemInfo(ZLEnumInfo zenum,FieldInfo zfieldInfo, FieldInfo sfieldInfo )
        {
            MarkField = zfieldInfo;
            SharpField = sfieldInfo;
            ZEnum = zenum;
        }

        public bool GetIsStatic() { return true; }
        public bool CanRead() { return true; }
        public bool CanWrite() { return false; }
        public AccessAttrEnum GetAccessAttr() { return AccessAttrEnum.Public; }

        string[] _znames = null;
        public string[] GetZNames()
        {
            if (_znames == null)
            {
                _znames = ZClassUtil.GetZNames(MarkField);
            }
            return _znames;
        }

        public bool HasZName(string zname)
        {
            string[] znames = GetZNames();
            foreach(var item in znames)
            {
                if (item == zname)
                    return true;
            }
            return false;
        }

        public object Value
        {
            get
            {
                return SharpField.GetValue(null);
            }
        }

        public string SharpMemberName
        {
            get { return this.MarkField.Name; }
        }

        public ZType GetMemberZType()
        {
           return ZTypeManager.GetBySharpType(SharpField.FieldType) as ZType;
        }

        public override string ToString()
        {
            var names = GetZNames();
            return this.MarkField.Name + "(" + string.Join(",", names) + ")";
        }
    }
}
