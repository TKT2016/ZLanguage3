using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;

namespace ZCompileDesc.ZTypes
{
    public static class ZTypeUtil
    {
        internal static FieldInfo[] GetEnumItems(Type type)
        {
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields;
        }

    }
}
