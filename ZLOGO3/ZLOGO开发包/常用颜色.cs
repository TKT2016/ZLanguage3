using System.Drawing;
using ZLangRT.Attributes;

namespace ZLOGO开发包
{
    [ZStatic(typeof(Color))]
    public class 常用颜色
    {
        [ZCode("红色")]
        public Color Red { get; set; }

        [ZCode("橙色")]
        public Color Orange { get; set; }

        [ZCode("黄色")]
        public Color Yellow { get; set; }

        [ZCode("绿色")]
        public Color Green { get; set; }

        [ZCode("蓝色")]
        public Color Blue { get; set; }

        [ZCode("紫色")]
        public Color Violet { get; set; }

        [ZCode("黑色")]
        public Color Black { get; set; }

        [ZCode("白色")]
        public Color White { get; set; }


        [ZCode("灰色")]
        public Color Gray { get; set; } 

        [ZCode("褐色")]
        public Color Brown { get; set; }

    }
}
