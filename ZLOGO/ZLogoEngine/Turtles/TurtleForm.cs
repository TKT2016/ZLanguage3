using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZGameEngine;
using ZLogoEngine.Turtles.Actions;
using ZOpen2D;

namespace ZLogoEngine.Turtles
{
    public class TurtleForm : ZGame
    {
        public TurtleSprite Turtle { get; set; }

        public TurtleForm()
        {
            this.Title = "ZLOGO程序";
        }
        
        public override void Dispose()
        {
            Turtle.Dispose();
        }

        protected override void Draw()
        {
            if (isLoaded)
                Turtle.Draw();
        }

        protected override void Update()
        {
            if (isLoaded)
                Turtle.Update();
        }

        bool isLoaded = false;
        protected override void Load()
        {
            Turtle.RunZLogo();
            Turtle.AddEndAction();
            isLoaded = true;
        }
    }
}
