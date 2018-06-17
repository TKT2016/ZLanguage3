//using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.桌面控件
{
    [ZInstance(typeof(Size))]
    public class 尺寸 
    {
        [ZCode("长度")]
        public int Width { get; set; }

        [ZCode("高度")]
        public int Height { get; set; } 
    }
}
