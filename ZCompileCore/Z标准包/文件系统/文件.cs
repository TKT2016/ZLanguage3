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
        internal FileInfo FeInfo;
        public 文件(string path)
        {
            FeInfo = new FileInfo(path);
        }

        [ZCode("名称")]
        public string Name { get { return FeInfo.Name; } }

        [ZCode("全路径")]
        public string FullName { get { return FeInfo.FullName; } }

        [ZCode("所在文件夹")]
        public 文件夹 所在文件夹 { get { return new 文件夹(FeInfo.Directory); } }

        //[ZCode("存在")]
        //public bool Exists { get; set; }
    }
}
