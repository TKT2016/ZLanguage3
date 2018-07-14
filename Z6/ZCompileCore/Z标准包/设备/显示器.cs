using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT.Attributes;


namespace Z标准包.设备
{
    [ZStatic]
    public static class 显示器
    {
        [ZCode("高度")]
        public static int 高度
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Height;
            }
        }

        [ZCode("长度")]
        public static int 长度
        {
            get
            {
                return Screen.PrimaryScreen.Bounds.Width;
            }
        }
    }
}
