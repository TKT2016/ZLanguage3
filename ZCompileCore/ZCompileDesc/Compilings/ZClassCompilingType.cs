using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZLangRT;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileDesc.Compilings
{
    /// <summary>
    /// Z类型(成员只有属性和方法，还有构造函数)
    /// </summary>
    public class ZClassCompilingType : ZClassType, IZCompilingInfo
    {
        public TypeBuilder ClassBuilder { get; private set; }

        public ZClassCompilingType()
        {
            IsGeneric = false;
            ZCompilingMembers = new Dictionary<string, ZMemberCompiling>();
            ZCompilingConstructors = new List<ZConstructorCompiling>();
            ZCompilingMethods = new List<ZMethodCompiling>();
        }

        public ZClassCompilingType(string name,bool isStatic,ZClassType superClass)
            :this()
        {
            ZName = name;
            this.IsStatic = isStatic;
            BaseZType = superClass;
        }

        public ZClassCompilingType(string zname)
            : this()
        {
            ZName = zname;
        }

        public void SetClassName(string name)
        {
            this.ZName = name;
        }


        public void SetIsStatic(bool isstatic)
        {
            this.IsStatic = isstatic;
        }

        public void SetBaseZType(ZClassType baseZType)
        {
            BaseZType = baseZType;
        }

        public void SetBuilder(TypeBuilder typeBuilder)
        {
            MarkType = typeBuilder;
            SharpType = typeBuilder;
            ClassBuilder = typeBuilder;
            IsGeneric = false;
            AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpType);
        }

        public void SetMemberBuilder(PropertyBuilder propertyBuilder)
        {
            string name = propertyBuilder.Name;
            ZMemberCompiling zcp = ZCompilingMembers[name];
            zcp.SetBuilder(propertyBuilder);
        }

        //public void SetMethodBuilder(ZMethodDesc methodDesc, MethodBuilder methodBuilder)
        //{
        //    string name = propertyBuilder.Name;
        //    ZMemberCompiling zcp = ZCompilingMembers[name];
        //    zcp.SetBuilder(propertyBuilder);
        //}

        public void AddProperty(ZMemberCompiling zPropertyCompilingInfo)
        {
            //Console.WriteLine("ZClassCompilingType " + ZName + " AddProperty " + zPropertyCompilingInfo.Name);
            ZCompilingMembers.Add(zPropertyCompilingInfo.Name, zPropertyCompilingInfo);
        }

        public void AddMethod(ZMethodCompiling zmc)
        {
            //Console.WriteLine("ZClassCompilingType " + ZName + " AddProperty " + zPropertyCompilingInfo.Name);
            ZCompilingMethods.Add(zmc);
        }

        public void AddConstructord(ZConstructorCompiling zmc)
        {
            //Console.WriteLine("ZClassCompilingType " + ZName + " AddProperty " + zPropertyCompilingInfo.Name);
            ZCompilingConstructors.Add(zmc);
        }

        #region ZMembers,ZConstructors,ZMethods
        public Dictionary<string,ZMemberCompiling> ZCompilingMembers { get; private set; }
        public override ZMemberInfo[] ZMembers
        {
            get
            {
                return ZCompilingMembers.Values.ToArray();
            }
        }

        public List<ZConstructorCompiling> ZCompilingConstructors { get; private set; }
        public override ZConstructorInfo[] ZConstructors
        {
            get
            {
                return ZCompilingConstructors.ToArray();
            }
        }

        public List<ZMethodCompiling> ZCompilingMethods { get; private set; }
        public override ZMethodInfo[] ZMethods
        {
            get
            {
                return ZCompilingMethods.ToArray();
            }
        }
         #endregion

        public override ZMethodInfo[] SearchZMethod(ZCallDesc zpdesc)
        {
            foreach(var item in this.ZCompilingMethods)
            {
                if(item.HasZProcDesc(zpdesc))
                {
                    return new ZMethodInfo[]{item};
                }
            }
            return this.BaseZType.SearchZMethod(zpdesc);
        }

        public ZMemberCompiling SeachDefZProperty(string zname)
        {
            return ZCompilingMembers[zname];
        }

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

        public ZMethodCompiling[] SearchThisZMethod(ZMethodDesc zdesc)
        {
            List<ZMethodCompiling> methods = new List<ZMethodCompiling>();
            foreach(var item in this.ZCompilingMethods)
            {
                if(item.HasZProcDesc(zdesc))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZMethodCompiling[] SearchThisZMethod(ZCallDesc zdesc)
        {
            List<ZMethodCompiling> methods = new List<ZMethodCompiling>();
            foreach (var item in this.ZCompilingMethods)
            {
                if (item.HasZProcDesc(zdesc))
                {
                    methods.Add(item);
                }
            }
            return methods.ToArray();
        }

        public ZMethodInfo[] SearchSuperZMethod(ZCallDesc zdesc)
        {
            return this.BaseZType.SearchZMethod(zdesc);  
        }

        #endregion


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
            return this.ZName + "-" + "(ZClassCompilingType)";
        }
    }
}
