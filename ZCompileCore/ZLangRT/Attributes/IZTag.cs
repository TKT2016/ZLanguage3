using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLangRT.Attributes
{
    public interface IZTag
    {
        bool IsStaticClass{ get;}
        Type SharpType { get;}
    }
}
