using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLogoEngine.Turtles.Actions
{
    public abstract class TurtleChange : ZLogoActionBase
    {
        public TurtleChange( )
            : base()
        {

        }

        public TurtleChange(ZLogoActionBase turtleAction)
            : base(turtleAction)
        {
          
        }

        public override void RunAction()
        {
            State = ActionExecState.End;
        }

        public override void Draw()
        {
            if(State == ActionExecState.Running)
                DrawRunningTexture();
        }

        public override Sprites.SpriteInfo GetRunningTurleInfo()
        {
            return _endTurleInfo;
        }
    }
}
