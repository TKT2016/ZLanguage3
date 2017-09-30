using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z语言系统
{
    [ZStatic]
    public static class 常用辅助
    {
        [ZCode("非(判断符:b)")]
        public static bool 非(bool b)
        {
            return !b;
        }

        [ZCode("暂停(整数:t)毫秒")]
        public static void 暂停(int t)
        {
            Thread.Sleep(t);
        }
    }
}
