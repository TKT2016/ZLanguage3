using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace Z语言系统
{
    [ZStatic]
    public class 常用转义符
    {
        [ZCode("换行符")]
        public const string NewLine="\n";

        [ZCode("制表符")]
        public const string Tab= "\t";
        
        [ZCode("单引号")]
        public const string SingleQuotationMarks= "'";

        [ZCode("双引号")]
        public const string DoubleQuotationMarks = "\"";
        
    }
}
