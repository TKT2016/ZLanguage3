using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT.Attributes;

namespace ZCompileDesc.Utils
{
    internal static class ZDescriptionHelper
    {
        public static string[] GetZNames(MemberInfo element)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(element);
            List<string> _znames = new List<string>();
            foreach (var attr in attrs)
            {
                if (attr is ZCodeAttribute)
                {
                    ZCodeAttribute zcodeAttr = (attr as ZCodeAttribute);
                    _znames.Add(zcodeAttr.Code);
                }
            }
            return _znames.ToArray();
        }
    }
}
