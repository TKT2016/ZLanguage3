using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.桌面控件
{
    [ZInstance(typeof(MouseEventArgs))]
    public class 鼠标参数
    {
        [ZCode("按键")]
        public MouseButtons Button { get; set; }

        [ZCode("X坐标")]
        public int X { get; set; }

        [ZCode("Y坐标")]
        public int Y { get; set; }

        [ZCode("鼠标位置")]
        public Point Location { get; set; }
    }
}
