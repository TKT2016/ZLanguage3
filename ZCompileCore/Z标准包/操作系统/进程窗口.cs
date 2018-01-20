using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZInstance]
    public class 进程窗口
    {
        internal IntPtr MainIntPtr { get; set; }

        internal 进程窗口(IntPtr mainIntPtr )
        {
            MainIntPtr = mainIntPtr;
        }

        [ZCode("标题")]
        public string Title
        {
            get
            {
                StringBuilder title = new StringBuilder();
                title.Length = 100;
                User32.GetWindowText(MainIntPtr , title, 100);//取标题
                return title.ToString();
            }
        }

        [ZCode("可见")]
        public bool Visible
        {
            get
            {
                 bool b = User32.IsWindowVisible(MainIntPtr);
                 return b;
            }
        }

        [ZCode("不可见")]
        public bool NotVisible
        {
            get
            {
                return !Visible;
            }
        }
    }
}
