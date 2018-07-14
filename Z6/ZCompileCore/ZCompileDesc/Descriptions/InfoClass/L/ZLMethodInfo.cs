using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions.Utils;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLMethodInfo : ZAMethodInfo, ICompleted
    {
        #region override
        public override AccessAttrEnum GetAccessAttr() { return AccessAttr; }
        public override bool GetIsStatic() { return IsStatic; }
        //public override ZAClassInfo GetZAClass() { return ZClass; }
        public override ZType GetZRetType() { return RetZType; }
        //public override ZAParamInfo[] GetZParams() { return ZParams; }
        //public override ZAMethodDesc[] GetZDescs() { return ZDescs; }
        public override string GetSharpName() { return SharpName; }
        #endregion

        #region 构造函数

        public ZLMethodInfo(MethodInfo markMethod, MethodInfo sharpMethod, ZLClassInfo zclass)
        {
            ZClass = zclass;
            MarkMethod = markMethod;
            SharpMethod = sharpMethod;
            AccessAttr = ZClassUtil.GetAccessAttributeEnum(sharpMethod);
            GenericParameterDict = GenericUtil.GetMethodGenericParameters(sharpMethod);
            //Init();
        }

        #endregion

        #region 字段
        private ZLMethodDesc[] _ZLMethodDescs;
        private ZLParamInfo[] _ZLParamInfos;
        #endregion

        #region 方法

        public bool HasGenericParameter(string parameter)
        {
            if (this.GenericParameterDict.ContainsKey(parameter))
            {
                return true;
            }
            else if (this.ZClass.GenericTypeDict.ContainsKey(parameter))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Type GetGenericParameter(string parameter)
        {
            if (this.GenericParameterDict.ContainsKey(parameter))
            {
                return this.GenericParameterDict[parameter];
            }
            else if (this.ZClass.GenericTypeDict.ContainsKey(parameter))
            {
                return this.ZClass.GenericTypeDict[parameter];
            }
            else
            {
                return null;
            }
        }

        public virtual bool HasZProcDesc(ZMethodCall procDesc)
        {
            foreach (ZLMethodDesc item in ZDescs)
            {
                if (ZDescUtil.ZEqualsDesc(item, procDesc))
                    return true;
            }
            return false;
        }


        public virtual bool HasZProcDesc(ZCMethodDesc zdesc)
        {
            foreach (ZLMethodDesc item in ZDescs)
            {
                if (ZDescUtil.ZEqualsDesc(item, zdesc))//)item.ZEquals(procDesc))
                    return true;
            }
            return false;
        }

        public ZLParamInfo SearchParameter(string parameterName)
        {
            var parameters = this.ZParams;
            foreach (var item in parameters)
            {
                if (item.ZParamName == parameterName)
                {
                    return item;
                }
            }
            return null;
        }

        #endregion

        #region 属性

        public Dictionary<string, Type> GenericParameterDict { get; private set; }

        public ZLParamInfo[] ZParams
        {
            get
            {
                if (_ZLParamInfos == null)
                {
                    _ZLParamInfos = ZClassUtil.GetZLParams(this);
                }
                return _ZLParamInfos;
            }
        }

        public ZLMethodDesc[] ZDescs
        {
            get
            {
                if (_ZLMethodDescs == null)
                {
                    _ZLMethodDescs = ZClassUtil.GetProcDescs(this);
                }
                return _ZLMethodDescs;
            }
        }

        public ZLClassInfo ZClass { get; private set; }
        public MethodInfo MarkMethod { get; private set; }
        public MethodInfo SharpMethod { get; private set; }
        public bool IsStatic { get { return SharpMethod.IsStatic; } }
        public AccessAttrEnum AccessAttr { get; private set; }
        protected ZLType _RetZType;
        public ZLType RetZType
        {
            get
            {
                if (_RetZType == null)
                {
                    Type rtype = SharpMethod.ReturnType;
                    _RetZType = ZTypeManager.GetBySharpType(rtype);
                }
                return _RetZType;
            }
        }
        public string SharpName
        {
            get { return this.MarkMethod.Name; }
        }
        #endregion

        #region 其它
        public override string ToString()
        {
            return this.MarkMethod.Name + "(" + string.Join(",", ZDescs.Select(p => p.ToString())) + ")";
        }
        #endregion

    }
}
