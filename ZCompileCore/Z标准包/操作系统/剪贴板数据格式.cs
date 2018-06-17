using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z标准包.操作系统
{
    [ZEnum]
    public enum 剪贴板数据格式
    {
        [ZCode("BMP图片格式")]
        BMP图片格式,

        [ZCode("网页文本格式")]
        网页文本格式,

        [ZCode("富文本格式")]
        富文本格式,

        [ZCode("纯文本格式")]
        纯文本格式

    }
}
