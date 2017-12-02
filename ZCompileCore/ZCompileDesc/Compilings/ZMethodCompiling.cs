using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZLangRT.Attributes;
using ZLangRT.Utils;
using ZCompileDesc.Utils;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;

namespace ZCompileDesc.Compilings
{
    public class ZMethodCompiling : ZMethodInfo, IZCompilingInfo
    {
        //ZMethodDesc MethodDesc;
        public ZMethodCompiling(ZMethodDesc zmethodDesc,ZType retType)
        {
            ZDesces = new ZMethodDesc[] { zmethodDesc };
            _RetZType = retType;
            //MethodDesc = zmethodDesc;
        }

        MethodBuilder ProcBuilder;
        public void SetBuilder(MethodBuilder method)
        {
            ProcBuilder = method;
            MarkMethod = method;
            SharpMethod = method;
        }

        public MethodBuilder GetBuilder()
        {
            return ProcBuilder;
        }

        //bool _isStatic;
        public void SetIsStatic(bool isStatic)
        {
            this.IsStatic = isStatic;
        }

     
        //public ZMethodDesc[] ZDesces { get; protected set; }
        //public AccessAttributeEnum AccessAttribute { get; protected set; }
        //public ZType RetZType 
        //{
        //    get {
        //        Type rtype = SharpMethod.ReturnType;
        //        return ZTypeManager.GetBySharpType(rtype) as ZType;
        //    }
        //}

        //public ZMethodCompilingInfo(MethodInfo method)
        //{
        //    MarkMethod = method;
        //    SharpMethod = method;
        //    Init();
        //}

        //public ZMethodCompilingInfo(MethodInfo markMethod,MethodInfo sharpMethod)
        //{
        //    MarkMethod = markMethod;
        //    SharpMethod = sharpMethod;
        //    Init();
        //}

        //public ZMethodCompilingInfo(MethodBuilder builder, bool isStatic, ZMethodDesc[] desces, AccessAttributeEnum accAttr)
        //{
        //    MarkMethod = builder;
        //    SharpMethod = builder;
        //    IsStatic = isStatic;
        //    ZDesces = desces;
        //    AccessAttribute = accAttr;
        //}

        //protected void Init()
        //{
        //    IsStatic = SharpMethod.IsStatic;
        //    ZDesces = GetProcDesc(MarkMethod, SharpMethod);
        //    AccessAttribute = ReflectionUtil.GetAccessAttributeEnum(SharpMethod);
        //}

        //public virtual bool HasZProcDesc(ZCallDesc procDesc)
        //{
        //    foreach (ZMethodDesc item in ZDesces)
        //    {
        //        if (item.ZEquals(procDesc))
        //            return true;
        //    }
        //    return false;
        //}

        //public virtual bool HasZProcDesc(ZMethodDesc procDesc)
        //{
        //    foreach (ZMethodDesc item in ZDesces)
        //    {
        //        if (procDesc.ZEquals(item))
        //            return true;
        //    }
        //    return false;
        //}

        //protected ZMethodDesc[] GetProcDesc(MethodInfo markMethod, MethodInfo sharpMethod)
        //{
        //    List<ZMethodDesc> list = new List<ZMethodDesc>();
        //    ZCodeAttribute[] attrs = AttributeUtil.GetAttributes<ZCodeAttribute>(markMethod);
        //    foreach (ZCodeAttribute attr in attrs)
        //    {
        //        ZCodeParser parser = new ZCodeParser(sharpMethod.DeclaringType,sharpMethod);
        //        ZMethodDesc typeProcDesc = parser.Parser(attr.Code);
        //        typeProcDesc.ZMethod = this;
        //        list.Add(typeProcDesc);
        //    }
        //    return list.ToArray();
        //}

        //public override string ToString()
        //{
        //    return this.MarkMethod.Name + "(" + string.Join(",", ZDesces.Select(p=>p.ToString())) + ")";
        //}

        //public string SharpMemberName
        //{
        //    get { return this.MarkMethod.Name; }
        //}
    }
}
