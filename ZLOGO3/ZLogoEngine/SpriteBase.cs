using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using ZGameEngine;
using ZGameEngine.Graphics;
using ZOpen2D;

namespace ZLogoEngine
{
    public class SpriteBase :Sprite2DBase, IDisposable
    {
        public Draw2D Graphics { get; protected set; }
        public Texture2D Texture { get;protected set; }

        protected virtual void DrawTexture()
        {
            Graphics.DrawTexture(this.Texture, this.Positon, this.Angle);
        }

        public virtual void Update()
        {
          
        }

        public virtual void Draw()
        {
            DrawTexture();
        }

        public void Dispose()
        {
            if(Texture!=null)
                Texture.Dispose();
        }
    }
}
