using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZInstance]
    public class 程序牌照
    {
        IntPtr Ptr;
        internal 程序牌照(IntPtr intPtr)
        {
            Ptr = intPtr;
        }

        [ZCode("接收消息(string:消息)")]
        public void 接收消息( string 消息)
        {
            IntPtrHelper.ReceiveMsg(Ptr, 消息);
            //byte[] bytes = (ASCIIEncoding.ASCII.GetBytes(消息));
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    User32.SendMessage(Ptr, User32.WM_CHAR, (IntPtr)(bytes[i]), (IntPtr)0);
            //}
        }

        [ZCode("有效")]
        public bool 有效( )
        {
            return IntPtrHelper.Effective(Ptr);// Ptr != null && Ptr.ToInt64() > 0;
        }
    }
    /*
    [ZInstance(typeof(IntPtr))]
    public abstract class 程序牌照
    {
        [ZCode("转化为整数")]
        public abstract string ToInt32();
        
    }*/
}
