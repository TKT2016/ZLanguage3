using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;
using System.Runtime.InteropServices;  //调用WINDOWS API函数时要用到
using Microsoft.Win32;
using Z标准包.文件系统;
using System.Drawing;
using System.IO;  //写入注册表时要用到

namespace Z标准包.操作系统
{
    [ZStatic]
    public static class 操作系统配置器
    {
        [ZCode("设置桌面背景方式为(桌面背景显示方式:F)")]
        public static void SetDescktopBackgroundFormat(桌面背景显示方式 F)
        {
            //设置墙纸显示方式
            RegistryKey myRegKey = Registry.CurrentUser.OpenSubKey("Control Panel/desktop", true);
            //赋值
            //注意：在把数值型的数据赋到注册表里面的时候，
            //如果不加引号，则该键值会成为“REG_DWORD”型；
            //如果加上引号，则该键值会成为“REG_SZ”型。
            if (F == 桌面背景显示方式.居中)
            {
                myRegKey.SetValue("TileWallpaper", "0");
                myRegKey.SetValue("WallpaperStyle", "0");
            }
            else if (F == 桌面背景显示方式.平铺)
            {
                myRegKey.SetValue("TileWallpaper", "1");
                myRegKey.SetValue("WallpaperStyle", "0");
            }
            else if (F == 桌面背景显示方式.拉伸)
            {
                myRegKey.SetValue("TileWallpaper", "0");
                myRegKey.SetValue("WallpaperStyle", "2");
            }

            //关闭该项,并将改动保存到磁盘
            myRegKey.Close(); 
        }

        [ZCode("设置桌面背景图片为(图片:T)")]
        public static void SetDescktopBackgroundImage(图片 T)
        {
            Image image = T.ImageInfo;
            string windows = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            string bmppath = Path.Combine(windows, "WallPaper.bmp");
            image.Save(bmppath, System.Drawing.Imaging.ImageFormat.Bmp);
            User32.SystemParametersInfo(20, 0, bmppath, 0x2);
        }
    }
}
