using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
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
            InitZLangBasicTypes();
        }

        internal static void InitZLangBasicTypes()
        {
            if (ZLangBasicTypes.ZOBJECT != null) return;

            ZLangBasicTypes.ZOBJECT = (ZLClassInfo)(CreateZTypeImp(typeof(事物))) ;
            Cache.AddCache(ZLangBasicTypes.ZOBJECT);
            Cache.AddCache(ZLangBasicTypes.ZOBJECT, "object");

            ZLangBasicTypes.ZVOID = (ZLClassInfo)(CreateZTypeImp(typeof(VOID)));// CreateZTypeImp(typeof(VOID)) as ZLClass;
            Cache.AddCache(ZLangBasicTypes.ZVOID);
            Cache.AddCache(ZLangBasicTypes.ZVOID, "void");

            ZLangBasicTypes.ZBOOL = (ZLClassInfo)(CreateZTypeImp(typeof(判断符)));//CreateZTypeImp(typeof(判断符)) as ZLClass;
            Cache.AddCache(ZLangBasicTypes.ZBOOL);
            Cache.AddCache(ZLangBasicTypes.ZBOOL, "bool");

            ZLangBasicTypes.ZINT = (ZLClassInfo)(CreateZTypeImp(typeof(整数)));//CreateZTypeImp(typeof(整数)) as ZLClass;
            Cache.AddCache(ZLangBasicTypes.ZINT);
            Cache.AddCache(ZLangBasicTypes.ZINT, "int");

            ZLangBasicTypes.ZFLOAT = (ZLClassInfo)(CreateZTypeImp(typeof(浮点数)));// CreateZTypeImp(typeof(浮点数)) as ZLClass;
            Cache.AddCache(ZLangBasicTypes.ZFLOAT);
            Cache.AddCache(ZLangBasicTypes.ZFLOAT, "float");

            ZLangBasicTypes.ZSTRING = (ZLClassInfo)(CreateZTypeImp(typeof(文本)));//CreateZTypeImp(typeof(文本)) as ZLClass;
            Cache.AddCache(ZLangBasicTypes.ZSTRING);
            Cache.AddCache(ZLangBasicTypes.ZSTRING, "string");

            ZLangBasicTypes.ZACTION = (ZLClassInfo)(CreateZTypeImp(typeof(可运行语句)));// CreateZTypeImp(typeof(可运行语句)) as ZLClass;
            Cache.AddCache(ZLangBasicTypes.ZACTION);

            ZLangBasicTypes.ZCONDITION = (ZLClassInfo)(CreateZTypeImp(typeof(可运行条件)));// CreateZTypeImp(typeof(可运行条件)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZCONDITION);

            ZLangBasicTypes.ZDATETIME = (ZLClassInfo)(CreateZTypeImp(typeof(时间日期)));//CreateZTypeImp(typeof(时间日期)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZDATETIME);

            ZLangBasicTypes.ZLIST = (ZLClassInfo)(CreateZTypeImp(typeof(列表<>)));//CreateZTypeImp(typeof(列表<>)) as ZClassType;
            Cache.AddCache(ZLangBasicTypes.ZLIST);
        }

        public static ZLClassInfo MakeGenericType(ZLClassInfo genericType, params ZType[] argZTypes)
        {
            ZLClassInfo newzclass = genericType.MakeGenericType(argZTypes);
            Cache.AddCache(newzclass);
            return newzclass;
        }

        public static ZType RegNewGenericType(Type newType)
        {
            ZType ztype = CreateZTypeImp(newType) as ZType;
            return ztype;
        }

        public static ZLType[] GetBySharpName(string sharpName)
        {
            if (Cache.SNameCache.ContainsKey(sharpName)) return new ZLType[] { Cache.SNameCache[sharpName] };
            else return new ZLType[] { };
        }

        public static ZLType[] GetByMarkName(string zname)
        {
            if (Cache.ZNameCache.ContainsKey(zname)) return new ZLType[] { Cache.ZNameCache[zname] };
            else return new ZLType[] { };
        }

        public static ZLType GetBySharpType(Type type)
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
                return (ZLType)newZtype;
            }

            return null;
        }

        public static ZLType GetByMarkType(Type type)
        {
            if (type == null) return null;
            if (Cache.MarkCache.ContainsKey(type)) return Cache.MarkCache[type];
            ZLType descType = CreateZTypeImp(type);
            if(descType!=null)
            {
                Cache.AddCache(descType);
            }
            return descType;
        }

        public static ZLDimInfo CreateZLDimImp(Type type)
        {
            if(AttributeUtil.HasAttribute<ZDimAttribute>(type))
            {
                ZLDimInfo zdim = new ZLDimInfo(type);
                return zdim;
            }
            return null;
        }

        private static ZLType CreateZTypeImp(Type type)
        {
            if (AttributeUtil.HasAttribute<ZInstanceAttribute>(type))
            {
                ZInstanceAttribute zAttr = AttributeUtil.GetAttribute<ZInstanceAttribute>(type);
                Type sharpType = (zAttr.SharpType == null ? type : zAttr.SharpType);
                ZLClassInfo zclass = new ZLClassInfo(type, sharpType, false);
                return zclass;
            }
            else if (AttributeUtil.HasAttribute<ZEnumAttribute>(type))
            {
                ZLEnumInfo zenum = new ZLEnumInfo(type);
                return zenum;
            }
            else if (AttributeUtil.HasAttribute<ZStaticAttribute>(type))
            {
                ZStaticAttribute zAttr = AttributeUtil.GetAttribute<ZStaticAttribute>(type);
                Type sharpType = (zAttr.SharpType == null ? type : zAttr.SharpType);
                ZLClassInfo zclass = new ZLClassInfo(type, sharpType, true);
                return zclass;
            }
           
            return null;
        }

        private class ZTypeCache
        {
            public Dictionary<Type, ZLType> MarkCache = new Dictionary<Type, ZLType>();
            public Dictionary<Type, ZLType> SharpCache = new Dictionary<Type, ZLType>();
            public Dictionary<string, ZLType> ZNameCache = new Dictionary<string, ZLType>();
            public Dictionary<string, ZLType> SNameCache = new Dictionary<string, ZLType>();

            public void AddCache(ZLType descType)
            {
                if (descType == null) return;
                try
                {
                    if (!MarkCache.ContainsKey(descType.MarkType))
                        MarkCache.Add(descType.MarkType, descType);
                    if (!SharpCache.ContainsKey(descType.SharpType))
                        SharpCache.Add(descType.SharpType, descType);
                    if (!ZNameCache.ContainsKey(descType.ZTypeName))
                        ZNameCache.Add(descType.ZTypeName, descType);
                    if (!SNameCache.ContainsKey(descType.SharpType.Name))
                        SNameCache.Add(descType.SharpType.Name, descType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ZTypeManager.Cache.AddCache(IBLibType):" + descType +":"+ ex.Message);
                }
            }

            public void AddCache(ZLType descType,string sname)
            {
                try
                {
                    if (!MarkCache.ContainsKey(descType.MarkType))
                        MarkCache.Add(descType.MarkType, descType);
                    if (!SharpCache.ContainsKey(descType.SharpType))
                        SharpCache.Add(descType.SharpType, descType);
                    if (!ZNameCache.ContainsKey(descType.ZTypeName))
                        ZNameCache.Add(descType.ZTypeName, descType);
                    if (!SNameCache.ContainsKey(sname))
                        SNameCache.Add(sname, descType);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ZTypeManager.Cache.AddCache(IBLibType):" + sname + ":" + ex.Message);
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

            public ZLType Get(Type type)
            {
                if (MarkCache.ContainsKey(type))
                    return MarkCache[type];
                else if (SharpCache.ContainsKey(type))
                    return SharpCache[type];
                else
                    return null;
            }

            public ZLType Get(string typeName)
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
