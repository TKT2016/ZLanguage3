using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileDesc
{
    public static class ZTypeManager
    {
        static ZTypeCache Cache = new ZTypeCache();

        static ZTypeManager()
        {
            ZLangBasicTypes.ZOBJECT = CreateZTypeImp(typeof(事物)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZOBJECT);
            Cache.AddCache(ZLangBasicTypes.ZOBJECT, "object");

            ZLangBasicTypes.ZVOID = CreateZTypeImp(typeof(VOID)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZVOID);
            Cache.AddCache(ZLangBasicTypes.ZVOID, "void");

            ZLangBasicTypes.ZBOOL = CreateZTypeImp(typeof(判断符)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZBOOL);
            Cache.AddCache(ZLangBasicTypes.ZBOOL, "bool");

            ZLangBasicTypes.ZINT = CreateZTypeImp(typeof(整数)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZINT);
            Cache.AddCache(ZLangBasicTypes.ZINT, "int");

            ZLangBasicTypes.ZFLOAT = CreateZTypeImp(typeof(浮点数)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZFLOAT);
            Cache.AddCache(ZLangBasicTypes.ZFLOAT, "float");

            ZLangBasicTypes.ZSTRING = CreateZTypeImp(typeof(文本)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZSTRING);
            Cache.AddCache(ZLangBasicTypes.ZSTRING, "string");

            ZLangBasicTypes.ZACTION = CreateZTypeImp(typeof(可运行语句)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZACTION);

            ZLangBasicTypes.ZCONDITION = CreateZTypeImp(typeof(可运行条件)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZCONDITION);

            ZLangBasicTypes.ZDATETIME = CreateZTypeImp(typeof(时间日期)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZDATETIME);

            ZLangBasicTypes.ZLIST = CreateZTypeImp(typeof(列表<>)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZLIST);
        }
      
        public static ZType RegNewGenericType(Type newType)
        {
            ZType ztype = CreateZTypeImp(newType) as ZType;
            return ztype;
        }

        public static IZDescType[] GetBySharpName(string sharpName)
        {
            if (Cache.SNameCache.ContainsKey(sharpName)) return new IZDescType[] { Cache.SNameCache[sharpName] };
            else return new IZDescType[] { };
        }

        public static IZDescType[] GetByMarkName(string zname)
        {
            if (Cache.ZNameCache.ContainsKey(zname)) return new IZDescType[] { Cache.ZNameCache[zname] };
            else return new IZDescType[] { };
        }

        public static IZDescType GetBySharpType(Type type)
        {
            if (type == null) return null;
            else if (Cache.SharpCache.ContainsKey(type))
            {
                return Cache.SharpCache[type];
            }
            else if (type.IsGenericType)
            {
                if(type.IsGenericTypeDefinition)
                {
                    return null;
                }
                /* 检查类型的泛型类型是否存在 */
                Type typeParentGeneric = type.GetGenericTypeDefinition();
                if (!Cache.SharpCache.ContainsKey(typeParentGeneric))
                {
                    return null;
                }
                /* 检查类型的参数类型是否存在 */
                Type[] typeArguments = type.GetGenericArguments();
                foreach (Type tParam in typeArguments)
                {
                    if (!Cache.SharpCache.ContainsKey(tParam))
                    {
                        return null;
                    }
                }
                ZType newZtype = ZTypeManager.RegNewGenericType(type);
                return newZtype;
            }

            return null;
        }

        public static IZDescType GetByMarkType(Type type)
        {
            if (type == null) return null;
            if (Cache.MarkCache.ContainsKey(type)) return Cache.MarkCache[type];
            IZDescType descType = CreateZTypeImp(type);
            if(descType!=null)
            {
                Cache.AddCache(descType);
            }
            return descType;
        }

        private static IZDescType CreateZTypeImp(Type type)
        {
            if (AttributeUtil.HasAttribute<ZInstanceAttribute>(type))
            {
                ZInstanceAttribute zAttr = AttributeUtil.GetAttribute<ZInstanceAttribute>(type);
                Type sharpType = (zAttr.SharpType == null ? type : zAttr.SharpType);
                ZClassLibType zclass = new ZClassLibType(type, sharpType, false);
                return zclass;
            }
            else if(AttributeUtil.HasAttribute<ZDimAttribute>(type))
            {
                ZDimType zdim = new ZDimType(type);
                return zdim;
            }
            else if (AttributeUtil.HasAttribute<ZEnumAttribute>(type))
            {
                ZEnumType zenum = new ZEnumType(type);
                return zenum;
            }
            else if (AttributeUtil.HasAttribute<ZStaticAttribute>(type))
            {
                ZStaticAttribute zAttr = AttributeUtil.GetAttribute<ZStaticAttribute>(type);
                Type sharpType = (zAttr.SharpType == null ? type : zAttr.SharpType);
                ZClassLibType zclass = new ZClassLibType(type, sharpType, true);
                return zclass;
            }
           
            return null;
        }

        private class ZTypeCache
        {
            public Dictionary<Type, IZDescType> MarkCache = new Dictionary<Type, IZDescType>();
            public Dictionary<Type, IZDescType> SharpCache = new Dictionary<Type, IZDescType>();
            public Dictionary<string, IZDescType> ZNameCache = new Dictionary<string, IZDescType>();
            public Dictionary<string, IZDescType> SNameCache = new Dictionary<string, IZDescType>();

            public void AddCache(IZDescType descType)
            {
                try
                {
                    if (!MarkCache.ContainsKey(descType.MarkType))
                        MarkCache.Add(descType.MarkType, descType);
                    if (!SharpCache.ContainsKey(descType.SharpType))
                        SharpCache.Add(descType.SharpType, descType);
                    if (!ZNameCache.ContainsKey(descType.ZName))
                        ZNameCache.Add(descType.ZName, descType);
                    if (!SNameCache.ContainsKey(descType.SharpType.Name))
                        SNameCache.Add(descType.SharpType.Name, descType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ZTypeManager.Cache.AddCache(IZDescType):" + descType +":"+ ex.Message);
                }
            }

            public void AddCache(IZDescType descType,string sname)
            {
                try
                {
                    if (!MarkCache.ContainsKey(descType.MarkType))
                        MarkCache.Add(descType.MarkType, descType);
                    if (!SharpCache.ContainsKey(descType.SharpType))
                        SharpCache.Add(descType.SharpType, descType);
                    if (!ZNameCache.ContainsKey(descType.ZName))
                        ZNameCache.Add(descType.ZName, descType);
                    if (!SNameCache.ContainsKey(sname))
                        SNameCache.Add(sname, descType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ZTypeManager.Cache.AddCache(IZDescType):" + sname + ":" + ex.Message);
                }
            }

            public bool Contains(Type type)
            {
                return MarkCache.ContainsKey(type) || SharpCache.ContainsKey(type);
            }

            public bool Contains(string typeName)
            {
                return ZNameCache.ContainsKey(typeName) || SNameCache.ContainsKey(typeName);
            }

            public IZDescType Get(Type type)
            {
                if (MarkCache.ContainsKey(type))
                    return MarkCache[type];
                else if (SharpCache.ContainsKey(type))
                    return SharpCache[type];
                else
                    return null;
            }

            public IZDescType Get(string typeName)
            {
                if (ZNameCache.ContainsKey(typeName))
                    return ZNameCache[typeName];
                else if (SNameCache.ContainsKey(typeName))
                    return SNameCache[typeName];
                else
                    return null;
            }

            public void Clear()
            {
                MarkCache.Clear();
                SharpCache.Clear();
                ZNameCache.Clear();
                SNameCache.Clear();
            }
        }
    }
}
