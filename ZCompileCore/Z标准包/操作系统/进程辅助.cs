using System;
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
    public class 进程辅助
    {
        [ZCode("启动程序(string:路径)")]
        public static Process 启动程序(string 路径)
        {
            ProcessStartInfo info = new ProcessStartInfo(路径);
            Process pro = Process.Start(info);
            return pro;
        }

        [ZCode("用参数(string:参数)启动程序(string:路径)")]
        public static Process 启动程序(string 路径,string 参数)
        {
            ProcessStartInfo info = new ProcessStartInfo(路径, 参数);
            Process pro = Process.Start(info);
            return pro;
        }

        [ZCode("根据名称(string:进程名称)查找线程")]
        public static 列表<Process> 查找进程(string 进程名称)
        {
            return new 列表<Process> (Process.GetProcessesByName(进程名称));
        }

        /*
        [MappingCode("关闭进程(string:进程名称)")]
        public static void 关闭进程(string 进程名称)
        {
            Process[] allProgresse = System.Diagnostics.Process.GetProcessesByName(进程名称);
            foreach (Process closeProgress in allProgresse)
            {
                if (closeProgress.ProcessName.Equals(进程名称))
                {
                    closeProgress.Kill();
                    closeProgress.WaitForExit();
                    break;
                }
            }
        }*/
        /*
        public static void 发送消息(string 窗口名称,string 消息)
        {
            IntPtr myIntPtr = FindWindow(null, 窗口名称);
            if (myIntPtr != null && myIntPtr.ToInt32()>0)
            {
                byte[] bytes = (ASCIIEncoding.ASCII.GetBytes(消息));
                for (int i = 0; i < bytes.Length; i++)
                {
                    SendMessage(myIntPtr, WM_CHAR, bytes[i], 0);
                }
            }
        }*/

        

    }
}
