using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileDesc.Descriptions
{
    public class ZCParamInfo : ZAParamInfo, ICompling,IParameter,IIdent
    {
        #region override

        public override bool GetIsFnParam(){return IsFnParam; }
        public override bool GetIsGenericParam() { return false; }
        public override string GetZParamName() { return ZParamName; }
        public override ZType GetZParamType() { return ZParamType; }
        public override ZAClassInfo GetZClass() { return ZClass; }
        public override int GetZParameterIndex() { return ParamIndex; }
        public override int GetEmitIndex() { return this.EmitIndex; }

        public override ZAProcInfo GetZProc()
        {
            if (ZConstructor != null)
            { return ZConstructor; } 
            else { return ZMethod; }
        }

        #endregion

        public ZCParamInfo(string name, ZCConstructorInfo constructor)
        {
            ZParamName = name;
            SetProc(constructor);
        }

        public ZCParamInfo(string name, ZCMethodInfo method)
        {
            ZParamName = name;
            SetProc(method);
        }
        //public ZCParamInfo(string name,ZType ztype)
        //{
        //    ZParamName = name;
        //    ZParamType = ztype;
        //}
        public ParameterBuilder ParamBuilder { get; set; }
        public bool IsFnParam { get; set; }
        public string ZParamName { get; set; }
        public ZType ZParamType { get; set; }
        public ZCClassInfo ZClass { get; set; }
        public int ParamIndex { private get; set; }
        public int EmitIndex
        {
            get
            {
                if (this.ZMethod.IsStatic) return ParamIndex; else return ParamIndex+1;
            }
        }
        public override string ZName { get { return ZParamName; } }
        public override ZType GetZType() { return ZParamType; } 
        public ZCConstructorInfo ZConstructor { get;private set; }
        public ZCMethodInfo ZMethod { get; private set; }

        public bool HasCallParameterName() { return true; }

        public void SetProc(ZCConstructorInfo constructor)
        {
            ZConstructor = constructor;
            ZMethod = null;
        }

        public void SetProc(ZCMethodInfo method)
        {
            ZConstructor = null;
            ZMethod = method;
        }

        public void DefineParameter( )
        {
            string pname = this.ZParamName;
            ParameterAttributes pa = ParameterAttributes.None;
            
            /* 参数在参数列表中的位置。 通过第一个参数以数字 1 开头对参数编制索引；数字 0 表示方法的返回值。 */
            int ParamIndex = this.ParamIndex + 1;
            if (this.ZMethod != null)
            {
                this.ParamBuilder = ZMethod.MethodBuilder.DefineParameter(ParamIndex, pa, pname);
            }
            else
            {
                this.ParamBuilder = ZConstructor.ConstructorBuilder.DefineParameter(ParamIndex, pa, pname);
            }
        }

        //public bool GetCanRead { get{ return false;}}
        //public bool GetCanWrite { get { return false; } }

        public override string ToString()
        {
            return string.Format("ZCParamInfo({0}:{1})", ZParamType.ZTypeName ,ZParamName);
        }

        public string ToZode()
        {
            return string.Format("{0}:{1}", ZParamType.ZTypeName, ZParamName);
        }
    }
}
