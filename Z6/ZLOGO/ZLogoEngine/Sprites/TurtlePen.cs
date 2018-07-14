using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZLogoEngine.Sprites
{
    public class TurtlePen
    {
        public float Size { get; set; }
        public Color Color { get; set; }
        public bool IsDraw { get; set; }

        public TurtlePen()
        {
            IsDraw = true;
        }

        public TurtlePen Clone()
        {
            TurtlePen pen = new TurtlePen();
            pen.Size = this.Size;
            pen.Color = this.Color;
            pen.IsDraw = this.IsDraw;
            return pen;
        }
    }
}
