using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ZLangRT;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZInstance(typeof(Process))]
    public abstract class 进程
    {
        [ZCode("名称")]
        public string ProcessName { get; set; }

        [ZCode("主窗口标识体")]
        public string MainWindowHandle { get; set; }

        [ZCode("关闭")]
        public abstract void Close();

        [ZCode("强制终止")]
        public abstract void Kill();

    }
}
