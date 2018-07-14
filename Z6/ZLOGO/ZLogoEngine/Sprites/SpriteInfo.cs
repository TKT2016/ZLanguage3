using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZGameEngine.Graphics;
using ZOpen2D;

namespace ZLogoEngine.Sprites
{
    public class SpriteInfo
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Angle { get; set; }
        public TurtlePen Pen { get; set; }
        public Texture2D Texture { get; set; }
        public float MoveSpeed { get; set; }
        public float RotateSpeed { get; set; }
        public Draw2D Painter { get; set; }
        public bool Visible { get; set; }

        public SpriteInfo Clone()
        {
            SpriteInfo si = new SpriteInfo();
            si.X = this.X;
            si.Y = this.Y;
            si.Angle = this.Angle;
            si.Texture = this.Texture;
            si.Visible = this.Visible;
            si.MoveSpeed = this.MoveSpeed;
            si.RotateSpeed = this.RotateSpeed;
            si.Painter = this.Painter;
            si.Pen = this.Pen.Clone();
            return si;
        }
        Vector2 _position;
        public Vector2 Position
        {
            get{
                //if (_position == null)
                {
                    _position = new Vector2(X, Y);
                }
                 return _position;
            }
        }

        public override string ToString()
        {
            return string.Format("(({0},{1}),{2})",X,Y,Angle);
        }
    }
}
