﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using ZLangRT;
using ZLangRT.Attributes;
using Z语言系统;

namespace Z标准包.操作系统
{
    [ZStatic]
    public class 进程操作
    {
        [ZCode("(Process:process)接收消息(string:消息)")]
        public static bool 接收消息(Process process, string 消息)
        {
            if (有效(process.MainWindowHandle))
            {
                var ptrEx = User32.FindWindowEx(process.MainWindowHandle, IntPtr.Zero, "Edit", "");
                接收消息(ptrEx, 消息);
                return true;
            }
            return false;
        }

        [ZCode("当前线程编号")]
        public static int 当前线程编号()
        {
            return Thread.CurrentThread.ManagedThreadId;
        }

        [ZCode("(IntPtr:intPtr)接收消息(string:消息)")]
        public static void 接收消息(IntPtr intPtr, string 消息)
        {
            byte[] bytes = (ASCIIEncoding.ASCII.GetBytes(消息));
            for (int i = 0; i < bytes.Length; i++)
            {
                User32.SendMessage(intPtr, User32.WM_CHAR, (IntPtr)(bytes[i]), (IntPtr)0);
            }
        }

        [ZCode("(IntPtr:intPtr)有效")]
        public static bool 有效(IntPtr intPtr)
        {
            return intPtr != null && intPtr.ToInt64() > 0;
        }
    }
}
