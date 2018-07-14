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
    [ZEnum(typeof(MouseButtons))]
    public enum 鼠标按键
    {
        //public int X { get; set; }

        //[MappingCode("Y坐标")]
        //public int Y { get; set; }

        //[MappingCode("鼠标位置")]
        //public Point Location { get; set; }

        //[MappingCode("X坐标")]
        //None = 0,

        [ZCode("鼠标左键")]
        Left = 1048576,

        [ZCode("鼠标右键")]
        Right = 2097152,

        [ZCode("鼠标中键")]
        Middle = 4194304//,

        //     第 1 个 XButton 曾按下。
        //XButton1 = 8388608,

        //     第 2 个 XButton 曾按下。
        //XButton2 = 16777216,
    }
}
