using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc
{
    public class ZLibRTException : Exception
    {
        public ZLibRTException()
        {
        }

        public ZLibRTException(string msg)
            : base(msg)
        {
        }

        public ZLibRTException(string format, params object[] args)
            : base(String.Format(format, args))
        {
        }
    }
}
