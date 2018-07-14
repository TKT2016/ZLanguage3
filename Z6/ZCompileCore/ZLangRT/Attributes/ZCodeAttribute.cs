﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLangRT.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public class ZCodeAttribute : ZyyAttribute
    {
        public ZCodeAttribute(string procCode)
        {
            Code = procCode;
        }

        public string Code { get; private set; }

        public override string ToString()
        {
            return string.Format("ZCodeAttribute({0})", Code);
        }
    }
}
