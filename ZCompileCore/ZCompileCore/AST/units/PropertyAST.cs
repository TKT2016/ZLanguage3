using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class PropertyAST : UnitBase
    {
        public Token NameToken;
        public Exp PropertyValue;

        string PropertyName;
        ZType PropertyZType;

        public SymbolDefProperty PropertySymbol { get; private set; }

        public void Analy(NameTypeParser parser, PropertyContextCollection context, ContextProc procContext)
        {
            var classContext = procContext.ClassContext;
            this.FileContext = classContext.FileContext;
            NameTypeParser.ParseResult result = parser.ParseVar(NameToken);
            if (PropertyValue == null && result == null)
            {
                PropertyName = NameToken.GetText();
                PropertyZType = ZLangBasicTypes.ZOBJECT;
                ErrorE(NameToken.Position, "无法确定属性'{0}'的数据类型", PropertyName);
            }
            else
            {
                if (PropertyValue != null)
                {
                    AnalyValue(procContext);
                }

                if (result != null)
                {
                    PropertyName = result.VarName;
                    PropertyZType = result.ZType;
                }
                else
                {
                    PropertyName = NameToken.GetText();
                    PropertyZType = PropertyValue.RetType;
                }
            }

            if (context.Dict.ContainsKey(PropertyName))
            {
                ErrorE(NameToken.Position, "属性'{0}'重复", PropertyName);
            }
            else
            {
                WordInfo word = new WordInfo(PropertyName, WordKind.MemberName, this);
                context.Dict.Add(word);
            }

            PropertySymbol = new SymbolDefProperty(PropertyName, PropertyZType, classContext.IsStaticClass);
            PropertySymbol.HasDefaultValue = (PropertyValue != null);
            classContext.AddMember(PropertySymbol);
        }

        PropertyBuilder propertyBuilder;
        FieldBuilder fieldBuilder;
        public void EmitName(bool isStatic, TypeBuilder classBuilder)
        {
            Type propertyType = PropertyZType.SharpType;
            MethodAttributes methodAttr = getMethodAttr(isStatic);
            FieldAttributes fieldAttr = getFieldAttr(isStatic);

            fieldBuilder = classBuilder.DefineField("_" + PropertyName, propertyType, fieldAttr);
            propertyBuilder = classBuilder.DefineProperty(PropertyName, PropertyAttributes.None, propertyType, null);

            EmitGet(classBuilder, PropertyName, propertyType, isStatic, fieldBuilder, propertyBuilder, methodAttr);
            EmitSet(classBuilder, PropertyName, propertyType, isStatic, fieldBuilder, propertyBuilder, methodAttr);
            PropertySymbol.Property = propertyBuilder;
        }

        public void AnalyValue( ContextProc procContext)
        {
            Exp exp2 = null;
            ContextExp context = new ContextExp(procContext);
            PropertyValue.SetContext(context);
            exp2 = PropertyValue.Parse();
            exp2 = exp2.Analy();
            PropertyValue = exp2;
        }

        public void EmitValue(bool isStatic, ILGenerator il)
        {
            if (PropertyValue == null) return;
            EmitHelper.EmitThis(il, isStatic);
            PropertyValue.Emit();
            EmitHelper.StormField(il, fieldBuilder);
        }

        MethodAttributes getMethodAttr(bool isStatic)
        {
            if (isStatic)
                return MethodAttributes.Public | MethodAttributes.Static;
            else
                return MethodAttributes.Public;
        }

        FieldAttributes getFieldAttr(bool isStatic)
        {
            if (isStatic)
                return FieldAttributes.Private | FieldAttributes.Static;
            else
                return FieldAttributes.Private;
        }

        private void EmitGet(TypeBuilder classBuilder, string propertyName, Type propertyType, bool isStatic, FieldBuilder field, PropertyBuilder property, MethodAttributes methodAttr)
        {
            MethodBuilder methodGet = classBuilder.DefineMethod("get_" + propertyName, methodAttr, propertyType, null);
            var ilget = methodGet.GetILGenerator();
            EmitHelper.EmitThis(ilget, isStatic);
            LocalBuilder retBuilder = ilget.DeclareLocal(propertyType);
            EmitHelper.LoadField(ilget, field);
            EmitHelper.StormVar(ilget, retBuilder);
            EmitHelper.LoadVar(ilget, retBuilder);
            ilget.Emit(OpCodes.Ret);
            property.SetGetMethod(methodGet);
        }

        private void EmitSet(TypeBuilder classBuilder, string propertyName, Type propertyType, bool isStatic, FieldBuilder field, PropertyBuilder property, MethodAttributes methodAttr)
        {
            MethodBuilder methodSet = classBuilder.DefineMethod("set_" + propertyName, methodAttr, typeof(void), new Type[] { propertyType });
            ILGenerator ilSet = methodSet.GetILGenerator();
            ilSet.Emit(OpCodes.Ldarg_0);
            EmitHelper.Emit_LoadThis(ilSet, isStatic);
            EmitHelper.StormField(ilSet, field);
            ilSet.Emit(OpCodes.Ret);
            property.SetSetMethod(methodSet);
        }

        public CodePosition Position
        {
            get
            {
                return this.NameToken.Position;
            }
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(this.NameToken.GetText());
            if (this.PropertyValue != null)
            {
                buff.Append("=");
                buff.Append(PropertyValue.ToString());
            }
            return buff.ToString();
        }
    }
}
