using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.文件系统
{
    [ZInstance]//[ZInstance(typeof(DirectoryInfo))]
    public class 文件夹
    {
        internal DirectoryInfo DirInfo;

        public 文件夹(string path)
        {
            DirInfo = new DirectoryInfo(path);
        }

        internal 文件夹(DirectoryInfo dinfo)
        {
            DirInfo = dinfo;
        }

        [ZCode("名称")]
        public string Name { get { return DirInfo.Name; } }

        [ZCode("全路径")]
        public string FullName { get { return DirInfo.FullName; } }

        [ZCode("上一文件夹")]
        public 文件夹 上一文件夹 { get { return new 文件夹(DirInfo.Parent); } }

        //[ZCode("存在")]
        //public bool Exists { get; set; }
    }
}
