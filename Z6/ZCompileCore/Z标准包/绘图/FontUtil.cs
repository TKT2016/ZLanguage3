using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;

namespace Z标准包.绘图
{
    public static class FontUtil
    {
        /// <summary>
        /// 获取当前系统中安装的所有字体
        /// </summary>
        public static FontFamily[] GetInstalledFamily()
        {
            InstalledFontCollection fc = new InstalledFontCollection();
            return fc.Families;
        }
        //public static FontStyle GetFontStyle(字形风格 arg)
        //{
        //    if (arg == 字形风格.普通文本) return FontStyle.Regular;

        //}
    }
}
