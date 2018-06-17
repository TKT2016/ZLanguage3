using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.文件系统
{
    [ZInstance(typeof(FileSystemInfo))]
    public class 文件系统节点
    {
        [ZCode("名称")]
        public string Name { get; set; }

        [ZCode("全路径")]
        public string FullName { get; set; }

        [ZCode("存在")]
        public bool Exists { get; set; }
    }
}
