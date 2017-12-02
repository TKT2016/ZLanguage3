using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.ZMembers;
using ZLangRT;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.Utils
{
    public static class ZClassTypeHelper
    {

        #region get method , constructor , field , property

        public static ZConstructorInfo[] GetZConstructors(Type markType, Type sharpType, bool isStatic)
        {
            List<ZConstructorInfo> list = new List<ZConstructorInfo>();
            if (!isStatic)
            {
                ConstructorInfo[] constructors = sharpType.GetConstructors();
                foreach (ConstructorInfo constructor in constructors)
                {
                    ZConstructorInfo zconstructor = new ZConstructorInfo(constructor);
                    list.Add(zconstructor);
                }
            }
            return list.ToArray();
        }

        public static ZMethodInfo[] GetZMethods(Type markType, Type sharpType, bool isStatic)
        {
            List<ZMethodInfo> list = new List<ZMethodInfo>();
            MethodInfo[] markMethods = markType.GetMethods();
            foreach (MethodInfo method in markMethods)
            {
                if (!ReflectionUtil.IsDeclare(markType, method)) continue;
                if (!AttributeUtil.HasAttribute<ZCodeAttribute>(method)) continue;
                MethodInfo newMethod = ReflectionUtil.GetMethod(sharpType, method);
                if (newMethod == null)
                {
                    throw new ZyyRTException();
                }
                else
                {
                    if (isStatic == newMethod.IsStatic)
                    {
                        ZMethodInfo zproperty = new ZMethodInfo(method, newMethod);
                        list.Add(zproperty);
                    }
                }
            }
            return list.ToArray();
        }

        public static ZFieldInfo[] GetZFields(Type markType, Type sharpType, bool isStatic)
        {
            List<ZFieldInfo> list = new List<ZFieldInfo>();

            FieldInfo[] fields = markType.GetFields();
            foreach (var field in fields)
            {
                if (ReflectionUtil.IsDeclare(markType, field) && AttributeUtil.HasAttribute<ZCodeAttribute>(field))
                {
                    FieldInfo newField = sharpType.GetField(field.Name);
                    if (newField == null)
                    {
                        throw new ZyyRTException();
                    }
                    else
                    {
                        if (isStatic == newField.IsStatic)
                        {
                            ZFieldInfo zproperty = new ZFieldInfo(field, newField);
                            list.Add(zproperty);
                        }

                    }
                }
            }
            return list.ToArray();
        }

        public static ZPropertyInfo[] GetZPropertys(Type markType, Type sharpType, bool isStatic)
        {
            List<ZPropertyInfo> list = new List<ZPropertyInfo>();
            PropertyInfo[] propertyArray = markType.GetProperties();
            foreach (var property in propertyArray)
            {
                if (ReflectionUtil.IsDeclare(markType, property))
                {
                    if (AttributeUtil.HasAttribute<ZCodeAttribute>(property))
                    {
                        PropertyInfo newPropertyInfo = sharpType.GetProperty(property.Name);
                        if (newPropertyInfo == null)
                        {
                            throw new ZyyRTException();
                        }
                        else
                        {
                            if (isStatic == ReflectionUtil.IsStatic(newPropertyInfo))
                            {
                                ZPropertyInfo zproperty = new ZPropertyInfo(property, newPropertyInfo);
                                list.Add(zproperty);
                            }
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static ZMemberInfo[] GetZMembers(Type markType, Type sharpType, bool isStatic)
        {
            List<ZMemberInfo> list = new List<ZMemberInfo>();
            list.AddRange(GetZFields(markType, sharpType, isStatic));
            list.AddRange(GetZPropertys(markType, sharpType, isStatic));
            return list.ToArray();
        }

        #endregion
    }
}
