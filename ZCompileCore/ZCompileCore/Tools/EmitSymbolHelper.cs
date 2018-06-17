using System.Reflection;
using System.Reflection.Emit;

using ZCompileDesc.Descriptions;
using ZCompileCore;
using ZCompileCore.Tools;

namespace ZCompileCore.Tools
{
    public static class EmitSymbolHelper
    {
        public static bool EmitLoada(ILGenerator il, IIdent iident)
        {
            if (iident is ZAParamInfo)
            {
                ZAParamInfo zlp = (ZAParamInfo)iident;
                EmitHelper.LoadArga(il, zlp.GetEmitIndex());
                return true;
            }
            else if (iident is ZCLocalVar)
            {
                ZCLocalVar zlp = (ZCLocalVar)iident;
                EmitHelper.LoadVara(il, zlp.VarBuilder);
                return true;
            }
            else if (iident is ZCFieldInfo)
            {
                ZCFieldInfo zlp = (ZCFieldInfo)iident;
                EmitHelper.LoadFielda(il, zlp.FieldBuilder);
                return true;
            }
            else if (iident is ZLFieldInfo)
            {
                ZLFieldInfo zlp = (ZLFieldInfo)iident;
                EmitHelper.LoadFielda(il, zlp.SharpField);
                return true;
            }
            else
            {
                return false;
            }
            //EmitHelper.LoadArg(il, zcparam.EmitIndex);
        }

        public static void EmitLoad(ILGenerator il, ZAPropertyInfo memberCompiling)
        {
            if(memberCompiling is ZLPropertyInfo)
            {
                EmitLoad(il, (ZLPropertyInfo)memberCompiling);
            }
            else if (memberCompiling is ZCPropertyInfo)
            {
                EmitLoad(il, (ZCPropertyInfo)memberCompiling);
            }
            else
            {
                throw new CCException();
            }
        }

        public static void EmitLoad(ILGenerator il, ZLFieldInfo zfield)
        {
            EmitHelper.LoadField(il, zfield.SharpField);
        }

        public static void EmitLoad(ILGenerator il, ZCPropertyInfo memberCompiling)
        {
            PropertyBuilder propertyBuilder = memberCompiling.PropertyBuilder;
            MethodInfo getMethod = propertyBuilder.GetGetMethod();
            EmitHelper.CallDynamic(il, getMethod);
        }

        public static void EmitLoad(ILGenerator il, ZLPropertyInfo property)
        {
            var ppi = property.SharpProperty;
            MethodInfo getMethod = ppi.GetGetMethod();
            EmitHelper.CallDynamic(il, getMethod);
        }

        public static void EmitLoad(ILGenerator il, ZCLocalVar symbolVar)
        {
            EmitHelper.LoadVar(il, symbolVar.VarBuilder);
        }

        public static void EmitLoad(ILGenerator il, ZCParamInfo zcparam)
        {
            ILGeneratorUtil.LoadArg(il, zcparam.EmitIndex);
        }

        public static void EmitLoad(ILGenerator il, ZLParamInfo zcparam)
        {
            ILGeneratorUtil.LoadArg(il, zcparam.EmitIndex);
        }

        public static void EmitLoad(ILGenerator il, ZCFieldInfo memberCompiling)
        {
             EmitHelper.LoadField(il, memberCompiling.FieldBuilder);
        }

        public static void EmitStorm(ILGenerator il, ZCFieldInfo memberCompiling)
        {
            EmitHelper.StormField(il, memberCompiling.FieldBuilder);
        }

        public static void EmitStorm(ILGenerator il, ZLFieldInfo memberCompiling)
        {
            EmitHelper.StormField(il, memberCompiling.SharpField);
        }

        public static void EmitStorm(ILGenerator il, ZCPropertyInfo memberCompiling)
        {
            PropertyBuilder propertyBuilder = memberCompiling.PropertyBuilder;
            MethodInfo setMethod = propertyBuilder.GetSetMethod();
            EmitHelper.CallDynamic(il, setMethod);
        }

        public static void EmitStorm(ILGenerator il, ZLPropertyInfo memberCompiling)
        {
            var property = memberCompiling.SharpProperty;
            MethodInfo setMethod = property.GetSetMethod();
            EmitHelper.CallDynamic(il, setMethod);
        }

        public static void EmitStorm(ILGenerator il, ZCLocalVar symbolVar)
        {
            EmitHelper.StormVar(il, symbolVar.VarBuilder);
        }

        public static void EmitStorm(ILGenerator il, ZCParamInfo argsymbol)
        {
            EmitHelper.StormArg(il, argsymbol.EmitIndex);
        }

        public static void EmitStorm(ILGenerator il, ZLParamInfo zp)
        {
            EmitHelper.StormArg(il, zp.EmitIndex);
        }
         
    }
}
