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
    public class 进程操作器
    {
        [ZCode("启动(进程:JC)")]
        public static void 启动(进程 JC)
        {
            ProcessStartInfo info = new ProcessStartInfo(JC.Path,JC.Args);
            Process proc = Process.Start(info);
            JC.Process = proc;
        }

        [ZCode("获取所有进程")]
        public static 列表<进程> 获取所有进程( )
        {
            Process[] arr = Process. GetProcesses();
            列表<进程> list = new 列表<进程>();
            foreach(var item in arr)
            {
                进程 A = new 进程(item);
                list.Add(A);
            }
            return list;
        }

        [ZCode("根据名称(string:进程名称)查找线程")]
        public static 列表<Process> 查找进程(string 进程名称)
        {
            return new 列表<Process>(Process.GetProcessesByName(进程名称));
        }

        //[ZCode("启动程序(string:路径)")]
        //public static Process 启动程序(string 路径)
        //{
        //    ProcessStartInfo info = new ProcessStartInfo(路径);
        //    Process pro = Process.Start(info);
        //    return pro;
        //}

        //[ZCode("用参数(string:参数)启动程序(string:路径)")]
        //public static Process 启动程序(string 路径,string 参数)
        //{
        //    ProcessStartInfo info = new ProcessStartInfo(路径, 参数);
        //    Process pro = Process.Start(info);
        //    return pro;
        //}

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

    }
}
