using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using ZOpen2D;
using GlPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SysPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ZGameEngine.Graphics
{
    public class Draw2D
    {
        public float Width { get; private set; }
        public float Height { get; private set; }

        private float HalfWidth;
        private float HalfHeight;

        public Draw2D(float width, float height)
        {
            this.Width = width;
            this.Height = height;
            HalfWidth = this.Width/2.0f;
            HalfHeight = this.Height / 2.0f;
        }

        private float getGLX(float x)
        {
            return x / HalfWidth;
        }

        private float getGLY(float y)
        {
            return y / HalfHeight;
        }

        //private int i = 0;
        public void DrawTexture(Texture2D texture, Vector2 position,float angle)
        {
            float px = getGLX(position.X);
            float py = getGLY(position.Y);

            GL.PushMatrix();
            GL.Translate(px, py, 0f);
            //GL.Scale(Scale.X, Scale.Y, 1f);
            GL.Rotate(angle, Vector3.UnitZ);//GL.Rotate(Rotation, 0f, 0f, 1f);
            Color color = Color.White;
            GL.Color4(color.R, color.G, color.B, (byte)255);

            GL.BindTexture(TextureTarget.Texture2D, texture.Id);

            GL.Begin(PrimitiveType.Quads);

           // float w = Origin.X / (Texture.Texture.Width / 1);
           // float h = Origin.Y / (Texture.Texture.Height / 1);
            float wx = getGLX(texture.HalfWidth);
            float wy = getGLY(texture.HalfHeight);

            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(-wx, -wy); // GL.Vertex2(-1.0f, -1.0f); //GL.Vertex2(-0.6f, -0.4f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(wx, -wy); // GL.Vertex2(1.0f, -1.0f); //GL.Vertex2(0.6f, -0.4f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(wx, wy); //GL.Vertex2(1.0f, 1.0f); //GL.Vertex2(0.6f, 0.4f);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(-wx, wy); //GL.Vertex2(-1.0f, 1.0f); //GL.Vertex2(-0.6f, 0.4f);
            GL.End();

            GL.PopMatrix();
       
        }

        public void DrawLine(Vector2 start, Vector2 end, float lineWidth, Color lineColor)
        {
            GL.PushMatrix();
            //GL.Translate(Position.X, Position.Y, 0f);
            //GL.Scale(Scale.X, Scale.Y, 0f);
            //GL.Rotate(Rotation, 0f, 0f, 1f);
            GL.LineWidth(lineWidth);
            GL.Color3(lineColor); //GL.Color4(lineColor.R, lineColor.G, lineColor.B, (byte)0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Begin(PrimitiveType.Lines);
            float sx = getGLX(start.X);
            float sy = getGLY(start.Y);
            float ex = getGLX(end.X);
            float ey = getGLY(end.Y);
            GL.Vertex2(sx, sy);
            GL.Vertex2(ex, ey);
            //GL.Vertex2(0f, 0f);
            //GL.Vertex2(1f, 0f);
            GL.End();
            GL.PopMatrix();
        }

        public void ClearBuffer()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Clear(Color color)
        {
            var red = color.R;
            var green = color.G;
            var blue = color.B;
            GL.ClearColor(red, green, blue, 0.0f);
            //GL.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
        }
        /*
        public static void DrawRectangle(Vector2 position, Vector2 size)
        {
            var z = 2;
            //var zx = position.X;
            var x_inc = position.X + size.X;
            var y_inc = position.Y + size.Y;
            GL.Begin(BeginMode.Quads);
            GL.Vertex3(position.X, y_inc, z); //GL.Vertex3(60, 550, 2);//// Move Up One Unit From Center (Top Point)
            GL.Vertex3(x_inc, y_inc, z); //GL.Vertex3(60 + mainPlayer.ReinforceTimer, 550, 2);// Left And Down One Unit (Bottom Left)
            GL.Vertex3(position.X, position.Y, z);//GL.Vertex3(60 + mainPlayer.ReinforceTimer, 582, z);// Right And Down One Unit (Bottom Right)
            GL.Vertex3(x_inc, position.Y, z);//GL.Vertex3( 60, 582, 2 );// Done Drawing A Triangle
            GL.End();
        }

        public static void DrawVertice(Vector2 uvsize, Vector2 size, float u,float v)
        {
            var USize = uvsize.X;
            var VSize = uvsize.Y;

            var Width = size.X;
            var Height = size.Y;

            GL.TexCoord2(new Vector2(USize * u, VSize * v));
            GL.Vertex3(new Vector3(0, 0, 0));

            GL.TexCoord2(new Vector2(USize * (u + 1), VSize * v));
            GL.Vertex3(new Vector3(Width, 0, 0));

            GL.TexCoord2(new Vector2(USize * (u + 1), VSize * (v + 1)));
            GL.Vertex3(new Vector3(Width, Height, 0));

            GL.TexCoord2(new Vector2(USize * u, VSize * (v + 1)));
            GL.Vertex3(new Vector3(0, Height, 0));
        }*/

    }
}
