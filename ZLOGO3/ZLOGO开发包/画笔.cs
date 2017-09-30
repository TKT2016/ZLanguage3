using System.Drawing;
using ZLangRT.Attributes;
using ZLogoEngine;

namespace ZLOGO开发包
{
    [ZInstance(typeof(TurtlePen))]
    public abstract class 画笔
    {
        [ZCode("宽度")]
        public float Size { get; set; }

        [ZCode("颜色")]
        public Color Color { get; set; }

        [ZCode("可见")]
        public bool Visible { get; set; }
    }
}
