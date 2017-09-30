using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.ZMembers
{
    public class ZPropertyInfo : ZMemberInfo
    {
        public PropertyInfo MarkProperty { get; set; }
        public PropertyInfo SharpProperty { get; set; }

        public ZPropertyInfo(PropertyInfo propertyInfo)
        {
            MarkProperty = propertyInfo;
            SharpProperty = propertyInfo;
            Init();
        }

        public ZPropertyInfo(PropertyInfo markPropertyInfo,PropertyInfo sharpPropertyInfo)
        {
            MarkProperty = markPropertyInfo;
            SharpProperty = sharpPropertyInfo;
            Init();
        }

        private void Init()
        {
            if (MarkProperty.GetGetMethod() != null)
                IsStatic= MarkProperty.GetGetMethod().IsStatic;
            else
                IsStatic = MarkProperty.GetSetMethod().IsStatic;

            ZNames = ZDescriptionHelper.GetZNames(MarkProperty);
            CanRead = SharpProperty.GetGetMethod()!=null;
            CanWrite = SharpProperty.GetSetMethod() != null;

            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpProperty);
        }

        public override ZType MemberZType
        {
            get { return ZTypeManager.GetBySharpType(MarkProperty.PropertyType ) as ZType ; }
        }

        public override string SharpMemberName
        {
            get { return this.MarkProperty.Name; }
        }

        public override string ToString()
        {
            return this.MarkProperty.Name +"("+ string.Join(",",ZNames)+")";
        }
    }
}
