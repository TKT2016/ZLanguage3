using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using ZLogoEngine;
using ZOpen2D;

namespace ZGameEngine
{
    public class AnimationRotate:IAnimation2D
    {
        public AnimationState State { get; private set; }
        public float FromAngle { get; set; }
        public float ToAngle { get; set; }
        public float Speed { get; set; }
        public float ChangeDegrees { get; private set; }

        public AnimationRotate(float speed, float degrees)
        {
            State= AnimationState.Wait;
            ChangeDegrees = degrees;
            Speed = speed;
        }

        private ISprite2D _sprite;
        public ISprite2D Sprite
        {
            get { return _sprite; }
            set
            {
                _sprite = value;
                FromAngle = _sprite.Angle;
                ToAngle = FromAngle + ChangeDegrees;
                //CurrentAngle = FromAngle;
            }
        }

        public void Run()
        {
            if (State == AnimationState.Wait)
            {
                State = AnimationState.Running;
            }
            if (State == AnimationState.Running)
            {
                if (ChangeDegrees > 0)
                {
                    _sprite.Angle += Speed;
                }
                else
                {
                    _sprite.Angle -= Speed;
                }
            }
            bool bend = endAngle();
            if (bend)
            {
                _sprite.Angle = ToAngle;
                State = AnimationState.End;
            }
        }

        private bool endAngle()
        {
            if (ChangeDegrees == 0) return true;
            if (_sprite.Angle == ToAngle) return true;
            if (ChangeDegrees > 0)
            {
                if (_sprite.Angle >= ToAngle)
                {
                    //CurrentAngle = ToPosition.X;
                    return true;
                }
            }
            else if (ChangeDegrees < 0)
            {
                if (_sprite.Angle <= ToAngle)
                {
                    //CurrentAngle = ToPosition.X;
                    return true;
                }
            }
            return false;
        }

       
    }
}
