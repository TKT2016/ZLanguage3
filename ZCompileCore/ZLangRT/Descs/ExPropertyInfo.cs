using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZLangRT.Descs
{
    public class ExPropertyInfo : ExMember
    {
        public PropertyInfo Property { get; private set; }
        public string ZName { get; private set; }

        public ExPropertyInfo(PropertyInfo property, bool isSelf, string zname)
        {
            Property = property;
            IsSelf = isSelf;
            ZName = zname;
        }

        //public override List<string> ZyyNames
        //{
        //    get
        //    {
        //        if(_ZyyNames==null)
        //        {
        //            _ZyyNames = GetZyyNames(this.Property);
        //        }
        //        return _ZyyNames;
        //    }
        //}
    }
}
