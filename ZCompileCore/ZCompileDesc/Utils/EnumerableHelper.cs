using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileDesc.Utils
{
    public static class EnumerableHelper
    {
        public static T SearchOne<T>(IEnumerable<T> data, Func<T, bool> fn)
        {
            foreach (var item in data)
            {
                if (fn(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        public static bool AnyOne<T>(IEnumerable<T> data,Func<T,bool> fn)
        {
            foreach(var item in data)
            {
                if(fn(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
