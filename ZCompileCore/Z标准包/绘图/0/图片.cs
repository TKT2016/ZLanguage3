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
    [ZInstance(typeof(Image))]
    public class 图片
    {
        [ZCode("长度")]
        public int Width { get; set; }

        [ZCode("高度")]
        public int Height { get; set; }
    }
}
