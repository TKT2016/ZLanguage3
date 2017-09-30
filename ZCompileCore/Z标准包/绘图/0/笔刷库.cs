using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.绘图
{
    [ZStatic(typeof(Brushes))]
    public static class 笔刷库
    {
        [ZCode("绿色笔刷")]
        public static Brush Green { get; set; }

        [ZCode("红色笔刷")]
        public static Brush Red { get; set; }
    }
}
