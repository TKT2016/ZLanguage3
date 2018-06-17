using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCMethodInfo : ZAMethodInfo, ICompling,IParameterCollection
    {
        #region override
        public override AccessAttrEnum GetAccessAttr() { return AccessAttr; }
        public override bool GetIsStatic() { return IsStatic; }
        //public override ZAClassInfo GetZAClass() { return ZClass; }
        public override ZType GetZRetType() { return RetZType; }
        //public override ZAParamInfo[] GetZParams() { return ZParams; }
        //public override ZAMethodDesc[] GetZDescs() { return new ZAMethodDesc[] { ZMethodDesc }; }
        public override string GetSharpName() { return SharpName; }
        #endregion

        #region 构造函数

        public ZCMethodInfo(ZCClassInfo zcclass)
        {
            ZClass = zcclass;
            ZMethodDesc = new ZCMethodDesc(this);
        }

        public ZCMethodInfo(ZCClassInfo zcclass,bool isStatic)
        {
            ZClass = zcclass;
            IsStatic = isStatic;
            ZMethodDesc = new ZCMethodDesc(this);
        }

        #endregion

        #region 字段

        #endregion

        #region 属性

        public MethodBuilder MethodBuilder { get; set; }
        public AccessAttrEnum AccessAttr { get; set; }
        public ZType RetZType { get; set; }
        public bool IsStatic { get; set; }
        public ZCClassInfo ZClass { get; private set; }
        public ZCMethodDesc ZMethodDesc { get;private set; }
        public string SharpName { get; set; }
        public ZCParamInfo[] ZParams { get { return _cparams.ToArray(); } }
        List<ZCParamInfo> _cparams = new List<ZCParamInfo>();

        #endregion

        #region 方法

        public ZCParamInfo GetParameter(string parameterName)
        {
            foreach (var item in _cparams)
            {
                if (item.ZParamName == parameterName)
                {
                    return item;
                }
            }
            return null;
        }

        public bool HasParameter(string parameterName)
        {
            foreach(var item in _cparams)
            {
                if(item.ZParamName== parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AddParameter(ZCParamInfo zcparam)
        {
            if (HasParameter(zcparam.ZParamName)) return false;
            _cparams.Add(zcparam);
            return true;
        }

        public ZCParamInfo AddParameter(string zcparamName)
        {
            if (HasParameter(zcparamName)) return null;
            ZCParamInfo zcparam = new ZCParamInfo(zcparamName,this);
            zcparam.ParamIndex = _cparams.Count;
            _cparams.Add(zcparam);
            return zcparam;
        }

        public bool HasZProcDesc(ZMethodCall procDesc)
        {
            return ZDescUtil.ZEqualsDesc(ZMethodDesc, procDesc);// ZMethodDesc.ZEquals(procDesc);
        }

        public bool HasZProcDesc(ZCMethodDesc procDesc)
        {
            return ZDescUtil.ZEqualsDesc(ZMethodDesc, procDesc); //return ZMethodDesc.ZEquals(procDesc);
        }

        #endregion

        #region 其它
        public override string ToString()
        {
            return "ZCMethodInfo(" + ZMethodDesc + ")";
        }
        #endregion
    }
}
