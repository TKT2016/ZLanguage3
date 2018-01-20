using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;
using Z标准包.文件系统;

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 快捷方式操作器
    {
        [ZCode("创建(文件:WJ)快捷方式到(文件夹:WJJ)")]
        public static void ShortcutOn(文件 WJ ,文件夹 WJJ )
        {
            string fileName = Path.GetFileNameWithoutExtension(WJ.FullName);
            ShortcutCreator.CreateShortcut(WJJ.FullName, fileName,WJ.FullName,null,null);
        }

        [ZCode("创建(文件:WJ)快捷方式到桌面")]
        public static void ShortcutOnDesktop(文件 WJ)
        {
            string fileName = Path.GetFileNameWithoutExtension(WJ.FullName);
            ShortcutCreator.CreateShortcutOnDesktop(fileName, WJ.FullName, null, null);
        }
    }
}
