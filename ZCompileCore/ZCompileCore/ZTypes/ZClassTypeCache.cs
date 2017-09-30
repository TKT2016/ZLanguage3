using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.ZTypes
{
    public class ZClassTypeCache
    {
        Dictionary<Type, ZClassType> cache = new Dictionary<Type, ZClassType>();
        
        private ZClassTypeCache()
        {

        }

        static ZClassTypeCache()
        {
            One = new ZClassTypeCache();
        }

        public static ZClassTypeCache One { get;private set; }

        public void Clear()
        {
            cache.Clear();
        }

        public ZClassType Get(Type type)
        {
            if (cache.ContainsKey(type)) return cache[type];
            return null;
        }

        public bool Set(Type type,ZClassType zcType)
        {
            if (cache.ContainsKey(type)) return false;
            cache.Add(type, zcType);
            return true;
        }
    }
}
