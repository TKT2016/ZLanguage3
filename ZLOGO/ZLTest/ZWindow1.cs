using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ZGameEngine.Graphics;
using GlPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SysPixelFormat = System.Drawing.Imaging.PixelFormat;

using ZOpen2D;

namespace ZLTest
{
    class ZWindow1 : ZOpen2DWindow
    {
        private Draw2D draw2D;

        public ZWindow1() : base(800, 600)
        {
            draw2D = new Draw2D(800, 600);
        }

        private Texture2D textureTurtle;
        private Texture2D textureLogo;

        /// <summary>Load resources here.</summary>
        /// <param name="e">Not used.</param>
        protected override void OnLoad(EventArgs e)
        {
            Bitmap bitmap = new Bitmap("Turtle.png");
            textureTurtle = ContentManager.LoadImage(bitmap);
            Bitmap bitmap2 = new Bitmap("logo.jpg");
            textureLogo = ContentManager.LoadImage(bitmap2);
            this.Title = "测试TEST";
        }

        protected override void OnUnload(EventArgs e) {
            //
            //GL.DeleteTextures(1, ref _texture);
            textureTurtle.Dispose();
            textureLogo.Dispose();
            base.OnUnload(e);
        }

        /// <summary>
        ///     Called when your window is resized. Set your viewport here. It is also
        ///     a good place to set up your projection matrix (which probably changes
        ///     along when the aspect ratio of your window).
        /// </summary>
        /// <param name="e">Contains information on the new Width and Size of the GameWindow.</param>
        protected override void OnResize(EventArgs e) {
            /*GL.Viewport(ClientRectangle);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);*/
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
            GL.ClearColor(Color.White);
            //drawTexture(-100,100);
            GL.LoadIdentity();
            draw2D.DrawTexture(textureTurtle, new Vector2(350, 100),90);
            GL.LoadIdentity();
            //draw2D.DrawTexture(textureTurtle, new Vector2(-50, -100), 90);
            //draw2D.DrawTexture(textureLogo, new Vector2(-100, 100), 45);

            draw2D.DrawLine(new Vector2(-100,100 ),new Vector2(100,-100 ),10,Color.Red);
            SwapBuffers();
        }

    }
}
