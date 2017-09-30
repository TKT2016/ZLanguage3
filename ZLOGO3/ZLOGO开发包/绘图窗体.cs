
using System.Drawing;
using ZLangRT.Attributes;
using ZLogoEngine;

namespace ZLOGO开发包
{
    [ZInstance(typeof(TurtleForm))]
    public abstract class 绘图窗体
    {
        [ZCode("小乌龟")]
        [ZCode("小海龟")]
        public TurtleSprite Turtle { get; set; }

        [ZCode("窗口")]
        public TurtleForm Window { get; set; }

        [ZCode("开始绘画")]
        [ZCode("开始画图")]
        [ZCode("开始绘图")]
        public abstract void RunZLogo();
    
        [ZCode("设置标题为(string:title)")]
        public abstract void SetTitle(string title);

        [ZCode("背景色")]
        public Color BackgroundColor { get; set; }
    }
}
