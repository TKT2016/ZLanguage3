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
    [ZInstance(typeof(FontFamily))]
    public abstract class 字体
    {
        [ZCode("字体名称")]
        [ZCode("名称")]
        public abstract string Name { get; }
        //public FontFamily FontFamily { get; private set; }

        //public 字体(string fname)
        //{
        //    FontFamily = new FontFamily(fname);
        //}
    }

    //[ZInstance]
    //public class 字体 
    //{
    //    public FontFamily FontFamily { get; private set; }

    //    public 字体(string fname)
    //    {
    //        FontFamily = new FontFamily(fname);
    //    }
    //}
}
