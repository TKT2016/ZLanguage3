using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.CommonCollections;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileNLP;
using System.Diagnostics;
using ZLangRT.Utils;
using System.Reflection;

namespace ZCompileCore.Contexts
{
    public class ContextMethod : ContextProc
    {
        public ZCMethodInfo ZMethodInfo { get;private set; }
        public string ProcName { get; set; }

        public ContextMethod(ContextClass classContext ):base(classContext)
        {
            ZCClassInfo cclass = this.ClassContext.GetZCompilingType();
            ZMethodInfo = new ZCMethodInfo(cclass);
            cclass.AddMethod(ZMethodInfo);
        }

        public void SetIsStatic(bool isStatic)
        {
            this.ZMethodInfo.IsStatic = isStatic;
        }

        public override ZCParamInfo GetParameter(string paramName)
        {
            return ZMethodInfo.GetParameter(paramName);
        }

        public override ZCParamInfo GetParameter(int i)
        {
            return ZMethodInfo.ZParams[i];
        }

        public override int GetParametersCount()
        {
            return ZMethodInfo.ZParams.Length;
        }

        public override bool HasParameter(string name)
        {
            return ZMethodInfo.HasParameter(name);
        }

        public override ZCParamInfo AddParameterName(string paramName)
        {
            return ZMethodInfo.AddParameter(paramName);
        }

        public bool AddParameter(ZCParamInfo zcparam)
        {
            return ZMethodInfo.AddParameter(zcparam);
        }

        public ZType RetZType
        {
            get
            {
                return ZMethodInfo.RetZType;
            }
            set
            {
                ZMethodInfo.RetZType = value;
            }
        }

        private int NestedIndex = 0;
        public override string CreateNestedClassName()
        {
            NestedIndex++;
            return (ProcName ?? "") + "Nested" + NestedIndex;
        }

        public void SetBuilder(MethodBuilder methodBuilder)
        {
            this.ZMethodInfo.MethodBuilder = methodBuilder;
        }

        public MethodBuilder GetBuilder()
        {
            return this.ZMethodInfo.MethodBuilder;
        }

        public override ILGenerator GetILGenerator()
        {
            return this.ZMethodInfo.MethodBuilder.GetILGenerator();
        }

        public override string ToString()
        {
            return string.Format("ContextMethod[{0}]->{1}",ProcName, this.ClassContext.ToString() );
        }
    }
}
