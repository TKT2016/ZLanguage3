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
    public class Excel工作表
    {
        public string Name { get; set; }
        private ExcelWorksheet Worksheet;

        internal Excel工作表(ExcelWorksheet worksheet)
        {
            Worksheet = worksheet;
        }

        public Excel工作表(string Name)
        {
            this.Name = Name;
        }

        internal void SetWorksheet(ExcelWorksheet worksheet)
        {
            if (worksheet.Name != Name)
            {
                throw new Exception("名称不相同");
            }
            Worksheet = worksheet;
            
        }

        private 表格块 _Cells;

        [ZCode("表格块")]
        public 表格块 Cells
        {
            get
            {
                if (_Cells == null)
                {
                    ExcelRange range = Worksheet.Cells;
                    _Cells = new 表格块(range);
                }
                return _Cells;
            }
        }

    }
}
