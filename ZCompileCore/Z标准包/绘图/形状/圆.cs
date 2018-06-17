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
    public class 圆
    {
        public 圆(Point 圆心, int 半径)
        {
            this.圆心 = 圆心;
            this.半径 = 半径;
        }


        public Point 圆心 { get; set; }
        public int 半径 { get; set; }

    }
}
