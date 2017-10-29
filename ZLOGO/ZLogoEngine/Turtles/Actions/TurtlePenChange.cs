using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ZLogoEngine.Sprites;

namespace ZLogoEngine.Turtles.Actions
{
    public class TurtlePenChange : TurtleChange
    {
        public TurtlePenChange(ZLogoActionBase turtleAction,TurtlePen pen)
            : base(turtleAction)
        {
            _endTurleInfo.Pen = pen.Clone();
        }

        public TurtlePenChange(ZLogoActionBase turtleAction, bool isDraw)
            : base(turtleAction)
        {
            _endTurleInfo.Pen.IsDraw = isDraw;
        }

        public TurtlePenChange(ZLogoActionBase turtleAction, Color color)
            : base(turtleAction)
        {
            _endTurleInfo.Pen.Color = color;
        }
    }
}
