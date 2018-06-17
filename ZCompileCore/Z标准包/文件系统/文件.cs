using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.文件系统
{
    [ZInstance]
    public class 文件
    {
        public FileInfo FileInfo { get; private set; }
        public 文件(string path)
        {
            FileInfo = new FileInfo(path);
        }

        [ZCode("名称")]
        public string Name { get { return FileInfo.Name; } }

        [ZCode("全路径")]
        public string FullName { get { return FileInfo.FullName; } }

        [ZCode("所在文件夹")]
        public 文件夹 所在文件夹 { get { return new 文件夹(FileInfo.Directory); } }

        //[ZCode("存在")]
        //public bool Exists { get; set; }
    }
}
