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
    [ZInstance(typeof(Point))]
    public abstract class 点
    {
        [ZCode("X坐标")]
        [ZCode("X")]
        public int X { get; set; }

        [ZCode("Y坐标")]
        [ZCode("Y")]
        public int Y { get; set; }
    }
}
