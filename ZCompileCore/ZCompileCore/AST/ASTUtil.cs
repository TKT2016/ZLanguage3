using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZLangRT.Attributes;

namespace ZCompileCore.AST
{
    public static class ASTUtil
    {
        public static void Errorf(ContextFile fileContext, CodePosition postion, string msgFormat, params string[] msgParams)
        {
            fileContext.Errorf(postion, msgFormat, msgParams);
        }

        public static void SetAttrZCode(FieldBuilder fieldBuilder, string zcode)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { zcode });
            fieldBuilder.SetCustomAttribute(attributeBuilder);
        }

        public static void SetZAttrEnum(EnumBuilder classBuilder)
        {
            Type myType = typeof(ZEnumAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public static void SetZAttrDim(TypeBuilder classBuilder)
        {
            Type myType = typeof(ZDimAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public static void SetZAttrClass(TypeBuilder classBuilder, bool IsStatic)
        {
            Type myType = IsStatic ? typeof(ZStaticAttribute) : typeof(ZInstanceAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public static void SetZAttr(PropertyBuilder builder, string name)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { name });
            builder.SetCustomAttribute(attributeBuilder);
        }

        public static void SetAttrZCode(MethodBuilder methodBuilder,string code)
        {
            Type myType = typeof(ZCodeAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { typeof(string) });
            //string code = this.NamePart.GetZDesc().ToZCode();
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { code });
            methodBuilder.SetCustomAttribute(attributeBuilder);
        }

    }
}
