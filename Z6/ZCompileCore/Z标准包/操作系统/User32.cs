using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Z标准包.操作系统
{
    public static class User32
    {
        #region Window 窗口

        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.
        /// </summary>
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 该函数返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。
        /// 函数原型：HWND GetWindow（HWND hWnd，UNIT nCmd）；
        /// </summary>
        /// <param name="hWnd">窗口句柄。要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄。</param>
        /// <param name="uCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值参考GetWindowCmd枚举。</param>
        /// <returns>返回值：如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。
        /// 若想获得更多错误信息，请调用GetLastError函数。
        /// 备注：在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。
        /// 速查：Windows NT：3.1以上版本；Windows：95以上版本；Windows CE：1.0以上版本；头文件：winuser.h；库文件：user32.lib。
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll ")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr parenthW, IntPtr child, string s1, string s2);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndlnsertAfter, int X, int Y, int cx, int cy, uint Flags);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);


        /// <summary>
        /// 用于枚举子窗体是的委托
        /// </summary>
        /// <param name="WindowHandle">窗体句柄</param>
        /// <param name="num">自定义</param>
        /// <returns></returns>
        public delegate bool EnumChildWindow(IntPtr WindowHandle, string num);
        /// <summary>
        /// 获取指定窗体的所有子窗体
        /// </summary>
        /// <param name="WinHandle">窗体句柄</param>
        /// <param name="ec">回调委托</param>
        /// <param name="name">自定义</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int EnumChildWindows(IntPtr WinHandle, EnumChildWindow ecw, string name);
        /// <summary>
        /// 获取指定窗体的标题
        /// </summary>
        /// <param name="WinHandle">窗体句柄</param>
        /// <param name="Title">缓冲区取用于存储标题</param>
        /// <param name="size">缓冲区大小</param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr WinHandle, StringBuilder Title, int size);

        /// <summary>
        /// 获取窗体类型
        /// </summary>
        /// <param name="WinHandle">窗体句柄</param>
        /// <param name="Type">类型</param>
        /// <param name="size">缓冲区大小</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr WinHandle, StringBuilder Type, int size);

        /// <summary>
        /// 根据句柄获得进程id值
        /// </summary>
        /// <param name="handle">句柄</param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int pid);

        #endregion

        #region Message 消息

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr wnd, int msg, IntPtr wP, IntPtr lP);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(int threadId, uint msg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region Cursor 鼠标

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        #endregion


        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern int SystemParametersInfo(
         int uAction,
         int uParam,
         string lpvParam,
         int fuWinIni
         ); 

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);



        [DllImport("user32.dll ")]
        static extern bool IsIconic(IntPtr hWnd);


        //ShowWindow参数
        private const int SW_SHOWNORMAL = 1;
        private const int SW_RESTORE = 9;
        private const int SW_SHOWNOACTIVATE = 4;
        //SendMessage参数
        public const int WM_KEYDOWN = 0X100;
        public const int WM_KEYUP = 0X101;
        public const int WM_SYSCHAR = 0X106;
        public const int WM_SYSKEYUP = 0X105;
        public const int WM_SYSKEYDOWN = 0X104;
        public const int WM_CHAR = 0X102;
    }
}
