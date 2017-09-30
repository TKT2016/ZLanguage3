using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLogoEngine;

namespace ZLTest
{
    class LogoTest1 : TurtleForm
    {
        public override void RunZLogo()
        {
            /*Turtle.RotateLeft(45);
            Turtle.Backward(100);
            Turtle.RotateLeft(90);
            Turtle.Forward(200);
            Turtle.RotateRight(30);
            Turtle.Forward(100);*/

            Turtle.Forward(100);
            Turtle.RotateLeft(90);
            Turtle.Forward(100);
            Turtle.RotateLeft(90);
            Turtle.Forward(100);
            Turtle.RotateLeft(90);
            Turtle.Forward(100);
            Turtle.RotateLeft(90);
        }
    }
}
