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
    public class Excel工作表集
    {
        private ExcelWorksheets Worksheets;

        public Excel工作表集(ExcelWorksheets worksheets)
        {
            Worksheets = worksheets;
        }

        public Excel工作表 this[int PositionID]
        {
            get
            {
                ExcelWorksheet sheet = Worksheets[PositionID - 1];
                Excel工作表 工作表 = new Excel工作表(sheet);
                return 工作表;
            }
        }

        public Excel工作表 this[string Name]
        {
            get
            {
                ExcelWorksheet sheet = Worksheets[Name];
                Excel工作表 工作表 = new Excel工作表(sheet);
                return 工作表;
            }
        }

        [ZCode("数量")]
        public int Count { get { return Worksheets.Count; } }

        //[ZCode("添加(Excel工作表:B)")]
        //public void Add(Excel工作表 B)
        //{
        //    ExcelWorksheet sheet = Worksheets.Add(B.Name);
        //    B.SetWorksheet(sheet);
        //}

        [ZCode("创建工作表(文本:Name)")]
        public Excel工作表 Create(string Name)
        {
            ExcelWorksheet sheet = Worksheets.Add(Name);
            Excel工作表 工作表 = new Excel工作表(sheet);
            return 工作表;
        }

        [ZCode("删除(整数:Index)")]
        public void Delete(int Index)
        {
            Worksheets.Delete(Index - 1);
        }

        [ZCode("删除(文本:Name)")]
        public void Delete(string Name)
        {
            Worksheets.Delete(Name);
        }
    }
}
