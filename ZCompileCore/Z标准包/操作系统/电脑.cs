using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 电脑
    {
        [ZCode("重启")]
        public static void Reboot()
        {
            DoExitWin(EWX_REBOOT);
        }

        [ZCode("注销")]
        public static void Logoff()
        {
            DoExitWin(EWX_LOGOFF);
        }

        [ZCode("关机")]
        public static void Shutdown()
        {
            DoExitWin(EWX_SHUTDOWN);
        }

        [ZCode("名称")]
        public static string MachineName { get { return Environment.MachineName; } }

        [ZCode("处理器数")]
        public static int ProcessorCount { get { return Environment.ProcessorCount; } }

        [ZCode("系统目录")]
        public static string SystemDirectory { get { return Environment.SystemDirectory; } }

        [ZCode("启动时长")]
        public static int TickCount { get { return Environment.TickCount; } }

        [ZCode("登录用户名")]
        public static string UserName { get { return Environment.UserName; } }
        

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct TokPriv1Luid
        {
            public int Count;
            public long Luid;
            public int Attr;
        }

        [DllImport("kernel32.dll", ExactSpelling = true)]
        internal static extern IntPtr GetCurrentProcess();

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool OpenProcessToken(IntPtr h, int acc, ref IntPtr phtok);

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool LookupPrivilegeValue(string host, string name, ref long pluid);

        [DllImport("advapi32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool AdjustTokenPrivileges(IntPtr htok, bool disall,ref TokPriv1Luid newst, int len, IntPtr prev, IntPtr relen);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        internal static extern bool ExitWindowsEx(int flg, int rea);

        internal const int SE_PRIVILEGE_ENABLED = 0x00000002;
        internal const int TOKEN_QUERY = 0x00000008;
        internal const int TOKEN_ADJUST_PRIVILEGES = 0x00000020;
        internal const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        internal const int EWX_LOGOFF = 0x00000000;
        internal const int EWX_SHUTDOWN = 0x00000001;
        internal const int EWX_REBOOT = 0x00000002;
        internal const int EWX_FORCE = 0x00000004;
        internal const int EWX_POWEROFF = 0x00000008;
        internal const int EWX_FORCEIFHUNG = 0x00000010;

        private static void DoExitWin(int flg)
        {
            bool ok;
            TokPriv1Luid tp;
            IntPtr hproc = GetCurrentProcess();
            IntPtr htok = IntPtr.Zero;
            ok = OpenProcessToken(hproc, TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, ref htok);
            tp.Count = 1;
            tp.Luid = 0;
            tp.Attr = SE_PRIVILEGE_ENABLED;
            ok = LookupPrivilegeValue(null, SE_SHUTDOWN_NAME, ref tp.Luid);
            ok = AdjustTokenPrivileges(htok, false, ref tp, 0, IntPtr.Zero, IntPtr.Zero);
            ok = ExitWindowsEx(flg, 0);
            
        }

        

        /*
        //SHUT DOWN
        protected void btnShutDown_Click(object sender, EventArgs e)
        {    
         Process.Start("shutdown.exe", "-s");
         // By Default the Shutdown will take place after 30 Seconds
         //if you want to change the Delay try this one
         Process.Start("shutdown.exe","-s -t xx");
         //Replace xx with Seconds example 10,20 etc
        }

        //RESTART
        protected void btnRestart_Click(object sender, EventArgs e)
        {
         Process.Start("shutdown.exe", "-r");
         // By Default the Restart will take place after 30 Seconds
         //if you want to change the Delay try this one
         Process.Start("shutdown.exe","-r -t 10");
         //Replace xx with Seconds example 10,20 etc
        }

        // LOG OFF
        protected void btnLogOff_Click(object sender, EventArgs e)
        {
         Process.Start("shutdown.exe", "-l");
         //This Code Will Directly Log Off the System Without warnings
        }
        */
    }
}
