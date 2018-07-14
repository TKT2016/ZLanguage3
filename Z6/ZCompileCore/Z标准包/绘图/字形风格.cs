using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.绘图
{
    [ZEnum(typeof(FontStyle))]
    public enum 字形风格
    {
        [ZCode("普通文本")]
        Regular = 0,

        [ZCode("粗体文本")]
        Bold = 1,

        [ZCode("倾斜文本")]
        Italic = 2,

        [ZCode("下划线文本")]
        Underline = 4,

        [ZCode("删除线文本")]
        Strikeout = 8
    }

    //public enum 字形风格
    //{
    //    普通文本,
    //    粗体文本,
    //    倾斜文本,
    //    下划线文本,
    //    删除线文本
    //}
}
