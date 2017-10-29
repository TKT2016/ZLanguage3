using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZOpen2D;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleJump : TurtleChange
    {
        public TurtleJump(ZLogoActionBase turtleAction, float x,float y)
            : base(turtleAction)
        {
            _endTurleInfo.X = x;
            _endTurleInfo.Y = y;
        }
    }
}
