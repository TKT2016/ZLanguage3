using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.绘图.形状
{
    [ZInstance]
    public class 线段
    {
        public 线段(Point 第一点, Point 第二点)
        {
            this.第一点 = 第一点;
            this.第二点 = 第二点;
        }

        public Point 第一点 { get; set; }
        public Point 第二点 { get; set; }
    }
}
