using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogoEngine.Sprites;
using ZOpen2D;

namespace ZLogoEngine.Turtles.Actions
{
    public abstract class TurtleRotate : ZLogoActionBase
    {
        protected float RotatingAngle;

        public TurtleRotate(ZLogoActionBase turtleAction)
            : base(turtleAction)
        {

        }

        public override void RunAction()
        {
            if (State == ActionExecState.Wait)
            {
                State = ActionExecState.Running;
                RotatingAngle = _startTurleInfo.Angle;
            }
            RunRotate(); //runangle -= _startTurleInfo.RotateSpeed;

            bool endx = !IsActIn(RotatingAngle, _startTurleInfo.Angle, _endTurleInfo.Angle);
            if (endx)
            {
                RotatingAngle = _endTurleInfo.X;
                State = ActionExecState.End;
            }
        }

        public override SpriteInfo GetRunningTurleInfo()
        {
            _runningTurleInfo.Angle = RotatingAngle;
            return _runningTurleInfo;
        }

        protected abstract void RunRotate();

        //protected bool isBetween(float a,float b ,float c)
        //{
        //    if (a == 0) return false;
        //    if(b==c) return true;
        //    else if(c>b)
        //    {
        //        return a>=b && a<c;
        //    }
        //    else if (c < b)
        //    {
        //        return a >= c && a < b;
        //    }
        //    return false;
        //}

        public override void Draw()
        {
            var painter = _startTurleInfo.Painter;
            if (this.State == ActionExecState.Running)
            {
                Vector2 currentPos = _startTurleInfo.Position;
                painter.DrawTexture(_startTurleInfo.Texture, currentPos, RotatingAngle);
            }
        }
    }
}
