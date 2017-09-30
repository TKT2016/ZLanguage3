using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.网络
{
    [ZStatic]
    public static class 网络操作
    {
        [ZCode("从(Uri:WZ)获取网页源码")]
        public static string 获取HTML(Uri WZ)
        {
            string html = HttpHelper.GetHtml(WZ.OriginalString);
            return html;
        }

        [ZCode("从(Uri:WZ)下载文件到(DirectoryInfo:WJJ)")]
        public static void 下载文件(Uri WZ, DirectoryInfo WJJ)
        {
            if (!WJJ.Exists)
            {
                WJJ.Create();
            }
            string newFilePath = Path.Combine(WJJ.FullName, PictureDownUtil.GetFileNameByUrl(WZ.OriginalString));
            PictureDownUtil.SavePhotoFromUrl(WZ.OriginalString, newFilePath);
        }
    }
}
