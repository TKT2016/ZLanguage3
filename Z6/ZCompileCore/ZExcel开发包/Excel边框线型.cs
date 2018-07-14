using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT.Attributes;

namespace ZExcel开发包
{
    [ZEnum(typeof(ExcelBorderStyle))]
    public enum Excel边框线型
    {
        [ZCode("无")]
        None = 0,

        [ZCode("细点边框")]
        Hair = 1,

        [ZCode("点边框")]
        Dotted = 2,

        [ZCode("点点划边框")]
        DashDot = 3,

        [ZCode("点划边框")]
        Thin = 4,

        [ZCode("长点边框")]
        DashDotDot = 5,

        [ZCode("普通边框")]
        Dashed = 6,

        [ZCode("粗点划边框")]
        MediumDashDotDot = 7,

        [ZCode("粗花边框")]
        MediumDashed = 8,

        [ZCode("粗点划边框")]
        MediumDashDot = 9,

        [ZCode("粗边框")]
        Thick = 10,

        [ZCode("高粗边框")]
        Medium = 11,

        [ZCode("双边框")]
        Double = 12,
    }
}
