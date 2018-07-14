using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLangRT.Attributes
{
    public sealed class ZEnumAttribute : ZyyAttribute //: ZClassAttribute, IZTag
    {
        public Type ForType { get; private set; }

        public ZEnumAttribute()
        {

        }

        public ZEnumAttribute(Type forType)
        {
            ForType = forType;
        }

        ////public bool IsStaticClass { get; private set; }
        ////public Type SharpType { get; private set; }
        //public Type ForType { get; private set; }
        //public ZEnumAttribute( )
        //{
        //    //IsStaticClass = true;
        //}

        //public ZEnumAttribute(Type forType)
        //{
        //    ForType = forType;
        //    //IsStaticClass = true;
        //    //SharpType = forType;
        //}
    }
}
