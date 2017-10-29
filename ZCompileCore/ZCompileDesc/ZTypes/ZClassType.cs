using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZMembers;
using ZLangRT;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileDesc.ZTypes
{
    /// <summary>
    /// Z类型(成员只有属性和方法，还有构造函数)
    /// </summary>
    public class ZClassType : ZType, IWordDictionary
    {
        public bool IsStatic { get; protected set; }
        public bool IsGeneric { get; protected set; }
        public ZClassType BaseZType
        {
            get
            {
                if (!IsStatic && SharpType != typeof(object))
                {
                    return ZTypeManager.GetBySharpType(SharpType.BaseType) as ZClassType;
                }
                else
                {
                    return null;
                }
            }
        }

        #region ZMembers,ZConstructors,ZMethods
        ZMemberInfo[] _ZMembers;
        public ZMemberInfo[] ZMembers
        {
            get
            {
                if (_ZMembers == null)
                {
                    _ZMembers = GetZMembers(MarkType, SharpType, IsStatic);
                }
                return _ZMembers;
            }
        }
        ZConstructorInfo[] _ZConstructors;
        public ZConstructorInfo[] ZConstructors
        {
            get
            {
                if (_ZConstructors == null)
                {
                    _ZConstructors  = GetZConstructors(MarkType, SharpType, IsStatic);
                }
                return _ZConstructors;
            }
        }
        ZMethodInfo[] _ZMethods;
        public ZMethodInfo[] ZMethods
        {
            get
            {
                if (_ZMethods == null)
                {
                    _ZMethods = GetZMethods(MarkType, SharpType, IsStatic);
                }
                return _ZMethods;
            }
        }
        #endregion

        public ZClassType(Type markType, Type sharpType, bool isStatic)
        {
            MarkType = markType;
            SharpType = sharpType;
            IsStatic = isStatic;
            Init();
        }
          
        protected void Init()
        {
            IsGeneric = SharpType.IsGenericType;
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpType);
        }

        public override bool ContainsWord(string text)
        {
            foreach (var member in this.ZMembers)
            {
                if (member.ContainsWord(text))
                    return true;
            }
            //if (this.SharpType == typeof(补语控制))
            //{
            //    Console.WriteLine("补语控制");
            //}

            foreach (var zc in this.ZConstructors)
            {
                if (zc.ContainsWord(text))
                    return true;
            }
            foreach (var method in this.ZMethods)
            {
                if (method.ContainsWord(text))
                    return true;
            }
            return false;
        }

        public override WordInfo SearchWord(string text)
        {
            if (!ContainsWord(text)) return null;
            WordInfo info1 = IWordDictionaryHelper.ArraySearchWord(text, ZMembers);
            WordInfo info2 = IWordDictionaryHelper.ArraySearchWord(text, ZConstructors);
            WordInfo info3 = IWordDictionaryHelper.ArraySearchWord(text, ZMethods);
            WordInfo newWord =  WordInfo.Merge(info1, info2,info3);
            return newWord;
        }

        #region get method , constructor , field , property

        protected static ZConstructorInfo[] GetZConstructors(Type markType, Type sharpType, bool isStatic)
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

        protected static ZMethodInfo[] GetZMethods(Type markType, Type sharpType, bool isStatic)
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

            //foreach (MethodInfo method in markMethods)
            //{
            //    if (ReflectionUtil.IsDeclare(markType, method))
            //    {
            //        if (AttributeUtil.HasAttribute<ZCodeAttribute>(method))
            //        {
            //            MethodInfo newMethod = sharpType.GetMethod(method.Name);
            //            if (newMethod == null)
            //            {
            //                throw new ZyyRTException();
            //            }
            //            else
            //            {
            //                if (isStatic == newMethod.IsStatic)
            //                {
            //                    ZMethodInfo zproperty = new ZMethodInfo(method, newMethod);
            //                    list.Add(zproperty);
            //                }
            //            }
            //        }
            //    }
            //}
            return list.ToArray();
        }

        protected static ZFieldInfo[] GetZFields(Type markType, Type sharpType, bool isStatic)
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

        protected static ZPropertyInfo[] GetZPropertys(Type markType, Type sharpType, bool isStatic)
        {
            List<ZPropertyInfo> list = new List<ZPropertyInfo>();
            PropertyInfo[] propertyArray = markType.GetProperties();
            foreach (var property in propertyArray)
            {
                if (ReflectionUtil.IsDeclare(markType, property)  )
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

        protected static ZMemberInfo[] GetZMembers(Type markType, Type sharpType, bool isStatic)
        {
            List<ZMemberInfo> list = new List<ZMemberInfo>();
            list.AddRange(GetZFields(markType, sharpType, isStatic));
            list.AddRange(GetZPropertys(markType, sharpType, isStatic));
            return list.ToArray();
        }

        #endregion

        public override string ZName
        {
            get {
                if (!this.MarkType.IsGenericType)
                    return MarkType.Name; 
                else
                    return GenericUtil.GetGenericTypeShortName(this.MarkType); 
            }
        }

        #region find declared member,constructor,method
        public ZMemberInfo FindDeclaredZMember(string zname)
        {
            foreach(var zmember in ZMembers)
            {
                if(zmember.HasZName(zname))
                {
                    return zmember;
                }
            }
            return null;
        }

        public ZConstructorInfo FindDeclaredZConstructor(ZNewDesc zcdesc)
        {
            foreach (var item in this.ZConstructors)
            {
                if (item.HasZConstructorDesc(zcdesc))
                {
                    return item;
                }
            }
            return null;
        }

        public ZMethodInfo[] FindDeclaredZMethod(ZCallDesc zpdesc)
        {
            List<ZMethodInfo> methods = new List<ZMethodInfo>();
            foreach (var item in this.ZMethods)
            {
                if (item.HasZProcDesc(zpdesc))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZMethodInfo[] FindDeclaredZMethod(ZMethodDesc zmdesc)
        {
            List<ZMethodInfo> methods = new List<ZMethodInfo>();
            foreach (var item in this.ZMethods)
            {
                if (item.HasZProcDesc(zmdesc))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZMethodInfo FindDeclaredZMethod(string sharpMethodName)
        {
            foreach (var item in this.ZMethods)
            {
                if (item.SharpMethod.Name == sharpMethodName)
                {
                    return item;
                }
            }
            return null;
        }

        #endregion


        #region search member,constructor,method
        public ZMemberInfo SearchZMember(string zname)
        {
            ZClassType temp = this;
            while(temp!=null)
            {
                ZMemberInfo zmember = temp.FindDeclaredZMember(zname);
                if(zmember!=null)
                {
                    return zmember;
                }
                else
                {
                    temp = temp.BaseZType;
                }
            }
            return null;
        }

        public ZMethodInfo[] SearchZMethod(ZCallDesc zpdesc)
        {
            ZClassType temp = this;
            while (temp != null)
            {
                ZMethodInfo[] zmethods = temp.FindDeclaredZMethod(zpdesc);
                if (zmethods.Length>0)
                {
                    return zmethods;
                }
                else
                {
                    temp = temp.BaseZType;
                }
            }
            return new ZMethodInfo[]{};
        }

        public ZMethodInfo[] SearchZMethod(ZMethodDesc zdesc)
        {
            ZClassType temp = this;
            while (temp != null)
            {
                ZMethodInfo[] zmethods = temp.FindDeclaredZMethod(zdesc);
                if (zmethods.Length > 0)
                {
                    return zmethods;
                }
                else
                {
                    temp = temp.BaseZType;
                }
            }
            return new ZMethodInfo[] { };
        }

        #endregion

        public override string ToString()
        {
            return this.MarkType.Name + "-" + this.SharpType.Name;
        }
    }
}
