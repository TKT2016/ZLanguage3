using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZOpen2D;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleMoveForward : TurtleMove
    {
        public TurtleMoveForward(ZLogoActionBase turtleAction,float distance)
            : base(turtleAction)
        {
            //_startTurleInfo = turtleAction.GetEndTurleInfo().Clone();
            var angle = _startTurleInfo.Angle;
            var speed = _startTurleInfo.MoveSpeed;
            Vector2 ToPosition = Vector2Util.GetPointByPolar(_startTurleInfo.X, _startTurleInfo.Y, distance, angle);
            //_endTurleInfo = _startTurleInfo.Clone();
            _endTurleInfo.X = ToPosition.X;
            _endTurleInfo.Y = ToPosition.Y;

            speedX = (float)(speed * MathUtil.Cos(angle));
            speedY = (float)(speed * MathUtil.Sin(angle));
        }

        protected override void RunMove()
        {
            _runningTurleInfo.X += speedX;
            _runningTurleInfo.Y += speedY;
            //movingX += speedX;
            //movingY += speedY;
        }
    }
}
