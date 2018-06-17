using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;
using GlPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using SysPixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ZOpen2D
{
    public class ContentManager
    {
        //private static string RootDirectory { get; set; }
        public static string ContentDirectory { get; set; }

        static ContentManager()
        {
            ContentDirectory = "";
        }

        public static Texture2D LoadImage(string imageRelativePath)
        {
            string imagePath = Path.Combine(ContentDirectory, imageRelativePath);
            
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("找不到文件‘" + imagePath + "’");
            }
            Bitmap textureBitmap = new Bitmap(imagePath);
            return LoadImage(textureBitmap);
        }

        public static Texture2D LoadImage(Bitmap image)
        {
            int id = 0;
            GL.Enable(EnableCap.Texture2D);
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);

            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);

            BitmapData bitmapData = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height),
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

            image.UnlockBits(bitmapData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Linear);
            var texture = new Texture2D(id, image.Width, image.Height);
            return texture;
        }

        public static void Unload(Texture2D texture2D)
        {
            GL.DeleteTexture(texture2D.Id);
        }
        /*
        public static int LoadTexture(string filename)
        {
            int texId = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texId);

            //string path = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string imagePath = Path.Combine(RootDirectory, filename);
            Bitmap TextureBitmap = new Bitmap(imagePath);
            BitmapData data = TextureBitmap.LockBits(new Rectangle(0, 0, TextureBitmap.Width, TextureBitmap.Height)
                , ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height,
                    0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);


            TextureBitmap.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, 0);
           // textures.Add(texId);

            return texId;
        }*/

        /*
        private static Texture2D CreateSpriteResources(string contentPath)
        {
            string imagePath = Path.Combine(RootDirectory, contentPath);
            if (RootDirectory == null)
            {
                Console.WriteLine("ERROR! No content root set");
                return null;
            };
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("ERROR! Coudlnt find file:" + imagePath);
                return null;
            }
            Bitmap bitmap = new Bitmap(imagePath);
            return CreateSpriteResources(bitmap);
        }

        private static Texture2D CreateSpriteResources(Bitmap image)
        {
             Texture2D texture = new Texture2D();
            int texID = 0;
            GL.GenTextures( 1, out texID );
            GL.BindTexture( TextureTarget.Texture2D, texID );

            //get raw data from the bitmap
            BitmapData data = image.LockBits( new Rectangle( 0, 0, image.Width, image.Height ), ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb );

            //fill the texture with data
            GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0 );

            image.UnlockBits( data );

            //USize = 1 / (float) HorizontalSegments;
            //VSize = 1 / (float) VerticalSegments;

            GL.Flush();
            texture.Id = texID;
            texture.Width = image.Width;
            texture.Height = image.Height;
            return texture;
        }
        */
        /*
        public static Texture2D LoadImage(string contentPath)
        {
            //Bitmap logoBitmap = new Bitmap(imagePath);
            //Texture2D texture2D = new Texture2D();
            string imagePath = Path.Combine(RootDirectory, contentPath);
            if (RootDirectory == null)
            {
                Console.WriteLine("ERROR! No content root set");
                return null;
            };
            if (!File.Exists(imagePath))
            {
                Console.WriteLine("ERROR! Coudlnt find file:" + imagePath);
                return null;
            }
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);


            Bitmap bitmap = new Bitmap(imagePath);

            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
            ImageLockMode.ReadOnly,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
            OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            Texture2D texture = new Texture2D();
            texture.Id = id;
            texture.Width = bitmap.Width;
            texture.Height = bitmap.Height;
            return texture;
        }*/

    }
}
