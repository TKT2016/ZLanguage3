using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogoEngine.Sprites;
using ZOpen2D;

namespace ZLogoEngine.Turtles.Actions
{
    public abstract class TurtleMove:ZLogoActionBase
    {
        protected float speedX;
        protected float speedY;

        public TurtleMove(ZLogoActionBase turtleAction)
            : base(turtleAction)
        {

        }
        public override SpriteInfo GetRunningTurleInfo()
        {
            return _runningTurleInfo;
        }

        public override void RunAction()
        {
            if (State == ActionExecState.Wait)
            {
                State = ActionExecState.Running;
            }
            RunMove();
            bool endx = !IsActIn(_runningTurleInfo.X, _startTurleInfo.X, _endTurleInfo.X);
            if (endx)
                _runningTurleInfo.X = _endTurleInfo.X;

            bool endy = !IsActIn(_runningTurleInfo.Y, _startTurleInfo.Y, _endTurleInfo.Y);
            if (endy)
                _runningTurleInfo.Y = _endTurleInfo.Y;

            if (endx && endy)
            {
                State = ActionExecState.End;
            }
        }

        protected abstract void RunMove();

        public override void Draw()
        {
            //Console.WriteLine(_runningTurleInfo);
            var painter = _startTurleInfo.Painter;
            if (this.State== ActionExecState.End)
            {
                if (_startTurleInfo.Pen.IsDraw)
                {
                    DrawLine(_startTurleInfo, _endTurleInfo);
                    //painter.DrawLine(_startTurleInfo.Position, _endTurleInfo.Position, _startTurleInfo.Pen.Size, _startTurleInfo.Pen.Color);
                }
            }
            else if (this.State == ActionExecState.Running)
            {
                if (_startTurleInfo.Pen.IsDraw)
                {
                    DrawLine(_startTurleInfo, _runningTurleInfo);
                }
                DrawRunningTexture();
            }
        }

        protected virtual void DrawLine(SpriteInfo start,SpriteInfo end)
        {
            var painter = _startTurleInfo.Painter;
            var startPos = start.Position;
            var endPos = end.Position;
            var pen = start.Pen;
            painter.DrawLine(startPos, endPos, pen.Size, pen.Color);
        }
    }
}
