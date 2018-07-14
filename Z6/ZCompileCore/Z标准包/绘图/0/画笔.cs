using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.绘图
{
    [ZInstance(typeof(Pen))]
    public class 画笔
    {
        [ZCode("颜色")]
        public Color Color { get; set; }   
    }
}
