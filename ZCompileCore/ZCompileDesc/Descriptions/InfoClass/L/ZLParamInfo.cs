using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZLParamInfo : ZAParamInfo, ICompleted, IParameter
    {
        public bool IsRuntimeType
        {
            get
            {
                Type parameterType = ParameterInfo.ParameterType;
                return ReflectionUtil.IsRuntimeType(parameterType);
            }
        }
        #region override

        public override bool GetIsFnParam() { return IsFnParam; }
        public override bool GetIsGenericParam() { return IsFnParam; }
        public override string GetZParamName() { return ZParamName; }
        public override ZType GetZParamType() { return ZParamType; }
        public override ZAClassInfo GetZClass() { return ZClass; }
        public override int GetZParameterIndex() { return ParamIndex; }
        public override int GetEmitIndex() { return this.EmitIndex; }

        public override ZAProcInfo GetZProc()
        {
            if (ZConstructor != null){ return ZConstructor; }
            else { return ZMethod; }
        }

        #endregion

        #region 构造函数

        private ZLParamInfo(ParameterInfo parameterInfo, int paramIndex)
        {
            ParameterInfo = parameterInfo;
            ZParamName = parameterInfo.Name;
            ParamIndex = paramIndex;
            if(string.IsNullOrWhiteSpace(ZParamName))
            {
                throw new ZLibRTException("ZCODE错误");
            }
            //IsGenericArg = isGenericArg;
            //if (isGenericArg)
            //{
            //    ZParamType = ZLangBasicTypes.ZOBJECT;
            //    IsFnParam = false; 
            //}
            //else
            //{
            //    ZParamType = (ZLType)ZTypeManager.GetBySharpType(parameterInfo.ParameterType);
            //    IsFnParam = ZLambda.IsFn(this.ZParamType.SharpType);
            //}
        }

        public ZLParamInfo(ParameterInfo parameterInfo, ZLMethodInfo zmethod, int paramIndex)
            : this(parameterInfo, paramIndex)//, isGenericArg)
        {
            ZMethod = zmethod;
        }

        public ZLParamInfo(ParameterInfo parameterInfo, ZLConstructorInfo zconstructor, int paramIndex)
            : this(parameterInfo, paramIndex)//, isGenericArg)
        {
            ZConstructor = zconstructor;        
        }

        #endregion

        #region 属性,字段

        private int ParamIndex;// { private get;private set; }
        public int EmitIndex
        {
            get
            {
                if (this.ZMethod.IsStatic) return ParamIndex; else return ParamIndex + 1;
            }
        }

        public ZLMethodInfo ZMethod { get; private set; }
        public ZLConstructorInfo ZConstructor { get; private set; }
        public string ZParamName { get; private set; }
        public ZLType ZParamType
        {
            get
            {
                if (IsGenericArg) 
                { 
                    return ZLangBasicTypes.ZOBJECT;
                }
                else {
                    Type parameterType = ParameterInfo.ParameterType;
                    return (ZLType)ZTypeManager.GetBySharpType(parameterType); 
                }
            }
        }

        public ParameterInfo ParameterInfo { get; private set; }

        public bool IsFnParam
        {
            get { if (IsGenericArg) { return false; } 
            else { return ZLambda.IsFn(this.ZParamType.SharpType); } }
        }
   

        private bool? _IsGenericArg = null;
        public bool IsGenericArg
        {
            get {
                if (_IsGenericArg==null)
                {
                    if (ZMethod!=null)// && this.ZMethod.GenericParameterDict.ContainsKey(ZParamName))
                    {
                        _IsGenericArg = this.ZMethod.HasGenericParameter(ZParamName);// true;
                    }
                    else if (this.ZClass.GenericTypeDict.ContainsKey(ZParamName))
                    {
                        _IsGenericArg = true;
                    }
                    else
                    {
                        _IsGenericArg = false;
                    }
                }
                return _IsGenericArg.Value; }
        }

        public bool HasCallParameterName() { return true; }
        public ZLClassInfo ZClass
        {
            get
            {
                if (ZConstructor != null) { return ZConstructor.ZClass; }
                else { return ZMethod.ZClass; }
            }
        }
        #endregion
    }
}
