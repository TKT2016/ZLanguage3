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
    public class 椭圆
    {
        public 椭圆(Point 圆心, int X轴半径, int Y轴半径)
        {
            this.圆心 = 圆心;
            this.X轴半径 = X轴半径;
            this.Y轴半径 = Y轴半径;
        }


        public Point 圆心 { get; set; }
        public int X轴半径 { get; set; }
        public int Y轴半径 { get; set; }
    }
}
