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
        public string NewLine
        {
            get
            {
                return "\n";
            }
        }

        [ZCode("制表符")]
        public string Tab
        {
            get
            {
                return "\t";
            }
        }

        [ZCode("单引号")]
        public string SingleQuotationMarks
        {
            get
            {
                return "'";
            }
        }
    }
}
