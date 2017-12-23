using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCConstructorInfo : ZAConstructorInfo, ICompling
    {
        #region override

        public override AccessAttrEnum GetAccessAttr() { return AccessAttrEnum.Public; }
        public override bool GetIsStatic() { return IsStatic; }
        //public override ZAClassInfo GetZAClass() { return ZClass; }
        //public override ZAParamInfo[] GetZParams() { return ZParams; }
        //public override ZAConstructorDesc GetZDesc() { return ZDesc; }

        #endregion

         #region 构造函数

        public ZCConstructorInfo(ZCClassInfo zcclass)
        {
            ZClass = zcclass;
            ZDesc = new ZCConstructorDesc(this);
        }

        #endregion

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
            foreach (var item in _cparams)
            {
                if (item.ZParamName == parameterName)
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
            _cparams.Add(zcparam);
            return zcparam;
        }

        public Type[] GetParameterTypes()
        {
            return ZParams.Select(u => ZTypeUtil.GetTypeOrBuilder(u.ZParamType)).ToArray();
        }

        public ZCParamInfo[] GetNormalParameters()
        {
            return ZParams.Where(u => u.GetIsGenericParam()).ToArray();
        }

        public ConstructorBuilder ConstructorBuilder { get; set; }
        public ZCClassInfo ZClass { get; set; }
        public bool IsStatic { get; set; }
        public ZCConstructorDesc ZDesc { get; protected set; }
        public ZCParamInfo[] ZParams { get { return _cparams.ToArray(); } }
        List<ZCParamInfo> _cparams = new List<ZCParamInfo>();
        public void AddZParam(ZCParamInfo zparam)
        {
            _cparams.Add(zparam);
        }
    }
}
