using System.Drawing;
using ZLangRT.Attributes;
using ZLogoEngine;

namespace ZLOGO开发包
{
    [ZInstance(typeof(TurtleSprite))]
    public abstract class 海龟精灵
    {
        [ZCode("X坐标")]
        public float X { get; set; }

        [ZCode("Y坐标")]
        public float Y { get; set; }

        [ZCode("角度")]
        public float Angle { get; set; }

        [ZCode("画笔")]
        public TurtlePen Pen { get; set; }

        //[ZCode("画笔颜色")]
        //public Color PenColor{ get; set; }

        //[ZCode("画笔宽度")]
        //public float PenSize{ get; set; }

        //[ZCode("设置延迟(int:time)")]
        //public abstract void SetDelay(int time);

        [ZCode("前进(float:distance)")]
        public abstract void Forward(float distance = 10);

        [ZCode("后退(float:distance)")]
        public abstract void Backward(float distance = 10);
        /*
        [ZCode("跳到(FloatPoint:fpoint)")]
        public abstract void MoveTo(FloatPoint fpoint);
        */
        [ZCode("左转(float:angleDelta)度")]
        public abstract void RotateLeft(float angleDelta);

        [ZCode("右转(float:angleDelta)度")]
        public abstract void RotateRight(float angleDelta);

        [ZCode("旋转到(float:newAngle)度")]
        public abstract void RotateTo(float newAngle);

        [ZCode("抬起画笔")]
        public abstract void PenUp();

        [ZCode("落下画笔")]
        public abstract void PenDown();
       
    }
}
