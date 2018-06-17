using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Attributes;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    internal static class ZClassUtil
    {
        public static FieldInfo[] GetEnumItems(Type type)
        {
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields;
        }

        public static ZLParamInfo[] GetZLParams(ZLMethodInfo zmethod)
        {
            var markMethod = zmethod.MarkMethod;
            var sharpMethod = zmethod.SharpMethod;
            
            List<ZLParamInfo> list = new List<ZLParamInfo>();
            ParameterInfo[] parameters = sharpMethod.GetParameters();
            int index = 0;
            foreach (ParameterInfo peramter in parameters)
            {
                ZLParamInfo zlparam = new ZLParamInfo(peramter, zmethod, index);//, false);
                list.Add(zlparam);
                index++;
            }
            return list.ToArray();
        }

        public static ZLMethodDesc[] GetProcDescs(ZLMethodInfo zmethod)
        {
            var markMethod = zmethod.MarkMethod;
            var sharpMethod = zmethod.SharpMethod;
            List<ZLMethodDesc> list = new List<ZLMethodDesc>();
            ZCodeAttribute[] attrs = AttributeUtil.GetAttributes<ZCodeAttribute>(markMethod);
            foreach (ZCodeAttribute attr in attrs)
            {
                ZCodeParser parser = new ZCodeParser(sharpMethod.DeclaringType, zmethod);
                ZLMethodDesc typeProcDesc = parser.Parser(attr.Code);
                list.Add(typeProcDesc);
            }
            return list.ToArray();
        }

        public static AccessAttrEnum GetAccessAttributeEnum(MethodBase methodBase)
        {
            if (methodBase == null) return AccessAttrEnum.Private;
            if (methodBase.IsPublic)
            {
                return AccessAttrEnum.Public;
            }
            else if (methodBase.IsPrivate)
            {
                return AccessAttrEnum.Private;
            }
            else if (methodBase.IsFamily)
            {
                return AccessAttrEnum.Internal;
            }
            else if (methodBase.IsFamilyOrAssembly)
            {
                return AccessAttrEnum.Protected;
            }
            else
            {
                return AccessAttrEnum.Private;
            }
        }

        public static ZLConstructorDesc CreateZConstructorDesc(ConstructorInfo ci, ZLConstructorInfo zlc, ZLClassInfo zclass)
        {
            ZLBracketDesc zbracket = new ZLBracketDesc();
            //List<ZArgDefNormalDesc> args = new List<ZArgDefNormalDesc>();
            int index = 0;
            foreach (ParameterInfo param in ci.GetParameters())
            {
                ZLParamInfo arg = new ZLParamInfo(param, zlc, index);// , false);
                zbracket.Add(arg);
                index++;
            }
            ZLConstructorDesc desc = new ZLConstructorDesc(zlc,zbracket);
            return desc;
        }

        public static string[] GetZNames(MemberInfo element)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(element);
            List<string> _znames = new List<string>();
            foreach (var attr in attrs)
            {
                if (attr is ZCodeAttribute)
                {
                    ZCodeAttribute zcodeAttr = (attr as ZCodeAttribute);
                    _znames.Add(zcodeAttr.Code);
                }
            }
            return _znames.ToArray();
        }

        public static ZLFieldInfo[] GetZFields(Type markType, Type sharpType, bool isStatic, ZLClassInfo zclass)
        {
            List<ZLFieldInfo> list = new List<ZLFieldInfo>();

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
                            ZLFieldInfo zproperty = new ZLFieldInfo(zclass,field, newField);
                            list.Add(zproperty);
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static ZLPropertyInfo[] GetZPropertys(Type markType, Type sharpType, bool isStatic, ZLClassInfo zclass)
        {
            List<ZLPropertyInfo> list = new List<ZLPropertyInfo>();
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
                                ZLPropertyInfo zproperty = new ZLPropertyInfo(zclass,property, newPropertyInfo);
                                list.Add(zproperty);
                            }
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public static ZLConstructorInfo[] GetZConstructors(Type markType, Type sharpType, bool isStatic, ZLClassInfo zclass)
        {
            List<ZLConstructorInfo> list = new List<ZLConstructorInfo>();
            if (!isStatic)
            {
                ConstructorInfo[] constructors = sharpType.GetConstructors();
                foreach (ConstructorInfo constructor in constructors)
                {
                    ZLConstructorInfo zconstructor = new ZLConstructorInfo(constructor, zclass);
                    list.Add(zconstructor);
                }
            }
            return list.ToArray();
        }


        public static ZLMethodInfo[] GetZMethods(Type markType, Type sharpType, bool isStatic,ZLClassInfo zclass)
        {
            //if (markType.Name == "控制台")
            //{
            //    Console.WriteLine("控制台");
            //}
            List<ZLMethodInfo> list = new List<ZLMethodInfo>();
            MethodInfo[] markMethods = markType.GetMethods();
            foreach (MethodInfo method in markMethods)
            {
                //if (method.Name == "Write")
                //{
                //    Console.WriteLine("Write");
                //}
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
                        ZLMethodInfo zmethod = new ZLMethodInfo(method, newMethod, zclass);
                        list.Add(zmethod);
                    }
                }
            }
            return list.ToArray();
        }
    }
}
