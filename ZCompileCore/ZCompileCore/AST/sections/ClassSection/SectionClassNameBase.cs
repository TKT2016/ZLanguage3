using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

using ZLangRT.Attributes;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

using ZCompileDesc;

namespace ZCompileCore.AST
{
    public abstract class SectionClassNameBase : SectionClassBase
    {
        protected string ClassName;
        protected string BaseTypeName;
        protected TypeBuilder Builder = null;

        public override void AnalyBody()
        {
            return;
        }

        public override void EmitName()
        {
            if (string.IsNullOrEmpty(ClassName)) throw new CCException();
            bool IsStatic = this.FileContext.ClassContext.IsStatic();
            ZLClassInfo BaseZType = this.FileContext.ClassContext.GetSuperZType();
            string packageName = this.FileContext.ProjectContext.PackageName;
            ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;

            var fullName = packageName + "." + ClassName;
            TypeAttributes typeAttrs = TypeAttributes.Public;
            if (IsStatic)
                typeAttrs = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;
            
            if (BaseZType != null)
            {
                Builder = moduleBuilder.DefineType(fullName, typeAttrs, BaseZType.SharpType);
            }
            else
            {
                Builder = moduleBuilder.DefineType(fullName, typeAttrs);
            }
            this.ClassContext.SetTypeBuilder(Builder);
            SetAttr(Builder, IsStatic);
        }

        public override void EmitBody()
        {

        }

        protected void SetAttr(TypeBuilder classBuilder, bool IsStatic)
        {
            Type myType = IsStatic ? typeof(ZStaticAttribute) : typeof(ZInstanceAttribute);
            ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
            CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
            classBuilder.SetCustomAttribute(attributeBuilder);
        }

        public void SetContext(ContextClass classContext)
        {
            this.ClassContext = classContext;
            this.FileContext = this.ClassContext.FileContext;
        }
    }
}
