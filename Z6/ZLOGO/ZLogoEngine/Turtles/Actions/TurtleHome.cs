using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogoEngine.Sprites;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleHome : TurtleChange
    {
        public TurtleHome(ZLogoActionBase turtleAction, SpriteInfo startInfo)
            : base(turtleAction)
        {
            _endTurleInfo = startInfo.Clone();
        }


    }
}
