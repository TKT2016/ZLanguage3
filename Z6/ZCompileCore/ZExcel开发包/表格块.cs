using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using ZLangRT.Attributes;

namespace ZExcel开发包
{
    [ZInstance]
    public class 表格块
    {
        private ExcelRange WorkRange;

        public 表格块(ExcelRange workRange)
        {
            WorkRange = workRange;
        }

        public 表格块 this[string Address]
        {
            get { 
                ExcelRange range = WorkRange[Address];
                表格块 单元格 = new 表格块(range);
                return 单元格;
            }
        }

        public 表格块 this[int Row, int Col]
        {
            get
            {
                ExcelRange range = WorkRange[Row,Col];
                表格块 单元格 = new 表格块(range);
                return 单元格;
            }
        }

        public 表格块 this[int FromRow, int FromCol, int ToRow, int ToCol]
        {
            get
            {
                ExcelRange range = WorkRange[ FromRow,  FromCol,  ToRow, ToCol];
                表格块 单元格 = new 表格块(range);
                return 单元格;
            }
        }

        [ZCode("内容")]
        public object Value
        {
            get { return WorkRange.Value; }
            set { WorkRange.Value = value; }
        }

        private Excel样式 _样式;

        public Excel样式 样式
        {
            get
            {
                if (_样式 == null)
                {
                    _样式 = new Excel样式(WorkRange.Style);
                }
                return _样式;
            }
        }
    }
}
