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
    public class SectionClassNameDefault : SectionClassNameBase
    {
        //string ClassName;
        //string BaseTypeName;

        public override void AnalyText()
        {
            string fileName = this.FileContext.FileModel.GetFileNameNoEx();
            ClassName = fileName;
            this.ClassContext.SetClassName(ClassName);
        }

        public override void AnalyType()
        {
            this.FileContext.ClassContext.SetIsStatic(false);
            this.FileContext.ClassContext.SetSuperClass(ZLangBasicTypes.ZOBJECT);
        }

        //public override void AnalyBody()
        //{

        //}
        //public TypeBuilder Builder = null;
        //public override void EmitName()
        //{
        //    bool IsStatic = this.FileContext.ClassContext.IsStatic();
        //    ZClassType BaseZType = this.FileContext.ClassContext.GetSuperZType();
        //    string packageName = this.FileContext.ProjectContext.PackageName;
        //    ModuleBuilder moduleBuilder = this.FileContext.ProjectContext.EmitContext.ModuleBuilder;

        //    var fullName = packageName + "." + ClassName;
        //    TypeAttributes typeAttrs = TypeAttributes.Public;
        //    if (IsStatic)
        //        typeAttrs = TypeAttributes.Public | TypeAttributes.Abstract | TypeAttributes.Sealed;
            
        //    if (BaseZType != null)
        //    {
        //        Builder = moduleBuilder.DefineType(fullName, typeAttrs, BaseZType.SharpType);
        //    }
        //    else
        //    {
        //        Builder = moduleBuilder.DefineType(fullName, typeAttrs);
        //    }
        //    SetAttr(Builder, IsStatic);
        //}

        //public override void EmitBody()
        //{

        //}

        //private void SetAttr(TypeBuilder classBuilder, bool IsStatic)
        //{
        //    Type myType = IsStatic ? typeof(ZStaticAttribute) : typeof(ZInstanceAttribute);
        //    ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
        //    CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
        //    classBuilder.SetCustomAttribute(attributeBuilder);
        //}

        //public void SetContext(ContextClass classContext)
        //{
        //    this.ClassContext = classContext;
        //    this.FileContext = this.ClassContext.FileContext;
        //}

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("普通");
            buf.Append("类型");
            buf.Append(":");
            buf.Append("");
            return buf.ToString();
        }
    }
}
