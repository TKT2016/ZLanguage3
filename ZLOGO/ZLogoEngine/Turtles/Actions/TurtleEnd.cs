using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZGameEngine.Graphics;
using ZLogoEngine.Sprites;
using System.Drawing;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleEnd : TurtleChange
    {
        public TurtleEnd(ZLogoActionBase turtleAction)
            : base(turtleAction)
        {
             
        }

        public override void Draw()
        {
             DrawRunningTexture();
        }
    }
}
