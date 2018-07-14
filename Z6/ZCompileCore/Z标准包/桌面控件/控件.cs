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
    [ZInstance(typeof(Control))]
    public abstract class 控件
    {
        //[ZCode("添加(Control:control)")]
        //public abstract void Add(Control control)
        //{
        //    control.Controls.Add();
        //}

        //[ZCode("内容")]
        //public string Text { get; set; }

        //[ZCode("尺寸")]
        //public Size Size { get; set; }

        [ZCode("位置")]
        public Point Location { get; set; }

        [ZCode("长度")]
        public int Width { get; set; }

        [ZCode("高度")]
        public int Height { get; set; }
    }
}
