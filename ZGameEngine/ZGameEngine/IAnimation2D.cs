using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace ZGameEngine
{
    public interface IAnimation2D
    {
        AnimationState State { get;}
        ISprite2D Sprite { get; set; }
        void Run();
    }
}
