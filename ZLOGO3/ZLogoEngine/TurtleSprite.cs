using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using ZGameEngine;
using ZGameEngine.Graphics;
using ZLangRT.Utils;
using ZOpen2D;

namespace ZLogoEngine
{
    public class TurtleSprite : DrawSprite
    {
        public TurtleSprite(TurtleForm form)
        {
            this.Graphics = new Draw2D(form.Width, form.Height);
            Init();
        }

        public override void Init()
        {
            //Dispose();
            base.Init();
            Angle = 90;
            MoveSpeed = 8;
            RotateSpeed = 6;
            //PenSize = 2;
            //PenColor = Color.Blue;
            Pen = new TurtlePen() { Color = Color.Blue, Size = 2, Visible = true };
            Texture = ContentManager.LoadImage("Content/Turtle.png");
        }

        public void Reset()
        {
            Dispose();
        }

        public void Forward(float distance)
        {
            AnimationTo animationTo = new AnimationTo(this.MoveSpeed, distance, false);
            Animations.Enqueue(animationTo);
        }

        public void Backward(float distance)
        {
            AnimationTo animationTo = new AnimationTo(this.MoveSpeed, distance, true);
            Animations.Enqueue(animationTo);
        }

        public void MoveTo(float newX, float newY)
        {
            this.X = newX;
            this.Y = newY;
        }

        protected void Rotate(float angleDelta)
        {
            AnimationRotate animation = new AnimationRotate(this.RotateSpeed, angleDelta);
            Animations.Enqueue(animation);
        }

        public void RotateLeft(float angleDelta)
        {
            Rotate(angleDelta);
        }

        public void RotateRight(float angleDelta)
        {
            Rotate(-angleDelta);
        }

        public void RotateTo(float newAngle)
        {
            Angle = newAngle;
        }

        public void PenUp()
        {
            Pen.Visible = false;
        }

        public void PenDown()
        {
            Pen.Visible = true;
        }
    }
}
