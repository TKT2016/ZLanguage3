using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZLangRT.Descs
{
    public class ExMethodInfo : ExMember
    {
        public MethodInfo Method { get; private set; }

        public ExMethodInfo(MethodInfo method, bool isSelf)
        {
            Method = method;
            IsSelf = isSelf;
        }

        //public override List<string> ZyyNames
        //{
        //    get
        //    {
        //        if (_ZyyNames == null)
        //        {
        //            _ZyyNames = GetZyyNames(this.Method);
        //        }
        //        return _ZyyNames;
        //    }
        //}
    }
}
