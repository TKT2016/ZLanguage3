using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT.Attributes;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class SectionDim : SectionBase
    {
        public Token KeyToken;
        public Token NameToken;
        public List<DimVarAST> Dims = new List<DimVarAST>();
        ZDimType EmitedZType;
        public bool IsInClass;

        string typeName;
        public string AnalyName(string fileName)
        {
            if (!IsInClass)
            {
                if (NameToken == null) return null;
                typeName = NameToken.GetText();
                return typeName;
            }
            else
            {
                typeName = fileName + "_Dim";
                return typeName;
            }
        }

        public ZDimType GetCreatedZType()
        {
            if(EmitedZType==null)
            {
                Type type = this.Builder.CreateType();
                EmitedZType = ZTypeManager.GetByMarkType(type) as ZDimType;
            }
            return EmitedZType;
        }

        public TypeBuilder Builder;
        public TypeBuilder EmitName(ModuleBuilder moduleBuilder, string packageName)
        {
            string fullName = packageName + "." + typeName;
            TypeAttributes typeAttrs = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit;
            Builder = moduleBuilder.DefineType(fullName, typeAttrs);
            SetZAttrClass(Builder);
            return Builder;
        }

        public bool AnalyBody()
        {
            if (Dims.Count == 0) return false;
            foreach (var item in Dims)
            {
                item.Analy();
            }
            return true;
        }

        public void EmitBody()
        {
            var constructorBuilder = Builder.DefineConstructor(
                MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, new Type[] { });
            ILGenerator IL = constructorBuilder.GetILGenerator();

            foreach(var item in Dims)
            {
                item.Emit(Builder,IL);
            }
            IL.Emit(OpCodes.Ret);
        }

        protected void SetZAttrClass(TypeBuilder classBuilder)
        {
            Type myType = typeof(ZDimAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append("声明");
            if (this.NameToken != null)
            {
                buff.Append(this.NameToken.GetText());
            }
            buff.Append(":");

            for (int i = 0; i < this.Dims.Count; i++)
            {
                var p = this.Dims[i];
                buff.Append(p.ToString());
                if (i < this.Dims.Count - 1)
                    buff.Append(",");
            }
            return buff.ToString();
        }
        
        public class DimVarAST
        {
            public Token NameToken;
            public Token TypeToken;
            public string DimName;
            public string DimTypeName;

            public bool Analy()
            {
                DimName = NameToken.GetText();
                DimTypeName = TypeToken.GetText();
                return true;
            }

            FieldBuilder fieldBuilder;
            public FieldBuilder Emit(TypeBuilder classBuilder, ILGenerator il)
            {
                FieldAttributes fieldAttr = FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly;
                fieldBuilder = classBuilder.DefineField(DimName, typeof(string), fieldAttr);
                EmitHelper.LoadString(il, DimTypeName);
                EmitHelper.StormField(il, fieldBuilder);
                return fieldBuilder;
            }

            public override string ToString()
            {
                return string.Format("{0}={1}", NameToken.GetText(), TypeToken.GetText());
            } 
        }
    }
}
