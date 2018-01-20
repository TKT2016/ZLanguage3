using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.设备
{
    [ZInstance]
    public class 硬盘
    {
        [ZCode("编号")]
        public string 编号 { get; set; }
    }
}
