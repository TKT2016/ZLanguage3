using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZMembers;
using ZLangRT;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileDesc.ZTypes
{
    /// <summary>
    /// Z类库 CLASS类型(成员只有属性和方法，还有构造函数)
    /// </summary>
    public class ZClassLibType : ZClassType //, IWordDictionary
    {
        public override ZClassType BaseZType
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

        public ZClassLibType(Type markType, Type sharpType, bool isStatic)
        {
            MarkType = markType;
            SharpType = sharpType;
            IsStatic = isStatic;
            IsGeneric = SharpType.IsGenericType;
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpType);
        }

        #region ZMembers,ZConstructors,ZMethods

        ZMemberInfo[] _ZMembers;
        public override ZMemberInfo[] ZMembers
        {
            get
            {
                if (_ZMembers == null)
                {
                    _ZMembers = ZClassTypeHelper.GetZMembers(MarkType, SharpType, IsStatic);
                }
                return _ZMembers;
            }
        }

        ZConstructorInfo[] _ZConstructors;
        public override ZConstructorInfo[] ZConstructors
        {
            get
            {
                if (_ZConstructors == null)
                {
                    _ZConstructors = ZClassTypeHelper.GetZConstructors(MarkType, SharpType, IsStatic);
                }
                return _ZConstructors;
            }
        }

        ZMethodInfo[] _ZMethods;
        public override ZMethodInfo[] ZMethods
        {
            get
            {
                if (_ZMethods == null)
                {
                    //if (SharpType.ToString().IndexOf("列表`1[小飞机游戏.子弹]")!=-1)
                    //{
                    //    Console.WriteLine("{Z语言系统.列表`1[小飞机游戏.子弹]");
                    //}
                    _ZMethods = ZClassTypeHelper.GetZMethods(MarkType, SharpType, IsStatic);
                }
                return _ZMethods;
            }
        }

        #endregion

        //#region Words

        //public override bool ContainsWord(string text)
        //{
        //    foreach (var member in this.ZMembers)
        //    {
        //        if (member.ContainsWord(text))
        //            return true;
        //    }
        //    foreach (var zc in this.ZConstructors)
        //    {
        //        if (zc.ContainsWord(text))
        //            return true;
        //    }
        //    foreach (var method in this.ZMethods)
        //    {
        //        if (method.ContainsWord(text))
        //            return true;
        //    }
        //    return false;
        //}

        //public override WordInfo SearchWord(string text)
        //{
        //    if (!ContainsWord(text)) return null;
        //    WordInfo info1 = IWordDictionaryHelper.ArraySearchWord(text, ZMembers);
        //    WordInfo info2 = IWordDictionaryHelper.ArraySearchWord(text, ZConstructors);
        //    WordInfo info3 = IWordDictionaryHelper.ArraySearchWord(text, ZMethods);
        //    WordInfo newWord = WordInfo.Merge(info1, info2, info3);
        //    return newWord;
        //}
        //#endregion

        #region search member,constructor,method
        public override ZMemberInfo SearchZMember(string zname)
        {
            ZClassType temp = this;
            while (temp != null)
            {
                ZMemberInfo zmember = temp.FindDeclaredZMember(zname);
                if (zmember != null)
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

        public override ZMethodInfo[] SearchZMethod(ZCallDesc zpdesc)
        {
            ZClassType temp = this;
            while (temp != null)
            {
                ZMethodInfo[] zmethods = temp.FindDeclaredZMethod(zpdesc);
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

        public override ZMethodInfo[] SearchZMethod(ZMethodDesc zdesc)
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

        public override string ZName
        {
            get
            {
                if (!this.MarkType.IsGenericType)
                    return MarkType.Name;
                else
                    return GenericUtil.GetGenericTypeShortName(this.MarkType);
            }
            protected set { }
        }


        #region find declared member,constructor,method
        public override ZMemberInfo FindDeclaredZMember(string zname)
        {
            foreach (var zmember in ZMembers)
            {
                if (zmember.HasZName(zname))
                {
                    return zmember;
                }
            }
            return null;
        }

        public override ZConstructorInfo FindDeclaredZConstructor(ZNewDesc zcdesc)
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

        public override ZMethodInfo[] FindDeclaredZMethod(ZCallDesc zpdesc)
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

        public override ZMethodInfo[] FindDeclaredZMethod(ZMethodDesc zmdesc)
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

        public override ZMethodInfo FindDeclaredZMethod(string sharpMethodName)
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

        public override string ToString()
        {
            return this.MarkType.Name + "-" + this.SharpType.Name;
        }
    }
}
