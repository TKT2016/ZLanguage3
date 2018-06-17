using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZLangRT;
using ZLangRT.Utils;

namespace ZCompileCore.AST
{
    static class ExpBinaryUtil
    {
        public enum CalculaterMethodTypeEnum
        {
           None,ContactString, MathOp,MathDiv, MathCompare, RefCompare, Logic
        }

        public static CalculaterMethodTypeEnum GetCalculaterMethodType(TokenKindSymbol opKind, Type ltype, Type rtype)
        {
            if (ltype == typeof(string) || rtype == typeof(string))
            {
                if (opKind == TokenKindSymbol.ADD)
                {
                    return CalculaterMethodTypeEnum.ContactString;
                }
            }
            if (opKind == TokenKindSymbol.ADD || opKind == TokenKindSymbol.SUB || opKind == TokenKindSymbol.MUL)
            {
                return CalculaterMethodTypeEnum.MathOp;
            }
            if (opKind == TokenKindSymbol.DIV)
            {
                return CalculaterMethodTypeEnum.MathDiv;
            }
            if (opKind == TokenKindSymbol.GT || opKind == TokenKindSymbol.GE || opKind == TokenKindSymbol.LT || opKind == TokenKindSymbol.LE)
            {
                return CalculaterMethodTypeEnum.MathCompare;
            }
            if (opKind == TokenKindSymbol.AND || opKind == TokenKindSymbol.OR)
            {
                return CalculaterMethodTypeEnum.Logic;
            }
            if (opKind == TokenKindSymbol.EQ || opKind == TokenKindSymbol.NE)
            {
                if (Calculater.IsNumberType(ltype) && Calculater.IsNumberType(rtype))
                {
                    return CalculaterMethodTypeEnum.MathCompare;
                }
                else
                {
                    return CalculaterMethodTypeEnum.RefCompare;
                }
            }
            return CalculaterMethodTypeEnum.None;
        }

        public static Type GetRetType(CalculaterMethodTypeEnum calculaterMethodType, Type ltype, Type rtype)
        {
            if (calculaterMethodType == CalculaterMethodTypeEnum.None) return null;
            if (calculaterMethodType == CalculaterMethodTypeEnum.ContactString) return typeof(string);
            if (calculaterMethodType == CalculaterMethodTypeEnum.MathOp)
            {
                if (ltype == typeof(int) || rtype == typeof(int)) return typeof(int);
                else return typeof(float);
            }
            if (calculaterMethodType == CalculaterMethodTypeEnum.MathDiv)
            {
                return typeof(float);
            }
            if (calculaterMethodType == CalculaterMethodTypeEnum.MathCompare) return typeof(bool);
            if (calculaterMethodType == CalculaterMethodTypeEnum.RefCompare) return typeof(bool);
            if (calculaterMethodType == CalculaterMethodTypeEnum.Logic) return typeof(bool);
            return null;
        }

        public static MethodInfo GetCalcMethod(TokenKindSymbol opKind,  Type ltype, Type rtype)
        {
            CalculaterMethodTypeEnum calculaterMethodType = GetCalculaterMethodType(opKind, ltype, rtype);
            if (calculaterMethodType == CalculaterMethodTypeEnum.None) return null;
            return GetCalcMethod(calculaterMethodType, opKind, ltype, rtype);
        }

        public static MethodInfo GetCalcMethod(TokenKindSymbol opKind, ZType ltype, ZType rtype)
        {
            if(ltype is ZLType  && rtype is ZLType)
            {
                return GetCalcMethod(opKind, (ltype as ZLType).SharpType, (rtype as ZLType).SharpType);
            }
            return null;
        }

        private static MethodInfo GetCalcMethod(CalculaterMethodTypeEnum calculaterMethodType, TokenKindSymbol opKind, Type ltype, Type rtype)
        {
            Type calcType = typeof(Calculater);
            
            if (calculaterMethodType == CalculaterMethodTypeEnum.ContactString) return calcType.GetMethod("AddString");
            string opName = GetOpName(opKind);
            if (opName == null) return null;
            string typeName = GetTypeName(ltype, rtype);
            if (typeName == null) return null;
            string methodName = opName + typeName;

            if (calculaterMethodType == CalculaterMethodTypeEnum.MathOp) return calcType.GetMethod(methodName);
            if (calculaterMethodType == CalculaterMethodTypeEnum.MathDiv) return calcType.GetMethod(methodName);
            if (calculaterMethodType == CalculaterMethodTypeEnum.MathCompare) return calcType.GetMethod(methodName);
            if (calculaterMethodType == CalculaterMethodTypeEnum.RefCompare) return calcType.GetMethod(methodName);
            if (calculaterMethodType == CalculaterMethodTypeEnum.Logic) return calcType.GetMethod(methodName);
            return null;
        }

        public static string GetOpName(TokenKindSymbol opKind)
        {
            if (opKind == TokenKindSymbol.ADD) return "Add";
            if (opKind == TokenKindSymbol.SUB) return "Sub";
            if (opKind == TokenKindSymbol.MUL) return "Mul";
            if (opKind == TokenKindSymbol.DIV) return "Div";
            if (opKind == TokenKindSymbol.GT) return "GT";
            if (opKind == TokenKindSymbol.GE) return "GE";
            if (opKind == TokenKindSymbol.EQ) return "EQ";
            if (opKind == TokenKindSymbol.NE) return "NE";
            if (opKind == TokenKindSymbol.LT) return "LT";
            if (opKind == TokenKindSymbol.LE) return "LE";
            if (opKind == TokenKindSymbol.AND) return "AND";
            if (opKind == TokenKindSymbol.OR) return "OR";
            return null;
        }

        public static string GetTypeName(Type ltype, Type rtype)
        {
            if (ltype == typeof(bool) && rtype == typeof(bool)) return "Bool";
            if (ltype == typeof(int) && rtype == typeof(int)) return "Int";
            if (Calculater.IsNumberType(ltype) && Calculater.IsNumberType(rtype)) return "Float";
            return "Ref";
        }
    }
}
