using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;
using Z语言系统;

namespace Z标准包.绘图
{
    [ZStatic]
    public static class 字体辅助
    {
        [ZCode("获取系统已安装字体")]
        public static 列表<FontFamily> GetInstalledFamily()
        {
            FontFamily[] ffs = FontUtil.GetInstalledFamily();
            列表<FontFamily> result = new 列表<FontFamily>(ffs);
            return result;
        }
    }
}
