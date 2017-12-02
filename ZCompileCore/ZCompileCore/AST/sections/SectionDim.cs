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
        public LexToken KeyToken;
        public LexToken NameToken;
        public List<DimVarAST> Dims = new List<DimVarAST>();

        ZDimType EmitedZType;
        public bool IsInClass;

        string DimName;

        public string GetName()
        {
            return DimName;
        }

        public override void AnalyText()
        {
            foreach (DimVarAST item in this.Dims)
            {
                item.AnalyText();
            }
        }

        public override void AnalyType()
        {
            if (!IsInClass)
            {
                if (NameToken == null) return;
                DimName = NameToken.GetText();
            }
            else
            {
                string fileName = this.FileContext.FileModel.GetFileNameNoEx();
                DimName = fileName + "_Dim";
            }
        }

        public override void AnalyBody()
        {
            foreach (var item in Dims)
            {
                item.AnalyBody();
            }
        }

        TypeBuilder Builder;
        public override void EmitName()
        {
            string packageName = this.FileContext.ProjectContext.PackageName;
            ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;
            string fullName = packageName + "." + DimName;
            TypeAttributes typeAttrs = TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit;
            Builder = moduleBuilder.DefineType(fullName, typeAttrs);
            SetZAttrClass(Builder);
        }

        public override void EmitBody()
        {
            var constructorBuilder = Builder.DefineConstructor(
                MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, new Type[] { });
            ILGenerator IL = constructorBuilder.GetILGenerator();

            foreach (var item in Dims)
            {
                item.Emit(Builder, IL);
            }
            IL.Emit(OpCodes.Ret);
        }

        private void SetZAttrClass(TypeBuilder classBuilder)
        {
            Type myType = typeof(ZDimAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
            foreach (DimVarAST item in this.Dims)
            {
                item.SetContext(fileContext);
            }
        }

        public ZDimType GetCreatedZType()
        {
            if (EmitedZType == null)
            {
                Type type = this.Builder.CreateType();
                EmitedZType = ZTypeManager.GetByMarkType(type) as ZDimType;
            }
            return EmitedZType;
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
    }
}
