using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZInstance(typeof(IntPtr))]
    public abstract class 平台标识体
    {
        [ZCode("转化为整数")]
        public abstract string ToInt32();
        
    }
}
