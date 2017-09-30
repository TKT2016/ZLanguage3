using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZLangRT.Descs
{
    public class ExFieldInfo : ExMember
    {
        public FieldInfo Field { get; private set; }
        public string ZName { get; private set; }

        public ExFieldInfo(FieldInfo property, bool isSelf,string zname)
        {
            Field = property;
            IsSelf = isSelf;
            ZName = zname;
        }

        //public override List<string> ZyyNames
        //{
        //    get
        //    {
        //        if(_ZyyNames==null)
        //        {
        //            _ZyyNames = GetZyyNames(this.Field);
        //        }
        //        return _ZyyNames;
        //    }
        //}
    }
}
