using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLangRT.Attributes
{
    public class ZStaticAttribute : ZyyAttribute// ZClassAttribute, IZTag
    {
        //public bool IsStaticClass { get; private set; }
        public Type SharpType { get; private set; }

        public ZStaticAttribute( )
        {
            //IsStaticClass = true;
        }

        public ZStaticAttribute(Type forType)
        {
            //IsStaticClass = true;
            SharpType = forType;
        }
    }
}
