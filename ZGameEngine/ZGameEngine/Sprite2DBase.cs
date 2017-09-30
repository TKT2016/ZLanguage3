using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace ZGameEngine
{
    public class Sprite2DBase : ISprite2D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Angle{ get; set; }

        public void TrimAngle()
        {
            Angle = Angle % 360;
            if (Angle < 0)
            {
                Angle += 360;
            }
        }

        public Vector2 Positon
        {
            get
            {
                return new Vector2(X, Y);
            }
            set
            {
                this.X = value.X;
                this.Y = value.Y;
            }
        }
    }
}
