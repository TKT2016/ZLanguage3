using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZLangRT.Attributes
{
    public class ZInstanceAttribute : ZyyAttribute
    {
        //public bool IsStaticClass { get; private set; }
        public Type SharpType { get; private set; }

        /// <summary>
        /// 是否默认将成员名称作为ZCode
        /// </summary>
        //public bool IsNameAsCode { get; private set; }

        public ZInstanceAttribute( )
        {
            //IsStaticClass = false;
            //IsNameAsCode = false;
        }

        //public ZInstanceAttribute(bool isNameAsCode)
        //{
        //    IsNameAsCode = true;
        //}

        public ZInstanceAttribute(Type forType)
        {
            //IsStaticClass = false;
            SharpType = forType;
            //IsNameAsCode = false;
        }
    }
}
