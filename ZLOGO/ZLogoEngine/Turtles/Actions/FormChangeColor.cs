using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ZLogoEngine.Turtles.Actions
{
    public class FormChangeColor : ZLogoActionBase
    {
        Color bgColor;
        TurtleForm turtleForm;

         public FormChangeColor(ZLogoActionBase turtleAction,TurtleForm tform, Color bgColor)
            : base(turtleAction)
        {
            this.turtleForm = tform;
            this.bgColor = bgColor;
        }

        public override void RunAction()
        {
            this.turtleForm.BackgroundColor = bgColor;
            State = ActionExecState.End;
        }

        public override void Draw()
        {
            
        }

        public override Sprites.SpriteInfo GetRunningTurleInfo()
        {
            return _endTurleInfo;
        }
    }
}
