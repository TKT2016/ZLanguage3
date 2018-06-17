using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace ZExcel开发包
{
    [ZInstance]
    public class Excel边框
    {
        private Border Border;

        public Excel边框(Border border)
        {
            Border = border;
        }

        [ZCode("底框")]
        public ExcelBorderItem Bottom { get { return Border.Bottom; } }

        //[ZCode("对角")]
        //public ExcelBorderItem Diagonal { get { return Border.Diagonal; } }
        //
        // 摘要: 
        //     A diagonal from the top left to bottom right of the cell
        //public bool DiagonalDown { get; set; }
        //
        // 摘要: 
        //     A diagonal from the bottom left to top right of the cell
        //public bool DiagonalUp { get; set; }

        [ZCode("左框")]
        public ExcelBorderItem Left { get { return Border.Left; } }

        [ZCode("右框")]
        public ExcelBorderItem Right { get { return Border.Right; } }

        [ZCode("顶框")]
        public ExcelBorderItem Top { get { return Border.Right; } }

        // 摘要: 
        //     Set the border style around the range.
        //
        // 参数: 
        //   Style:
        //     The border style
        //public void BorderAround(ExcelBorderStyle Style);

        //
        // 摘要: 
        //     Set the border style around the range.
        //
        // 参数: 
        //   Style:
        //     The border style
        //
        //   Color:
        //     The color of the border
        //public void BorderAround(ExcelBorderStyle Style, Color Color);
    }
}
