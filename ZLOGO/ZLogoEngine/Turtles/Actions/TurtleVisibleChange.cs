using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogoEngine.Sprites;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleVisibleChange : TurtleChange
    {
        public TurtleVisibleChange(ZLogoActionBase turtleAction, bool visible)
            : base(turtleAction)
        {
            _endTurleInfo.Visible = visible;
        }


    }
}
