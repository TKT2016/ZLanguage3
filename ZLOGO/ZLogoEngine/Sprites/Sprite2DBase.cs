using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using ZOpen2D;
using ZGameEngine.Graphics;
using ZLogoEngine.Turtles.Actions;

namespace ZLogoEngine.Sprites
{
    public class Sprite2DBase
    {
        protected internal List<ZLogoActionBase> Animations = new List<ZLogoActionBase>();

        //public float TrimAngle(float Angle)
        //{
        //    Angle = Angle % 360;
        //    if (Angle < 0)
        //    {
        //        Angle += 360;
        //    }
        //    return Angle;
        //}

        public Draw2D Painter { get; protected set; }
        public Texture2D Texture { get; protected set; }

        public void Dispose()
        {
            if (Texture != null)
                Texture.Dispose();
        }

        public virtual void Update()
        {
            ZLogoActionBase currentAnimation2D = GetCurrentAnimation();
            if (currentAnimation2D != null)
            {
                if (currentAnimation2D.State != ActionExecState.End)
                {
                    currentAnimation2D.RunAction();
                }

                if (currentAnimation2D.State == ActionExecState.End)
                {
                    if (CurrentAnimationIndex < Animations.Count - 1)
                        CurrentAnimationIndex++;
                }
            }
        }

        //protected void DrawHistory()
        //{
        //    for (int i = 0; i < CurrentAnimationIndex; i++)
        //    {
        //        var animation = Animations[i];
        //        if (animation is AnimationMove)
        //        {
        //            AnimationMove ato = (animation as AnimationMove);
        //            if (ato.Path.IsDraw)
        //            {
        //                Graphics.DrawLine(ato.Path.FromPosition, ato.Path.ToPosition, ato.Pen.Size, ato.Pen.Color);
        //            }
        //        }
        //    }
        //}

        public virtual void Draw()
        {
            for(int i=0;i<=CurrentAnimationIndex;i++)
            {
                ZLogoActionBase act = Animations[i];
                act.Draw();
            }
            //DrawHistory();
            ////ShowSceneImage();
            //IAnimation2D currentAnimation2D = GetCurrentAnimation();
            //if (currentAnimation2D != null && currentAnimation2D is AnimationMove)
            //{
            //    AnimationMove ato = (currentAnimation2D as AnimationMove);
            //    if (ato.Path.IsDraw)
            //    {
            //        Graphics.DrawLine(ato.Path.FromPosition, ato.Sprite.Positon, ato.Pen.Size, ato.Pen.Color);
            //    }
            //    //Vector2Ex lastPoint = spritePather.Top;
            //    //if(lastPoint.IsDraw)
            //    //    Graphics.DrawLine(lastPoint.Point, this.Positon, Pen.Size, Pen.Color);
            //}

            //if (currentAnimation2D != null)
            //{
            //    Graphics.DrawTexture(this.Texture, currentAnimation2D.Sprite.Positon, currentAnimation2D.Sprite.Angle);
            //}
            //else
            //{
            //    Graphics.DrawTexture(this.Texture, this.Positon, this.Angle);
            //}

            //i++;
            //sceneImage = Graphics.GetSceneImage();
            //DrawTexture();

        }
        //int i = 0;
        //Bitmap sceneImage;
        //private void ShowSceneImage()
        //{
        //    if(sceneImage!=null)
        //    {
        //        Texture2D tt2d = ContentManager.LoadImage(sceneImage);
        //        Graphics.DrawTextureGL(tt2d, 0,0 , 0);
        //    }
        //}
        int CurrentAnimationIndex = 0;
        protected ZLogoActionBase GetCurrentAnimation()
        {
            return Animations[CurrentAnimationIndex];

        }
    }
}
