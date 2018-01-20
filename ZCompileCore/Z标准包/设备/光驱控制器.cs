using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.设备
{
    [ZStatic]
    public static class 光驱控制器
    {
        [DllImport("winmm.dll", EntryPoint="mciSendStringA", CharSet=CharSet.Ansi)]
        private static extern int mciSendString(string mciCommand,StringBuilder returnValue,int returnLength,IntPtr callback);

        [ZCode("打开光驱")]
        public static void 打开光驱()
        {
            int result = mciSendString("set cdaudio door open", null, 0, IntPtr.Zero);
        }

        [ZCode("关闭光驱")]
        public static void 关闭光驱()
        {
            int result = mciSendString("set cdaudio door closed",null, 0, IntPtr.Zero);
        }
    }
}
