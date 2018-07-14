using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ZLangRT.Utils
{
    public static class AttributeUtil
    {
        public static bool HasAttribute<T>(MemberInfo member) where T : Attribute
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(member);
            foreach(var  attr in attrs)
            {
                if(attr is T)
                {
                    return true;
                }
            }
            return false;
        }

        //public static T GetAttribute<T>(Type type) where T : Attribute
        //{
        //    var attr = Attribute.GetCustomAttribute(type, typeof(T));
        //    if (attr == null) return null;
        //    return attr as T;
        //}

        public static T GetAttribute<T>(MemberInfo member) where T : Attribute
        {
            var attr = Attribute.GetCustomAttribute(member, typeof(T));
            if (attr == null) return null;
            return attr as T;
        }

        public static T[] GetAttributes<T>(MemberInfo member) where T : Attribute
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(member, typeof(T));
            if (attrs == null) return null;
            T[] attrs2 = new T[attrs.Length];
            for (int i = 0; i < attrs.Length; i++)
            {
                attrs2[i] = attrs[i] as T;
            }
            return attrs2;
        }
    }
}
