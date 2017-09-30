using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK;
using ZGameEngine;

namespace ZLogoEngine
{
    public class DrawSprite : SpriteBase
    {
        public float MoveSpeed { get; set; }
        public float RotateSpeed { get; set; }
       
        //public bool PenVisible { get; set; }

        //public float PenSize { get; set; }
        //public Color PenColor { get; set; }
        public TurtlePen Pen { get; set; }
        public virtual void Init()
        {
            X = 0;
            Y = 0;
            //PenVisible = true;    
            DrawedPoints.Add(this.Positon);
            
        }

        protected Queue<IAnimation2D> Animations = new Queue<IAnimation2D>();
        protected List<Vector2> DrawedPoints = new List<Vector2>();

        public override void Update()
        {
            IAnimation2D currentAnimation2D = GetCurrentAnimation();
            if (currentAnimation2D!=null)
            {
                if (currentAnimation2D.State == AnimationState.Wait)
                {
                    currentAnimation2D.Sprite = this;              
                }

                if (currentAnimation2D.State != AnimationState.End)
                {
                    currentAnimation2D.Run();
                }        
                if (currentAnimation2D.State == AnimationState.End)
                {
                    if (currentAnimation2D is AnimationTo)
                    {
                        AnimationTo animationTo = (currentAnimation2D as AnimationTo);
                        DrawedPoints.Add(animationTo.ToPosition);
                    }
                    Animations.Dequeue();
                    //isd = true;
                }
            }
        }

        //private bool isd = false;
            
        protected virtual void DrawRunedLines()
        {
            if (DrawedPoints.Count >= 2)
            {
                for (int i = 0; i < DrawedPoints.Count - 1; i++)
                {
                    Graphics.DrawLine(DrawedPoints[i], DrawedPoints[i+1], Pen.Size, Pen.Color);
                }
            }
        }

        public override void Draw()
        {
            DrawRunedLines();
            
            IAnimation2D currentAnimation2D = GetCurrentAnimation();
            if (currentAnimation2D is AnimationTo)
            {
                AnimationTo animationTo = (currentAnimation2D as AnimationTo);
                Vector2 fromVector = DrawedPoints[DrawedPoints.Count - 1];
                Graphics.DrawLine(fromVector, this.Positon, Pen.Size, Pen.Color);
               /* if (isd)
                {
                    isd = false;
                    Console.WriteLine(string.Format("{0}->{1}", fromVector, this.Positon));
                }*/
            }
            DrawTexture(); //base.Draw();
            //Graphics.DrawLine(new Vector2(-100, 100), new Vector2(100, -100), 10, Color.Red);
        }

        protected IAnimation2D GetCurrentAnimation()
        {
            if (Animations.Count > 0) return Animations.Peek();
            return null;
        }
     

    }
}
