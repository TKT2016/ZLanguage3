using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Z语言系统;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 进程窗口操作器
    {
        [ZCode("取得所有进程窗口")]
        public static 列表<进程窗口> GetAllWindowList()
        {
            列表<进程窗口> list = new 列表<进程窗口>();
            //1、获取桌面窗口的句柄
            IntPtr desktopPtr = User32.GetDesktopWindow();
            //2、获得一个子窗口（这通常是一个顶层窗口，当前活动的窗口）
            IntPtr winPtr = User32.GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);

            //3、循环取得桌面下的所有子窗口
            while (winPtr != IntPtr.Zero)
            {
                进程窗口 pw = new 进程窗口(winPtr);
                list.Add(pw);
                //4、继续获取下一个子窗口
                winPtr = User32.GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);
            }

            //var windows = new List<IntPtr>();
            //WndEnumCallBack wndEnumProc = (hWnd, obj) =>
            //{
            //    if (User32.IsWindowVisible(hWnd))   //跳过不可见的窗口
            //    {
            //        windows.Add(hWnd);
            //    }
            //    return true;    //返回true保证枚举完所有窗口
            //};
            //EnumWindows(wndEnumProc, IntPtr.Zero);
            //return windows;

            return list;
        }
    }
}
