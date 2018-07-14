using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using ZGameEngine.Graphics;
using ZOpen2D;
using System.Drawing;

namespace ZOpen2D
{
    public class ZGame:IDisposable
    {
        private ZGameWindow _window;
        public Draw2D Graphics { get; private set; }

        private void InitWindow(int width,int height)
        {
            _window = new ZGameWindow(width, height);
            _window.LoadAction = this.Load;
            _window.UpdateAction = this.Update;
            _window.DrawAction = this.Draw;
            _window.UnLoadAction = this.Unload;
            //_window.FocusedAction = this.Focused;
            //_window.ResizeAction = this.Resize;
            Graphics = new Draw2D(width, height);
            ContentManager.ContentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public ZGame( )
        {
            InitWindow(800, 600);
           
        }

        public ZGame(int width,int height)
        {
            InitWindow(width, height);
        }

        public Color BackgroundColor
        {
            get { return _window.BackgroundColor; }
            set { _window.BackgroundColor = value; }
        }

        public string Title
        {
            get { return _window.Title; }
            set { _window.Title = value; }
        }

        public int Width
        {
            get { return this._window.Width; }
        }

        public int Height
        {
            get { return this._window.Height; }
        }
        
        public void Exit()
        {
            _window.Exit();
        }
        /*
        public KeyboardDevice Keyboard
        {
            get { return _window.Keyboard; }
        }

        public MouseDevice Mouse
        {
            get { return _window.Mouse; }
        }
        */
        protected virtual void Load()
        {

        }

        public virtual void Run()
        {
            _window.Run(60.0);
        }

        protected virtual void Update( )
        {
            
        }

        protected virtual void Draw( )
        {

        }
        
        protected virtual void Resize( )
        {

        }

        /*
        protected virtual void Initialize()
        {
            
        }*/

        protected virtual void Unload()
        {
            this.Dispose();
        }

        public virtual void Dispose()
        {
            
        }
        /*
        protected virtual void Focused()
        {
            
        }*/

    }
}
