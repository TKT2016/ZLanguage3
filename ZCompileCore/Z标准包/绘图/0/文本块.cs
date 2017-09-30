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
    [ZInstance]
    public class 文本块 
    {
        public string 字体类型 { get; set; }
        public float 字体大小 { get; set; }
        public Point 位置 { get; set; }
        public string 内容 { get; set; }

        public 文本块(string 字体类型, float 字体大小, Point 位置, string 内容)
        {
            this.字体类型 = 字体类型;
            this.字体大小 = 字体大小;
            this.位置 = 位置;
            this.内容 = 内容;
        }
    }
}
