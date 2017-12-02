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
    /// Z类型(成员只有属性和方法，还有构造函数)
    /// </summary>
    public abstract class ZClassType : ZType //, IWordDictionary
    {
        public ZEnumAttribute MarkAttribute { get; protected set; }
        //public ZEnumItemInfo[] EnumElements { get; protected set; }
        public Type MarkType { get; protected set; }
        public Type SharpType { get; protected set; }
        public AccessAttributeEnum AccessAttribute { get; protected set; }
        public virtual bool IsMarkSelf { get { return MarkType == SharpType; } }
        public bool IsStatic { get; protected set; }
        public bool IsGeneric { get; protected set; }
        public virtual ZClassType BaseZType { get; set; }
        public virtual string ZName { get; protected set; }

        public abstract ZMemberInfo[] ZMembers { get; }
        public abstract ZConstructorInfo[] ZConstructors { get; }
        public abstract ZMethodInfo[] ZMethods { get; }

        public abstract ZMemberInfo FindDeclaredZMember(string zname);
        public abstract ZConstructorInfo FindDeclaredZConstructor(ZNewDesc zcdesc);
        public abstract ZMethodInfo[] FindDeclaredZMethod(ZCallDesc zpdesc);
        public abstract ZMethodInfo[] FindDeclaredZMethod(ZMethodDesc zmdesc);
        public abstract ZMethodInfo FindDeclaredZMethod(string sharpMethodName);

        //public abstract bool ContainsWord(string text);
        //public abstract WordInfo SearchWord(string text);
        public abstract ZMemberInfo SearchZMember(string zname);
        public abstract ZMethodInfo[] SearchZMethod(ZCallDesc zpdesc);
        public abstract ZMethodInfo[] SearchZMethod(ZMethodDesc zdesc);
    }
}
