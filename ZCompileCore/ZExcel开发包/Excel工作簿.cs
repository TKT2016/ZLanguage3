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
    public class Excel工作簿
    {
        private ExcelWorkbook Workbook;

        public Excel工作簿(ExcelWorkbook workbook)
        {
            Workbook = workbook;
        }

        [ZCode("属性集")]
        public OfficeProperties Properties { get { return Workbook.Properties; } }

        private Excel工作表集 _工作表集;

        [ZCode("工作表集")]
        public Excel工作表集 工作表集
        {
            get
            {
                if(_工作表集==null)
                {
                    _工作表集 = new Excel工作表集(Workbook.Worksheets);
                }
                return _工作表集;
            }
        }


        [ZCode("创建工作表(文本:Name)")]
        public Excel工作表 Create(string Name)
        {
            var b = 工作表集.Create(Name);
            return b;
        }
    }
}
