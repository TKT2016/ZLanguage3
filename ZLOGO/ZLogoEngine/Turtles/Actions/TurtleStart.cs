using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZGameEngine.Graphics;
using ZLogoEngine.Sprites;
using System.Drawing;
using ZOpen2D;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtleStart : TurtleChange
    {
        public TurtleStart(Draw2D painter)
           // : base()
        {
            _startTurleInfo = new SpriteInfo() { 
                Angle=90,  MoveSpeed=2, RotateSpeed=2 , X=0, Y=0, 
                Visible =true,
                Painter=painter ,
                Pen = new TurtlePen (){ Color = Color.Blue , Size=2, IsDraw =true},
                Texture = ContentManager.LoadImage("Content/Turtle.png")
            };

            _endTurleInfo = _startTurleInfo.Clone();
            _runningTurleInfo = _startTurleInfo.Clone();
        }
    }
}
