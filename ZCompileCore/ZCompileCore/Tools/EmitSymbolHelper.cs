using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileKit;
using ZCompileKit.Tools;

namespace ZCompileCore.Tools
{
    public static class EmitSymbolHelper
    {
        public static bool NeedCallThis(SymbolBase symbol)
        {
            if (symbol is SymbolLocalVar) return false;
            else if (symbol is SymbolArg) return false;
            else if (symbol is SymbolDefProperty) 
            {
                SymbolDefProperty symbol2 = (symbol as SymbolDefProperty);
                return symbol2.IsStatic == false;
            }
            else if (symbol is SymbolDefField)
            {
                SymbolDefField symbol2 = (symbol as SymbolDefField);
                return symbol2.IsStatic == false;
            }
            else if (symbol is SymbolRefStaticMember)
            {
                return false;
            }
            else
            {
                throw new CompileCoreException();
            }
        }

        public static void EmitLoad(ILGenerator il, SymbolBase symbol)
        {
            if (symbol is SymbolLocalVar)
            {
                var symbolVar = symbol as SymbolLocalVar;
                EmitHelper.LoadVar(il, symbolVar.VarBuilder);
            }
            else if (symbol is SymbolArg)
            {
                SymbolArg argsymbol = (symbol as SymbolArg);
                EmitHelper.LoadArg(il, argsymbol.ArgIndex);
            }
            else if (symbol is SymbolDefProperty)
            {
                SymbolDefProperty symbol2 = (symbol as SymbolDefProperty);
                MethodInfo getMethod = symbol2.Property.GetGetMethod();
                EmitHelper.CallDynamic(il, getMethod);
            }
            else if (symbol is SymbolDefField)
            {
                SymbolDefField symbol2 = (symbol as SymbolDefField);
                EmitHelper.LoadField(il, symbol2.Field);
            }
            else if (symbol is SymbolRefStaticMember)
            {
                EmitLoad(il, symbol as SymbolRefStaticMember);
            }
            else
            {
                throw new CompileCoreException();
            }
        }

        public static void EmitLoad(ILGenerator il, SymbolRefStaticMember symbol)
        {
            if (symbol.ZMember is ZPropertyInfo)
            {
                MethodInfo getMethod = (symbol.ZMember as ZPropertyInfo).SharpProperty.GetGetMethod();
                EmitHelper.CallDynamic(il, getMethod);
            }
            else if (symbol.ZMember is ZFieldInfo)
            {
                EmitHelper.LoadField(il, (symbol.ZMember as ZFieldInfo).SharpField);
            }
            else if (symbol.ZMember is ZEnumItemInfo)
            {
                int enumValue = (int)((symbol.ZMember as ZEnumItemInfo).Value);
                EmitHelper.LoadInt(il, enumValue);
            }
            else
            {
                throw new CompileCoreException();
            }
        }

        public static void EmitStorm(ILGenerator il, SymbolBase symbol)
        {
            if (symbol is SymbolLocalVar)
            {
                var symbolVar = symbol as SymbolLocalVar;
                EmitHelper.StormVar(il, symbolVar.VarBuilder);
            }
            else if (symbol is SymbolArg)
            {
                SymbolArg argsymbol = (symbol as SymbolArg);
                EmitHelper.StormArg(il, argsymbol.ArgIndex);
            }
            else if (symbol is SymbolDefProperty)
            {
                SymbolDefProperty symbol2 = (symbol as SymbolDefProperty);
                MethodInfo setMethod = symbol2.Property.GetSetMethod();
                EmitHelper.CallDynamic(il, setMethod);
            }
            else if (symbol is SymbolDefField)
            {
                SymbolDefField symbol2 = (symbol as SymbolDefField);
                EmitHelper.StormField(il, symbol2.Field);
            }
            else if (symbol is SymbolRefStaticMember)
            {
                EmitStorm(il, symbol as SymbolRefStaticMember);
            }
            else
            {
                throw new CompileCoreException();
            }
        }

        public static void EmitStorm(ILGenerator il, SymbolRefStaticMember symbol)
        {
            if (symbol.ZMember is ZPropertyInfo)
            {
                MethodInfo setMethod = (symbol.ZMember as ZPropertyInfo).SharpProperty.GetSetMethod();
                EmitHelper.CallDynamic(il, setMethod);
            }
            else if (symbol.ZMember is ZFieldInfo)
            {
                EmitHelper.StormField(il, (symbol.ZMember as ZFieldInfo).SharpField);
            }
            else if (symbol.ZMember is ZEnumItemInfo)
            {
                int enumValue = (int)((symbol.ZMember as ZEnumItemInfo).Value);
                EmitHelper.LoadInt(il, enumValue);
            }
            else
            {
                throw new CompileCoreException();
            }
        }
    }
}
