using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileDesc.ZTypes;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZParam
    {
        public bool IsGenericArg { get; set; }
        public string ZParamName { get; set; }
        public ZType ZParamType { get; set; }
        public ParameterInfo ParameterInfo { get; set; }

        public ZParam()
        {
 
        }

        public ZParam(string name, ZType ztype)
        {
            ZParamName = name;
            ZParamType = ztype;
        }

        public ZParam(string name, ZType ztype, ParameterInfo parameterInfo)
        {
            ZParamName = name;
            ZParamType = ztype;
            ParameterInfo = parameterInfo;
        }

        public bool ZEquals(ZParam zparam)
        {
            if (this.IsGenericArg != zparam.IsGenericArg) return false;
            if (IsGenericArg) return true;
            if (this.ZParamType.SharpType == zparam.ZParamType.SharpType) return true;
            //throw new ZLibRTException("Compare超出范围");
            return true;
        }

        public bool ZEquals(ZArg zarg)
        {
            //if (this.IsGeneric != zarg.IsGeneric) return false;
            if (IsGenericArg) return true;
            if (zarg.ZArgType == null || this.ZParamType == null) return false;
            if (ReflectionUtil.IsExtends(zarg.ZArgType.SharpType, this.ZParamType.SharpType)) return true;
            if (ZLambda.IsFn(this.ZParamType.SharpType)) return true;
            return false;
            throw new ZLibRTException("Compare超出范围");
        }

        public string ToZCode()
        {
            if(IsGenericArg)
            {
                var ztype ="类型";
                return string.Format("{0}:{1}", ztype, ZParamName);
            }
            else
            {
                var ztype = ZParamType.ZName ;
                return string.Format("{0}:{1}", ztype, ZParamName);
            }
        }

        public override string ToString()
        {
            return ToZCode();
        }
    }
}
