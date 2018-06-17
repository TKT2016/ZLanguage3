using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ZLangRT
{
    public static class Invoker
    {
        public static object Call(object obj, string funcName, params object[] args)
        {
            object[] newArgs = args;
            object result = null;

            if (obj is Type)
            {
                //debug("is Type " + obj.ToString());
                Type type = (Type)obj;
                MethodInfo func = GetMethodInfo(type, funcName, newArgs);
                result =  func.Invoke(null, newArgs);
            }
            else
            {
                //debug("not is Type " + obj.ToString());
                Type type = obj.GetType();
                MethodInfo func = GetMethodInfo(type, funcName, newArgs);
                result = func.Invoke(obj, newArgs);
            }
            //debug(" **** call result= " + result.ToString());
            return result;
        }

        public static MethodInfo GetMethodInfo(Type type, string funcName, params object[] args)
        {
            List<Type> types = new List<Type>();
            foreach (object arg in args)
            {
                types.Add(arg.GetType());
                //debug("arg = "+arg.ToString()+", type = " +arg.GetType().FullName);
            }

            MethodInfo func = type.GetMethod(funcName, types.ToArray());
            if (func == null)
            {
                throw new ZyyRTException(string.Format("没有找到类型{0}的方法{1}({2})", type.FullName, funcName, string.Join(",", types.Select(p => p.FullName))));
            }

            return func;
        }

        public static object GetValue(object obj, string memberName)
        {
            Type type = null;
            if (obj is Type)
            {
                type = (Type)obj;
            }
            else
            {
                type = obj.GetType();
            }

            var property = type.GetProperty(memberName);
            if (property != null)
            {
                return property.GetValue(obj is Type ? null : obj,null);
            }

            var field = type.GetField(memberName);
            if (field != null)
            {
                return field.GetValue(obj is Type ? null : obj);
            }

            throw new ZyyRTException("找不到类型" + type.FullName + "的" + memberName + "成员,无法取值");
        }
        
    }
}
