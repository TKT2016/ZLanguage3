using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.网络
{
    [ZInstance(typeof(Uri))]
    public abstract class 网址
    {
        [ZCode("绝对路径")]
        public abstract string AbsolutePath { get; }

        [ZCode("主机名")]
        public abstract string Host { get; }

        [ZCode("端口号")]
        public abstract int Port { get; }

        [ZCode("文本内容")]
        public abstract string OriginalString { get; set; }
    }
}
