using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Utils;
using ZLangRT.Tags;
using ZLangRT.Descs;
using ZLangRT.Utils;

namespace ZCompileCore.ZTypes
{
    public class ZClassGenType : ZClassType
    {
        ZClassAttribute ClassAttribute { get; set; }

        public override string ZyyName
        {
            get
            {
                return this.SharpType.Name;
            }
        }

        public ZClassGenType(Type type)
        {
            this.SharpType = type;
            this.ClassAttribute = AttributeUtil.GetAttribute<ZClassAttribute>(type);

            Type baseType = ClassAttribute.BaseMappingType != null ? ClassAttribute.BaseMappingType : typeof(Z语言系统.事物);
            ZClassType zc = ZType.CreateZType(baseType) as ZClassType;
            ZClassTypeCache.One.Set(baseType,zc);
            this.ParentMapping = ZClassTypeCache.One.Get(baseType);
        }

        //public override ZType CreateNewFor(Type forType)
        //{
        //    PureZType gcl = new PureZType(forType);
        //    return gcl;
        //}

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

        public override TKTConstructorDesc SearchConstructor(TKTConstructorDesc desc)
        {
            return ZTypeUtil.SearchConstructor(desc, this.SharpType);
        }

        public override ExPropertyInfo[] GetPropertyInfoes()
        {
            List<ExPropertyInfo> exList = new List<ExPropertyInfo>();
            var propertyArray = this.SharpType.GetProperties();
            foreach (var property in propertyArray)
            {
                ZCodeAttribute propertyAttr = Attribute.GetCustomAttribute(property, typeof(ZCodeAttribute)) as ZCodeAttribute;
                if (propertyAttr != null)
                {
                    var zpropertyName = propertyAttr.Code;
                    ExPropertyInfo exPI = new ExPropertyInfo(property, ReflectionUtil.IsDeclare(this.SharpType, property), zpropertyName);
                    exList.Add(exPI);
                }
            }
            ExPropertyInfo[] pArr = ParentMapping.GetPropertyInfoes();
            foreach(ExPropertyInfo pitem in pArr)
            {
                pitem.IsSelf = false;
            }
            exList.AddRange(pArr);
            return exList.ToArray();
        }

        public override ExPropertyInfo SearchExProperty(string name)
        {
            var propertyArray = this.SharpType.GetProperties();
            foreach (var property in propertyArray)
            {
                if (!ReflectionUtil.IsDeclare(this.SharpType, property)) continue;

                ZCodeAttribute propertyAttr = Attribute.GetCustomAttribute(property, typeof(ZCodeAttribute)) as ZCodeAttribute;
                string zpropertyName = null;
                if (propertyAttr != null)
                {
                    //zpropertyName = property.Name;
                    zpropertyName = propertyAttr.Code;
                    if (zpropertyName == name)
                    {
                        return new ExPropertyInfo(property, true, name);
                    }
                }
                //else
                //{
                //    zpropertyName = property.Name;
                //}
                //if (zpropertyName == name)
                //{
                //    return new ExPropertyInfo(property, true, name);
                //}
            }
            //if (ParentMapping != null)
            //{
                ExPropertyInfo epi = ParentMapping.SearchExProperty(name);
                if (epi != null)
                {
                    epi.IsSelf = false;
                    return epi;
                }
            //}
            return null;
        }

        public override TKTProcDesc[] GetProces()
        {
            List<TKTProcDesc> list = new List<TKTProcDesc>();
            var methodArray = this.SharpType.GetMethods();
            foreach (var method in methodArray)
            {
                if (!ReflectionUtil.IsDeclare(SharpType, method)) continue;
                /* 编译器生成的类肯定有标注 */
                ZCodeAttribute procAttr = AttributeUtil.GetAttribute<ZCodeAttribute>(method);
                ProcDescCodeParser parser = new ProcDescCodeParser();
                parser.InitType(SharpType, method);
                TKTProcDesc typeProcDesc = parser.Parser(procAttr.Code);
                ExMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(method, this.SharpType);
                typeProcDesc.ExMethod = exMethod;
                list.Add(typeProcDesc);
            }
            if (ParentMapping != null)
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
            var methodArray = this.SharpType.GetMethods( );
            foreach (var method in methodArray)
            {
                if (!ReflectionUtil.IsDeclare(SharpType, method)) continue;
                /* 编译器生成的类肯定有标注 */
                ZCodeAttribute procAttr = AttributeUtil.GetAttribute<ZCodeAttribute>(method);// Attribute.GetCustomAttribute(method, typeof(ZCodeAttribute)) as ZCodeAttribute;
                //if (procAttr == null)
                //{
                //    ExMethodInfo exMethod = ZTypeHelper.CreatExMethodInfo(method, this.SharpType);
                //    TKTProcDesc typeProcDesc = ProcDescHelper.CreateProcDesc(exMethod);
                //    if (typeProcDesc.Eq(procDesc))
                //    {
                //        return typeProcDesc;
                //    }
                //}
                //else
                //{
                    ProcDescCodeParser parser = new ProcDescCodeParser();
                    parser.InitType(SharpType, method);              
                    TKTProcDesc typeProcDesc = parser.Parser( procAttr.Code);
                    if (typeProcDesc.Eq(procDesc))
                    {
                        ExMethodInfo exMethod = ZTypeUtil.CreatExMethodInfo(method, this.SharpType);
                        typeProcDesc.ExMethod = exMethod;
                        return typeProcDesc;
                    }
                //}
            }
            if (ParentMapping != null)
            {
                return ParentMapping.SearchProc(procDesc);
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("TKT类({0})", ZyyName);
        }
    }
}
