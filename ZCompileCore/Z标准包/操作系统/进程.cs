using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZInstance]
    public class 进程
    {
        [ZCode("路径")]
        public string Path { get; private set; }

        [ZCode("启动参数")]
        public string Args { get; private set; }

        public Process Process { get; internal set; }

        internal 进程(Process process)
        {
            Process = process;
        }

        public 进程(string 路径)
        {
            Path = 路径;
        }

        [ZCode("设置启动参数为(string:args)")]
        public void SetArgs(string args)
        {
            Args = args;
        }

        //public 进程(string 路径,string 参数)
        //{
        //    Path = 路径;
        //    Args = 参数;
        //}

        [ZCode("启动")]
        public void 启动()
        {
            ProcessStartInfo info = new ProcessStartInfo(this.Path, this.Args);
            Process proc = Process.Start(info);
            this.Process = proc;
        }

        [ZCode("名称")]
        public string ProcessName { get { return Process.ProcessName; } }

        //[ZCode("主窗口标识体")]
        public IntPtr MainWindowHandle { get { return Process.MainWindowHandle; } }

        [ZCode("关闭")]
        public void Close()
        {
            Process.Close();
        }

        [ZCode("强制终止")]
        public void Kill()
        {
            Process.Kill();
        }

        [ZCode("接收消息(string:消息)")]
        public bool 接收消息( string 消息)
        {
            if (IntPtrHelper.Effective(this.Process.MainWindowHandle))
            {
                var ptrEx = User32.FindWindowEx(this.Process.MainWindowHandle, IntPtr.Zero, null, null);
                IntPtrHelper.ReceiveMsg(ptrEx , 消息);
                return true;
            }
            //if (有效(process.MainWindowHandle))
            //{
            //    var ptrEx = User32.FindWindowEx(process.MainWindowHandle, IntPtr.Zero, "Edit", "");
            //    接收消息(ptrEx, 消息);
            //    return true;
            //}
            return false;
        }
    }
}
