using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.设备
{
    [ZStatic]
    public static class 喇叭控制器
    {
        [ZCode("播放提示音")]
        public static void 播放提示音()
        {
            Console.Beep();
        }

        [ZCode("播放提示音(int:频率,int:时长)")]
        public static void 播放提示音(int 频率, int 时长)
        {
            int frequency = 频率;
            int duration = 时长;
            Console.Beep(frequency, duration);
        }
    }
}
