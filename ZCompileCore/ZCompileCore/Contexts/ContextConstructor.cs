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
    public class ContextConstructor : ContextProc
    {
        public ZCConstructorInfo ZConstructorInfo { get;private set; }
        //public ZCMethodDesc CMethodDesc { get;private set; }
        //public ZType RetZType { get; set; }
        //public string ProcName { get; set; }
        //private static int _methodIndex = 0;

        public ContextConstructor(ContextClass classContext)
            : base(classContext)
        {
            IsConstructor = true;
            ZConstructorInfo = new ZCConstructorInfo(classContext.GetZCompilingType());
        }

        public override ZCParamInfo GetParameter(string paramName)
        {
            return ZConstructorInfo.GetParameter(paramName);
        }

        public override ZCParamInfo GetParameter(int i)
        {
            return ZConstructorInfo.ZParams[i];
        }

        public override int GetParametersCount()
        {
            return ZConstructorInfo.ZParams.Length;
        }

        public override bool HasParameter(string name)
        {
            return ZConstructorInfo.HasParameter(name);
        }

        public override ZCParamInfo AddParameterName(string paramName)
        {
            return ZConstructorInfo.AddParameter(paramName);
            //_argDefDict.Add(argSymbol.Name, argSymbol);
        }

        public bool AddParameter(ZCParamInfo zcparam)//(SymbolArg argSymbol)
        {
            return ZConstructorInfo.AddParameter(zcparam);
            //_argDefDict.Add(argSymbol.Name, argSymbol);
        }

        private int NestedIndex = 0;
        public override string CreateNestedClassName()
        {
            NestedIndex++;
            return  "__new__" + "Nested" + NestedIndex;
        }

        public void SetBuilder(ConstructorBuilder builder)
        {
            this.ZConstructorInfo.ConstructorBuilder = builder;
        }

        public override ILGenerator GetILGenerator()
        {
            return this.ZConstructorInfo.ConstructorBuilder.GetILGenerator();
        }

    }
}
