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
    [ZInstance(typeof(ZForm))]
    public abstract class 窗体
    {
        [ZCode("标题")]
        public string Text { get; set; }

        [ZCode("尺寸")]
        public Size Size { get; set; }

        [ZCode("位置")]
        public Point Location { get; set; }

        [ZCode("光标")]
        public Cursor Cursor { get; set; }

        [ZCode("背景色")]
        public Color BackColor { get; set; }

        [ZCode("长度")]
        public int Width { get; set; }

        [ZCode("高度")]
        public int Height { get; set; }

        [ZCode("添加(Control:control)")]
        public abstract void Add(Control control);
    }
}
