using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Utils;
using ZLangRT.Tags;
using ZLangRT.Descs;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileCore.ZTypes
{
    public class ZClassMappingType : ZClassType
    {
        ProcDescCodeParser parser = new ProcDescCodeParser();
        ZMappingAttribute MappingAttribute;
        Type ZMappingType { get; set; }

        public ZClassMappingType(Type type)
        {
            this.ZMappingType = type;
            this.MappingAttribute = AttributeUtil.GetAttribute<ZMappingAttribute>(type);
            this.SharpType = MappingAttribute.ForType;

            Type baseType = MappingAttribute.BaseMappingType != null ? MappingAttribute.BaseMappingType : typeof(Z语言系统.事物);
            ZClassType zc = ZType.CreateZType(baseType) as ZClassType;
            ZClassTypeCache.One.Set(baseType, zc);
            this.ParentMapping = ZClassTypeCache.One.Get(baseType);
        }

        public override string ZyyName
        {
            get
            {
                return this.ZMappingType.Name;
            }
        }

        //public override ZType CreateNewFor(Type forType)
        //{
        //    MappingZType gcl = new MappingZType(ZyyType, forType);
        //    return gcl;
        //}

        bool isExFieldInfoByAttr(string name, MemberInfo member)
        {
            Attribute[] attrs = Attribute.GetCustomAttributes(member, typeof(ZCodeAttribute));
            if (attrs.Length == 0)
            {
                if (member.Name == name)
                {
                    return true;
                }
            }
            else
            {
                foreach (Attribute attr in attrs)
                {
                    ZCodeAttribute zCodeAttribute = attr as ZCodeAttribute;
                    if (zCodeAttribute.Code == name)
                    {
                        return true;
                    }
                }
            }
            return false; 
        }

        public override ExPropertyInfo[] GetPropertyInfoes()
        {
            List<ExPropertyInfo> exList = new List<ExPropertyInfo>();
            var propertyArray = this.SharpType.GetProperties();
            foreach (var property in propertyArray)
            {
                ZCodeAttribute[] arrs = AttributeUtil.GetAttributes<ZCodeAttribute>(property);
                foreach (var zcattr in arrs)
                {
                    ExPropertyInfo exPI = new ExPropertyInfo(property, true, zcattr.Code);
                    exList.Add(exPI);
                }
            }
            if (ParentMapping != null && !isRoot())
            {
                ExPropertyInfo[] pArr = ParentMapping.GetPropertyInfoes();
                foreach (ExPropertyInfo pitem in pArr)
                {
                    pitem.IsSelf = false;
                }
                exList.AddRange(pArr);
            }
            return exList.ToArray();
        }

        public override ExPropertyInfo SearchExProperty(string name)
        {
            var propertyArray = this.SharpType.GetProperties();
            foreach (var property in propertyArray)
            {
                if (!ReflectionUtil.IsDeclare(this.SharpType, property)) continue;
                /* 映射类可能有多个同义的属性名称对应同一个实际属性 */
                ZCodeAttribute[] arrs = AttributeUtil.GetAttributes<ZCodeAttribute>(property);
                foreach(var zcode in arrs)
                {
                    if(zcode.Code==name)
                    {
                        return new ExPropertyInfo(property, true, name);
                    }
                }
            }
            if (ParentMapping != null&& !isRoot())
            {
                ExPropertyInfo epi = ParentMapping.SearchExProperty(name);
                if (epi != null)
                {
                    epi.IsSelf = false;
                    return epi;
                }
            }
            return null;
        }

        public override TKTConstructorDesc[] GetConstructors()
        {
            List<TKTConstructorDesc> list = new List<TKTConstructorDesc>();
            //TKTConstructorDesc bracket2 = desc;
            ConstructorInfo[] constructorInfoArray = this.SharpType.GetConstructors();
            foreach (ConstructorInfo ci in constructorInfoArray)
            {
                if (ci.IsPublic)
                {
                    TKTConstructorDesc bracketCi = ProcDescHelper.CreateProcBracket(ci);
                    list.Add(bracketCi);
                }
            }
            return list.ToArray();
        }

        public override TKTConstructorDesc SearchConstructor(TKTConstructorDesc bracket)
        {
            return ZTypeUtil.SearchConstructor(bracket, this.SharpType);
        }


        public override TKTProcDesc[] GetProces()
        {
            List<TKTProcDesc> list = new List<TKTProcDesc>();
            var methodArray = this.SharpType.GetMethods();
            foreach (var method in methodArray)
            {
                if (!ReflectionUtil.IsDeclare(SharpType, method)) continue;
                /* 映射类可能有多个同义的方法对应同一个实际方法 */
                ZCodeAttribute[] attrs = AttributeUtil.GetAttributes<ZCodeAttribute>(method);
                ProcDescCodeParser parser = new ProcDescCodeParser();
                parser.InitType(SharpType, method);
                foreach (ZCodeAttribute attr in attrs)
                {
                    TKTProcDesc typeProcDesc = parser.Parser(attr.Code);
                    ExMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(method, this.SharpType);
                    typeProcDesc.ExMethod = exMethod;
                    list.Add(typeProcDesc);
                }
            }
            if (ParentMapping != null && !isRoot())
            {
                TKTProcDesc[] epi = ParentMapping.GetProces();
                foreach (var pitem in epi)
                {
                    pitem.ExMethod.IsSelf = false;
                }
                list.AddRange(epi);
            }
            return list.ToArray();
        }

        public override TKTProcDesc SearchProc(TKTProcDesc procDesc)
        {
            var methodArray = this.SharpType.GetMethods();
            foreach (var method in methodArray)
            {
                if (!ReflectionUtil.IsDeclare(SharpType, method)) continue;
                /* 映射类可能有多个同义的方法对应同一个实际方法 */
                ZCodeAttribute[] attrs = AttributeUtil.GetAttributes<ZCodeAttribute>(method);
                /* 编译器生成的类可能没有标注,没有标注的方法必定在ZMappingType上 */
                if (attrs.Length == 0)
                {
                    ExMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(method, this.SharpType);
                    TKTProcDesc typeProcDesc = ProcDescHelper.CreateProcDesc(exMethod);
                    if (typeProcDesc.Eq(procDesc))
                    {
                        TKTProcDesc rdesc = ProcDescHelper.CreateProcDesc(exMethod);
                        return rdesc;
                    }
                }
                else if (attrs.Length > 0)
                {
                    ParameterInfo[] paramArray = method.GetParameters();
                    parser.InitType(SharpType, method);
                    foreach (ZCodeAttribute attr in attrs)
                    {
                        ZCodeAttribute zCodeAttribute = attr as ZCodeAttribute;
                        TKTProcDesc typeProcDesc = parser.Parser(zCodeAttribute.Code);
                        if (method.IsStatic && !method.IsAbstract && typeProcDesc.HasSubject() &&
                            typeProcDesc.GetSubjectArg().ArgType == this.SharpType)
                        {
                            typeProcDesc = typeProcDesc.CreateTail();
                        }
                        if (typeProcDesc.Eq(procDesc))
                        {
                            ExMethodInfo exMethod = null;// getExMethod(method);
                            /* 非 Abstract 的方法肯定从被映射的类中搜索 */
                            if (method.IsAbstract)
                            {
                               var  method2 = searchMethodFromSharp(method);
                                 exMethod = ZTypeUtil.CreatExMethodInfo(method2, this.SharpType);
                                //return exMethod;
                            }
                            else
                            {
                                 exMethod = ZTypeUtil.CreatExMethodInfo(method, this.ZMappingType);
                                //return exMethod;
                            }
                            typeProcDesc.ExMethod = exMethod;
                            return typeProcDesc;
                        }
                    }
                }
            }
            if (ParentMapping != null && !isRoot())
            {
                var epi = ParentMapping.SearchProc(procDesc);
                if (epi != null)
                {
                    //epi.IsSelf = false;
                    return epi;
                }
            }
            return null;
        }

        bool isRoot()
        {
            return this.SharpType == typeof(事物);
        }

        //private ExMethodInfo getExMethod(MethodInfo rmethod)
        //{
        //    //MethodInfo rmethod = method;
        //    /* 非 Abstract 的方法肯定从被映射的类中搜索 */
        //    if (rmethod.IsAbstract)
        //    {
        //        rmethod = searchMethodFromSharp(rmethod);
        //        ExMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(rmethod, this.SharpType);
        //        return exMethod;
        //    }
        //    else
        //    {
        //        ExMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(rmethod, this.ZMappingType);
        //        return exMethod;
        //    }
        //}

        private MethodInfo searchMethodFromSharp(MethodInfo method)
        {
            ParameterInfo[] paramArray = method.GetParameters();
            Type[] types = new Type[paramArray.Length];
            for (int i = 0; i < paramArray.Length; i++)
            {
                types[i] = paramArray[i].ParameterType;
            }
            MethodInfo rmethod = SharpType.GetMethod(method.Name, types);
            return rmethod;
        }

        public override string ToString()
        {
            return string.Format("TKT映射({0}->{1})", ZyyName, ZMappingType.Name);
        }
    }
}
