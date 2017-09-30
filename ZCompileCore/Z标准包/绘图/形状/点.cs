using System;
using System.Drawing;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.绘图.形状
{
    [ZInstance(typeof(Point))]
    public class 点
    {
        [ZCode("X坐标")]
        public int X { get; set; }

        [ZCode("Y坐标")]
        public int Y { get; set; }
    }
}
