using System;
using OpenTK.Graphics.OpenGL;

namespace ZOpen2D
{
	public class Texture2D:IDisposable
	{
        public int Id { get; private set; }
        public float Width { get; private set; }
        public float Height { get; private set; }

        public float HalfWidth{ get;private set;}
        public float HalfHeight{ get;private set;}

	    public Texture2D(int id, float width, float height)
	    {
	        Id = id;
            this.Width = width;
            this.Height = height;
            HalfWidth = this.Width / 2.0f;
            HalfHeight = this.Height / 2.0f;
	    }

	    public void Dispose()
	    {
	        int id = Id;
            GL.DeleteTextures(1, ref id);
	    }
	}
}
