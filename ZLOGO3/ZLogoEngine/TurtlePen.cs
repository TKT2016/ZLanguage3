using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZLogoEngine
{
    public class TurtlePen
    {
        public float Size { get; set; }
        public Color Color { get; set; }
        public bool Visible { get; set; }

        public TurtlePen()
        {
            Visible = true;
        }
    }
}
