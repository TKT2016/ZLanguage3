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
    [ZInstance(typeof(Font))]
    public abstract class 字形
    {
        [ZCode("已加粗")]
        public abstract bool Bold { get; }

        [ZCode("字体")]
        public abstract FontFamily FontFamily { get; }

        [ZCode("高度")]
        public abstract int Height { get; }

        [ZCode("名称")]
        public abstract string Name { get; }

        [ZCode("尺寸")]
        public abstract float Size { get; }

        [ZCode("字形风格")]
        public abstract FontStyle Style { get; }

        [ZCode("已加下划线")]
        public abstract bool Underline { get; }

        [ZCode("已加删除线")]
        public abstract bool Strikeout { get; }

        [ZCode("已倾斜")]
        public abstract bool Italic { get; }
    }

    //[ZInstance]
    //public class 字形 
    //{

    //    public Font Font { get; set; }
    //    ////private 字体 _字体;

    //    public 字形(string fname, float fsize, FontStyle fstyle)
    //    {
    //        Font = new Font(fname, fsize, fstyle);
    //        //_字体 = new 字体(fname);
    //    }

    //    //[ZCode("字体")]
    //    //public 字体 字体
    //    //{
    //    //    get
    //    //    {
    //    //        return new 字体(fname);
    //    //    }
    //    //    set
    //    //    {
    //    //        Font.FontFamily = value.FontFamily;
    //    //    }
    //    //}

    //}
}
