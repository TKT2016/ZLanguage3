using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.文件系统
{
    [ZInstance]
    public class 图片
    {
        internal Image ImageInfo;
        //string path;
        internal FileInfo FeInfo;

        public 图片(string path)
        {
            FeInfo = new FileInfo(path);
            ImageInfo = Image.FromFile(path);
        }

        [ZCode("长度")]
        public int Width { get { return ImageInfo.Width; } }

        [ZCode("高度")]
        public int Height { get { return ImageInfo.Height; } }

        [ZCode("全路径")]
        public string FullName { get { return FeInfo==null?"": FeInfo.FullName; } }
    }
}
