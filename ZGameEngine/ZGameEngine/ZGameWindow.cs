using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;
using ZGameEngine.Graphics;
using ZOpen2D;

namespace ZGameEngine
{
    public class ZGameWindow : ZOpen2DWindow
    {
        //private int _texture;
        //private Draw2D draw2D;

        public ZGameWindow(int width, int height)
            : base(width, height)
        {
            _BackgroundColor = Color.White;
        }

        internal Action LoadAction { get; set; }

        Color _BackgroundColor;
        public Color BackgroundColor
        {
            get { return _BackgroundColor; }
            set { _BackgroundColor = value; GL.ClearColor(_BackgroundColor); }
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(_BackgroundColor);
            //GL.InitNames();
            //GL.Enable(EnableCap.DepthTest);
            GL.Enable( EnableCap.LineSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);// GL_LINE_SMOOTH, GL_NICEST);

            GL.Enable(EnableCap.Blend);
            GL.Disable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            //GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            if (LoadAction != null)
            {
                LoadAction();
            }
        }

        internal Action UnLoadAction { get; set; }
        protected override void OnUnload(EventArgs e) {
            if (UnLoadAction != null)
            {
                UnLoadAction();
            }
            base.OnUnload(e);
        }

        protected override void OnResize(EventArgs e) {
            GL.Viewport(ClientRectangle);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(-1.0, 1.0, -1.0, 1.0, 0.0, 4.0);
        }

        internal Action UpdateAction { get; set; }
        protected override void OnUpdateFrame(FrameEventArgs e) {
            if (UpdateAction != null)
            {
                UpdateAction();
            }
        }

        internal Action DrawAction { get; set; }
      
        protected override void OnRenderFrame(FrameEventArgs e) {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            if (DrawAction != null)
            {
                DrawAction();
            }
            GL.Flush();
            SwapBuffers();   
        }
    }
}
