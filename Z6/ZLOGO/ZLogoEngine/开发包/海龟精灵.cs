using System.Drawing;
using ZGameEngine;
using ZLangRT.Attributes;
using ZLogoEngine;
using ZLogoEngine.Turtles;

namespace ZLogoEngine.开发包
{
    [ZInstance(typeof(TurtleSprite))]
    public abstract class 海龟精灵
    {
        [ZCode("开始绘画")]
        [ZCode("开始画图")]
        [ZCode("开始绘图")]
        public abstract void RunZLogo();

        [ZCode("隐藏")]
        public abstract void Hide();

        [ZCode("显示")]
        public abstract void Show();

        [ZCode("前进(float:distance)")]
        public abstract void Forward(float distance = 10);

        [ZCode("后退(float:distance)")]
        public abstract void Backward(float distance = 10);

        [ZCode("左转(float:angleDelta)度")]
        public abstract void RotateLeft(float angleDelta);

        [ZCode("右转(float:angleDelta)度")]
        public abstract void RotateRight(float angleDelta);

        [ZCode("提笔")]
        [ZCode("提起画笔")]
        [ZCode("抬笔")]
        [ZCode("抬起画笔")]
        public abstract void PenUp();

        [ZCode("落笔")]
        [ZCode("落下画笔")]
        public abstract void PenDown();

        [ZCode("复位")]
        public abstract void Home();

        [ZCode("设置画笔颜色为(Color:color)")]
        public abstract void SetPenColor(Color color);

        [ZCode("设置背景颜色为(Color:color)")]
        public abstract void SetBgColor(Color color);
    }
}
