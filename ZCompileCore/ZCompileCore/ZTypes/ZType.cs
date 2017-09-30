using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Utils;
using ZLangRT;
using ZLangRT.Tags;
using ZLangRT.Utils;

namespace ZCompileCore.ZTypes
{
    public abstract class ZType : IZType
    {
        public Type SharpType { get; protected set; }
        
        public abstract string ZyyName { get; }

        public static ZType CreateZType(Type type)
        {
            ZClassAttribute zAttr = AttributeUtil.GetAttribute<ZClassAttribute>(type);
            if (zAttr!=null)
            {
                if(type.IsEnum)
                {
                    return new ZEnumGenType(type);
                }
                else
                {
                    return new ZClassGenType(type);
                }
            }
            else
            {
                ZMappingAttribute mAttr = AttributeUtil.GetAttribute<ZMappingAttribute>(type);
                if (mAttr == null) return null;
                if (type.IsEnum)
                {
                    return new ZEnumMappingType(type);
                }
                else
                {
                    return new ZEnumGenType(type);
                }
            }
            //return null;
        }

        //public static ZClassType CreateClassType(Type type)
        //{
        //    ZClassAttribute cattr = AttributeUtil.GetAttribute<ZClassAttribute>(type);
        //    if (cattr != null)
        //    {
        //        return new ZClassGenType(type);
        //    }
        //    ZMappingAttribute mattr = AttributeUtil.GetAttribute<ZMappingAttribute>(type);
        //    if (cattr != null)
        //    {
        //        return new ZClassMappingType(type);
        //    }
        //    return null;
        //}
    }
}
