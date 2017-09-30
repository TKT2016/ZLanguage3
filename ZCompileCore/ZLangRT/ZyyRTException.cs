using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLangRT
{
    public class ZyyRTException : Exception
    {
        public ZyyRTException()
        {
        }

        public ZyyRTException(string msg)
            : base(msg)
        {
        }

        public ZyyRTException(string format, params object[] args)
            : base(String.Format(format, args))
        {
        }
    }
}
