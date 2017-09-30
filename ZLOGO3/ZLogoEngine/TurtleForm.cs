using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZGameEngine;

namespace ZLogoEngine
{
    public class TurtleForm : ZGame
    {
        public TurtleSprite Turtle { get;private set; }
        public TurtleForm Window { get; private set; }

        public TurtleForm()
        {
            this.Title = "ZLOGO程序";
            StackFrame[] stacks = new StackTrace().GetFrames();
            Turtle = new TurtleSprite(this);
            Window = this;
        }
        
        public override void Dispose()
        {
            Turtle.Dispose();
        }

        protected override void Draw()
        {
            Turtle.Draw();
        }

        protected override void Update()
        {
            //if (isFocused)
            Turtle.Update();
        }

        public virtual void RunZLogo()
        {
            
        }

        protected override void Load()
        {
            RunZLogo();
        }

        public void SetTitle(string title)
        {
            this.Title = title;
        }
        /*
        bool isFocused=false  ;
        protected override void Focused()
        {
            isFocused = true;
        }*/
       
    }
}
