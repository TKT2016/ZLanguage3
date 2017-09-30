using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ZOpen2D
{
    public class ZOpen2DWindow : GameWindow
    {
        public ZOpen2DWindow(int width, int height)
            : base(width, height)
        {
            //GL.Viewport(0, 0, Width, Height);
        }
    }
}
