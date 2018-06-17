using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZLangRT.Tags;

namespace ZLangRT.Descs
{
    public abstract class ExMember
    {
        public bool IsSelf { get;  set; }

        //protected List<string> _ZyyNames;

        //public abstract List<string> ZyyNames { get; }

        //protected List<string> GetZyyNames(MemberInfo member)
        //{
        //    List<string> names = new List<string>();
        //    Attribute[] attrs = Attribute.GetCustomAttributes(member, typeof(ZCodeAttribute));
        //    if (attrs.Length == 0)
        //    {
        //        names.Add(member.Name);
        //    }
        //    else
        //    {
        //        foreach (Attribute attr in attrs)
        //        {
        //            ZCodeAttribute zCodeAttribute = attr as ZCodeAttribute;
        //            names.Add(zCodeAttribute.Code);
        //        }
        //    }
        //    return names; 
        //}
    }
}
