using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogoEngine.Sprites;

namespace ZLogoEngine.Turtles.Actions
{
    public abstract class ZLogoActionBase
    {
        public ActionExecState State { get; protected set; }

        protected SpriteInfo _startTurleInfo;
        protected SpriteInfo _endTurleInfo;
        protected SpriteInfo _runningTurleInfo;

        public ZLogoActionBase( )
        {
            State = ActionExecState.Wait;
        }

        public ZLogoActionBase(ZLogoActionBase turtleAction)
        {
            State = ActionExecState.Wait;
            _startTurleInfo = turtleAction.GetEndTurleInfo().Clone();
            _endTurleInfo = turtleAction.GetEndTurleInfo().Clone();
            _runningTurleInfo = turtleAction.GetEndTurleInfo().Clone();
        }

        public virtual SpriteInfo GetStartTurleInfo()
        {
            return _startTurleInfo;
        }

        public virtual SpriteInfo GetEndTurleInfo()
        {
            return _endTurleInfo;
        }

        public abstract SpriteInfo GetRunningTurleInfo();
        public abstract void RunAction();
        public abstract void Draw();

        protected void DrawRunningTexture()
        {
            if (_startTurleInfo.Visible)
            {
                var painter = _startTurleInfo.Painter;
                SpriteInfo runningInfo = GetRunningTurleInfo();
                painter.DrawTexture(runningInfo.Texture, runningInfo.Position, runningInfo.Angle);
            }
        }

        protected bool IsActIn(float a, float b, float c)
        {
            if (a == 0) return false;
            if (a == c) return false;
            if (b == c) return false;
            else if (c > b)
            {
                return a >= b && a < c;
            }
            else if (c < b)
            {
                return a >= c && a < b;
            }
            return false;
        }
    }
}
