using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZLangRT.Utils;

namespace ZCompileDesc.ZMembers
{
    public class ZFieldInfo : ZMemberInfo
    {
        public FieldInfo MarkField { get; private set; }
        public FieldInfo SharpField { get; private set; }

        public ZFieldInfo(FieldInfo fieldInfo)
        {
            MarkField = fieldInfo;
            SharpField = fieldInfo;
            Init();
        }

        public ZFieldInfo(FieldInfo zfieldInfo, FieldInfo sfieldInfo)
        {
            MarkField = zfieldInfo;
            SharpField = sfieldInfo;
            Init();
        }

        private void Init()
        {
            IsStatic = SharpField.IsStatic;
            //ZNames = ZDescriptionHelper.GetZNames(MarkField);
            CanRead = true;
            CanWrite = !SharpField.IsInitOnly;
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpField);
        }

        string[] _znames = null;
        public override string[] GetZNames()
        {
            if (_znames == null)
            {
                _znames = ZDescriptionHelper.GetZNames(MarkField);
            }
            return _znames;
        }

        public override ZType MemberZType
        {
            get { return ZTypeManager.GetBySharpType(SharpField.FieldType) as ZType; }
        }

        public override string SharpMemberName
        {
            get { return this.MarkField.Name; }
        }

        public override string ToString()
        {
            var names = this.GetZNames();
            return this.MarkField.Name + "(" + string.Join(",", names) + ")";
        }
    }
}
