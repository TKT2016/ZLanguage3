using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions 
{
    public class ZArgCall : ACall, IParameter
    {
        public bool IsRuntimeType { get { return false; } }
        public bool IsGeneric { get; set; }
        public bool HasCallParameterName() {  return !string.IsNullOrWhiteSpace(ZArgName); } 
        public string ZArgName { get; set; }
        public ZType ZArgType { get; set; }

        public ZArgCall()
        {
 
        }

        public ZArgCall(string name, ZType ztype)
        {
            ZArgName = name;
            ZArgType = ztype;
        }

        public ZType GetZParamType()
        {
            return ZArgType;
        }

        public bool IsCallArg()
        {
            return true;
        }

        public string GetZParamName()
        {
            return ZArgName;
        }

        //public override bool ZEquals(ZAParamInfo zlparam)
        //{
        //    if (zlparam.IsGenericArg) return true;
        //    if (this.ZArgType == null || zlparam.ZParamType == null) return false;
        //    if (this.ZArgType is ZLType)
        //    {
        //        ZLType zlargType = (this.ZArgType as ZLType);
        //        if (ReflectionUtil.IsExtends(zlargType.SharpType, zlparam.ZParamType.SharpType)) return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    if (ZLambda.IsFn(zlparam.ZParamType.SharpType)) return true;
        //    return false;
        //}

        public override string ToZCode()
        {
            if (HasCallParameterName())
            {
                return string.Format("{0}={1}", ZArgName, ZArgType.ZTypeName);
            }
            else
            {
                var ztype = ZArgType.ZTypeName ;
                return ZArgType.ZTypeName;
            }
        }

    }
}
