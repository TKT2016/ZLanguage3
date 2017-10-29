using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZOpen2D;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleRotateLeft : TurtleRotate
    {
        public TurtleRotateLeft(ZLogoActionBase turtleAction, float degrees)
            : base(turtleAction)
        {
            //_startTurleInfo = turtleAction.GetEndTurleInfo().Clone();
            _endTurleInfo.Angle = _startTurleInfo.Angle+degrees;
        }

        protected override void RunRotate()
        {
            RotatingAngle += _startTurleInfo.RotateSpeed;
        }
    }
}
