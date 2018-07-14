using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Z标准包.操作系统
{
    public static class IntPtrHelper
    {
        public static bool Effective(IntPtr Ptr)
        {
            return Ptr != null && Ptr.ToInt64() > 0;
        }

        public static void ReceiveMsg(IntPtr intPtr, string msg)
        {
            byte[] bytes = (ASCIIEncoding.ASCII.GetBytes(msg));
            for (int i = 0; i < bytes.Length; i++)
            {
                User32.SendMessage(intPtr, User32.WM_CHAR, (IntPtr)(bytes[i]), (IntPtr)0);
            }
        }
    }
}
