using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using ZLogoEngine;
using ZOpen2D;

namespace ZGameEngine
{
    public class AnimationTo:IAnimation2D
    {
        public Vector2 FromPosition { get; set; }
        public Vector2 ToPosition { get; set; }
        public float Speed { get; set; }
        public AnimationState State { get;private set; }     
        public float Distance { get; private set; }

        private float speedx;
        private float speedy;
        private bool isBack = false;

        public AnimationTo(float speed, float distance,bool isBack)
        {
            State = AnimationState.Wait;
            Speed = speed;
            Distance = distance;
            this.isBack = isBack;
        }

        private ISprite2D _sprite;
        public ISprite2D Sprite
        {
            get { return _sprite; }
            set
            {
                _sprite = value;
                FromPosition = new Vector2(_sprite.X, _sprite.Y);
                //Console.WriteLine("FromPosition:" + FromPosition);
                var angle = _sprite.Angle;
                if (isBack)
                {
                    angle += 180;
                }
                ToPosition = Vector2Util.GetPointByPolar(FromPosition.X, FromPosition.Y, Distance, angle);
                speedx = (float)(Speed * MathUtil.Cos(angle));
                speedy = (float)(Speed * MathUtil.Sin(angle));
            }
        }

        public void Run()
        {
            //var currentX = _sprite.X;
            //var currentY = _sprite.Y;
            if (State == AnimationState.Wait)
            {
                State = AnimationState.Running;
            }
            if (State == AnimationState.Running)
            {
                _sprite.X += speedx;
                _sprite.Y += speedy;
            }
            //bool endx = isEndx();
            //bool endy = isEndy();
            bool endx = isEnd(speedx,_sprite.X,ToPosition.X);
            bool endy = isEnd(speedy, _sprite.Y, ToPosition.Y);
            if (endx)
                _sprite.X = ToPosition.X;
            if (endy)
                _sprite.Y = ToPosition.Y;
            if (endx && endy)
            {
                State = AnimationState.End;
                //Console.WriteLine("end x,y:"+_sprite.X+","+_sprite.Y) ;
            }
        }

        private bool isEnd(float speedCoord,float currentCoord,float toCoord)
        {
            if (speedCoord == 0) return true;
            if (currentCoord == toCoord) return true;
            if (speedCoord > 0)
            {
                if (currentCoord >= toCoord)
                {
                    return true;
                }
            }
            else if (speedCoord < 0)
            {
                if (currentCoord <= toCoord)
                {
                    return true;
                }
            }
            return false;
        }
        /*
        private bool isEndx()
        {
            if (speedx == 0) return true;
            var currentX = _sprite.X;
            if (currentX == ToPosition.X) return true;
            if (speedx > 0)
            {
                if (currentX >= ToPosition.X)
                {
                    //currentX = ToPosition.X;
                    return true;
                }
            }
            else if (speedx < 0)
            {
                if (currentX <= ToPosition.X)
                {
                    //currentX = ToPosition.X;
                    return true;
                }
            }
            return false;
        }

        private bool isEndy()
        {
            if (speedy == 0) return true;
            var currentY = _sprite.Y;
            if (currentY == ToPosition.Y) return true;
            if (speedy > 0)
            {
                if (currentY >= ToPosition.Y)
                {
                    //currentY = ToPosition.Y;
                    return true;
                }
            }
            else if (speedy < 0)
            {
                if (currentY <= ToPosition.Y)
                {
                    //currentY = ToPosition.Y;
                    return true;
                }
            }
            return false;
        }*/
    }
}
