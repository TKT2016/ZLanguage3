using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace ZExcel开发包
{
    [ZInstance]
    public class Excel样式
    {
        private ExcelStyle Style;

        public Excel样式(ExcelStyle style)
        {
            Style = style;
        }

        [ZCode("边距")]
        public int Indent
        {
            get { return Style.Indent; }
            set { Style.Indent = value; }
        }
    }
}
