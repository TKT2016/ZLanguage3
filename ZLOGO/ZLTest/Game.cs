using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ZGameEngine.Graphics;
using ZOpen2D;
using GlPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SysPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ZLTest
{

    public class Game : ZOpen2DWindow
    {
        private int _texture;
        private Draw2D draw2D;
        private Texture2D textureLogo;

        private static int width =800;
        private static int height =600;
        private int textureWidth = 400;
        private int textureHeight = 300;

        public Game()
            : base(800, 600)
        {
            draw2D = new Draw2D(800, 600);
        }

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e) {
             Bitmap bitmap2 = new Bitmap("logo.jpg");
             textureLogo = ContentManager.LoadImage(bitmap2);

            GL.ClearColor(Color.MidnightBlue);
            //GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out _texture);
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            BitmapData bitmapData = bitmap2.LockBits(
                new Rectangle(0, 0, bitmap2.Width, bitmap2.Height),
                ImageLockMode.ReadOnly,
                SysPixelFormat.Format32bppArgb
                );

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmapData.Width,
                bitmapData.Height,
                0,
                GlPixelFormat.Bgra,
                PixelType.UnsignedByte,
                bitmapData.Scan0
                );

            bitmap2.UnlockBits(bitmapData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            textureWidth = bitmapData.Width;
            textureHeight = bitmapData.Height;
        }

        protected override void OnUnload(EventArgs e) {
            //base.OnUnload(e);
            GL.DeleteTextures(1, ref _texture);
        }

        /// <summary>
        ///     Called when your window is resized. Set your viewport here. It is also
        ///     a good place to set up your projection matrix (which probably changes
        ///     along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Contains information on the new Width and Size of the GameWindow.</param>
        protected override void OnResize(EventArgs e) {
            GL.Viewport(ClientRectangle);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        /// <summary>
        ///     Called when it is time to setup the next frame. Add you game logic here.
        /// </summary>
        /// <param name="e">Contains timing information for framerate independent logic.</param>
        protected override void OnUpdateFrame(FrameEventArgs e) {
            if (Keyboard[Key.Escape]) {
                Exit();
            }
        }

      

        /// <summary>
        ///     Called when it is time to render the next frame. Add your rendering code here.
        /// </summary>
        /// <param name="e">Contains timing information.</param>
        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            drawTexture(-100,100);
            draw2D.DrawTexture(textureLogo, new Vector2(200, 100), 45);
            SwapBuffers();
        }


        Vector2 getGLVector2(float x, float y)
        {
            float glX = x/(width/2.0f);
            float glY = y / (height / 2.0f);
            return new Vector2(glX, glY);
        }

        float getGLX(float x)
        {
            return x/(width/2.0f);
        }

        float getGLY(float y)
        {
            return y / (height / 2.0f);
        }

        void drawTexture(float x, float y)
        {
            float lx =getGLX(x - textureWidth/2.0f);
            float rx = getGLX(x + textureWidth / 2.0f);
            float dy = getGLY(y - textureHeight / 2.0f);
            float uy = getGLY(y + textureHeight/2.0f);
          
            GL.BindTexture(TextureTarget.Texture2D, _texture);

            GL.Begin(PrimitiveType.Quads);
            //GL.Translate(-1.5f, 0.0f, -6.0f);  
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(lx,dy); // GL.Vertex2(-1.0f, -1.0f); //GL.Vertex2(-0.6f, -0.4f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(rx, dy); // GL.Vertex2(1.0f, -1.0f); //GL.Vertex2(0.6f, -0.4f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(rx, uy); //GL.Vertex2(1.0f, 1.0f); //GL.Vertex2(0.6f, 0.4f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(lx, uy); //GL.Vertex2(-1.0f, 1.0f); //GL.Vertex2(-0.6f, 0.4f);

            GL.End();
        }
    }
}
