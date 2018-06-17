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

        public void DrawTextureGL(Texture2D texture, float px, float py, float angle)
        {
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
        
        public void DrawTexture(Texture2D texture, Vector2 position,float angle)
        {
            float px = getGLX(position.X);
            float py = getGLY(position.Y);
            DrawTextureGL(texture,px,py,angle);
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

        public Bitmap GetSceneImage()
        {
            int width = (int)this.Width;
            int height = (int)this.Height;
            int x = 0;
            int y = 0;
            var format = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
            var lockMode = System.Drawing.Imaging.ImageLockMode.WriteOnly;
            Bitmap bitmap = new Bitmap(width, height, format);
            var bitmapRect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(bitmapRect, lockMode, format);
            GL.ReadPixels(x, y, width, height, GlPixelFormat.Rgba, PixelType.UnsignedByte, bmpData.Scan0);
            //GL.ReadPixels(x, y, width, height, GL.GL_BGRA, GL.GL_UNSIGNED_BYTE, bmpData.Scan0);
            bitmap.UnlockBits(bmpData);
            bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);

            //bitmap.Save(fileName);
            return bitmap;
        }

        public void SaveSceneImage(string fileName)
        {
            Bitmap bitmap = GetSceneImage();
            bitmap.Save(fileName);

           /*
            var pdata = new UnmanagedArray<Pixel>(width * height);

             GL.ReadPixels(0,0, width,height, GlPixelFormat.Rgb,PixelType.Bitmap,)//  GL.GL_RGBA, GL.GL_UNSIGNED_BYTE, pdata.Header);
             var bitmap = new Bitmap(width, height);
             int index = 0;
             for (int j = height - 1; j >= 0; j--)
             {
                 for (int i = 0; i < width; i++)
                 {
                     Pixel v = pdata[index++];
                     Color c = v.ToColor();
                     bitmap.SetPixel(i, j, c);
                 }
             }
 
             bitmap.Save(fileName);*/
            /*
         GLint ViewPort[4];
         GL.GetInteger(); glGetIntegerv(GL_VIEWPORT,ViewPort);
         GLsizei ColorChannel = 3;
         GLsizei bufferSize = ViewPort[2]*ViewPort[3]*sizeof(GLubyte)*ColorChannel;
         GLubyte * ImgData = (GLubyte*)malloc(bufferSize);
 
          GL.PixelStore();// glPixelStorei(GL_UNPACK_ALIGNMENT,1);
         GL.ReadPixels();// glReadPixels(ViewPort[0],ViewPort[1],ViewPort[2],ViewPort[3],GL_RGB,GL_UNSIGNED_BYTE,ImgData);
 
         FILE * saveTxt=NULL;
         saveTxt=fopen("H:\\Data\\saveImage.txt","w");
         if (!saveTxt)
         {
          cout<<"Cannot save the RGB value！"<<endl;
          getchar();
         }
         for (int i=0;i
         {
          fprintf(saveTxt,"%d  %d  %d\n",ImgData[3*i],ImgData[3*i],ImgData[3*i]);
         }
 
         BITMAPFILEHEADER hdr;
         BITMAPINFOHEADER infoHdr;
 
         infoHdr.biSize = sizeof(BITMAPINFOHEADER);
         infoHdr.biWidth = ViewPort[2];
         infoHdr.biHeight = ViewPort[3];
         infoHdr.biPlanes = 1;
         infoHdr.biBitCount = 24;
         infoHdr.biCompression = 0;
         infoHdr.biSizeImage =ViewPort[2]*ViewPort[3]*3;
         infoHdr.biXPelsPerMeter = 0;
         infoHdr.biYPelsPerMeter = 0;
         infoHdr.biClrUsed = 0;
         infoHdr.biClrImportant = 0;
 
         hdr.bfType = 0x4D42;
         hdr.bfReserved1 = 0;
         hdr.bfReserved2 = 0;
         hdr.bfOffBits = 54;
         hdr.bfSize =(DWORD)(sizeof(BITMAPFILEHEADER)+sizeof(BITMAPINFOHEADER)+ViewPort[2]* ViewPort[3] * 3);
         FILE *fid=NULL;
         if( !(fid = fopen(fileName,"wb+")) )
         {
          cout<<"Cannot load bmp image format!"<<endl;
          getchar();
         }
         fwrite(&hdr,1,sizeof(BITMAPFILEHEADER),fid);
         fwrite(&infoHdr,1,sizeof(BITMAPINFOHEADER),fid);
         fwrite(ImgData,1,ViewPort[2]* ViewPort[2] * 3,fid);
         fclose(fid);
         free(ImgData);
             * */
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
