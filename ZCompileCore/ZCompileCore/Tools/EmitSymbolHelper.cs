using System.Reflection;
using System.Reflection.Emit;

using ZCompileDesc.Descriptions;
using ZCompileKit;
using ZCompileKit.Tools;

namespace ZCompileCore.Tools
{
    public static class EmitSymbolHelper
    {
        //public static bool EmitLoad(ILGenerator il, IIdent iident)
        //{
        //    if (iident is ZLParamInfo)
        //    {
        //        EmitLoad(il, (ZLParamInfo)iident); 
        //        return true;
        //    }
        //    else if (iident is ZCParamInfo)
        //    {
        //        EmitLoad(il, (ZCParamInfo)iident);
        //        return true;
        //    }
        //    else if (iident is ZCLocalVar)
        //    {
        //        EmitLoad(il, (ZCLocalVar)iident);
        //        return true;
        //    }
        //    else if (iident is ZCFieldInfo)
        //    {
        //        EmitLoad(il, (ZCFieldInfo)iident);
        //        return true;
        //    }
        //    else if (iident is ZLFieldInfo)
        //    {
        //        EmitLoad(il, (ZLFieldInfo)iident);
        //        return true;
        //    }
        //    else if (iident is ZLPropertyInfo)
        //    {
        //        EmitLoad(il, (ZLPropertyInfo)iident);
        //        return true;
        //    }
        //    else if (iident is ZCPropertyInfo)
        //    {
        //        EmitLoad(il, (ZCPropertyInfo)iident);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    //EmitHelper.LoadArg(il, zcparam.EmitIndex);
        //}

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
            EmitHelper.LoadArg(il,zcparam.EmitIndex);
        }

        public static void EmitLoad(ILGenerator il, ZLParamInfo zcparam)
        {
            EmitHelper.LoadArg(il, zcparam.EmitIndex);
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

        //public static void EmitStorm(ILGenerator il, IIdent symbol)
        //{
        //    if (symbol is ZCLocalVar)
        //    {
        //        var symbolVar = symbol as ZCLocalVar;
        //        EmitHelper.StormVar(il, symbolVar.VarBuilder);
        //    }
        //    //else if (symbol is SymbolArg)
        //    //{
        //    //    SymbolArg argsymbol = (symbol as SymbolArg);
        //    //    EmitHelper.StormArg(il, argsymbol.ArgIndex);
        //    //}
        //    //else if (symbol is SymbolDefProperty)
        //    //{
        //    //    SymbolDefProperty symbol2 = (symbol as SymbolDefProperty);
        //    //    MethodInfo setMethod = symbol2.Property.GetSetMethod();
        //    //    EmitHelper.CallDynamic(il, setMethod);
        //    //}
        //    //else if (symbol is SymbolDefField)
        //    //{
        //    //    SymbolDefField symbol2 = (symbol as SymbolDefField);
        //    //    EmitHelper.StormField(il, symbol2.Field);
        //    //}
        //    //else if (symbol is SymbolRefStaticMember)
        //    //{
        //    //    EmitStorm(il, symbol as SymbolRefStaticMember);
        //    //}
        //    else
        //    {
        //        throw new CCException();
        //    }
        //}

        //public static bool NeedCallThis(IIdent symbol)
        //{
        //    if (symbol is ZCLocalVar) return false;
        //    else if (symbol is ZCParamInfo) return false;
        //    //else if (symbol is SymbolDefProperty) 
        //    //{
        //    //    SymbolDefProperty symbol2 = (symbol as SymbolDefProperty);
        //    //    return symbol2.IsStatic == false;
        //    //}
        //    //else if (symbol is SymbolDefField)
        //    //{
        //    //    SymbolDefField symbol2 = (symbol as SymbolDefField);
        //    //    return symbol2.IsStatic == false;
        //    //}
        //    //else if (symbol is SymbolRefStaticMember)
        //    //{
        //    //    return false;
        //    //}
        //    else
        //    {
        //        throw new CCException();
        //    }
        //}
    }
}
