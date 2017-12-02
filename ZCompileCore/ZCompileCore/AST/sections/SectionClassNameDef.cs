using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZLangRT.Attributes;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class SectionClassNameDef : SectionClassNameBase
    {
        public LexToken BaseTypeToken;
        public LexToken NameToken;

        //string ClassName;
        //string BaseTypeName;

        public override void AnalyText()
        {
            string fileName = this.FileContext.FileModel.GetFileNameNoEx();
            if (NameToken == null)
            {
                ClassName = fileName;
            }
            else
            {
                ClassName = NameToken.GetText();
                if (ClassName != fileName)
                {
                    this.FileContext.Errorf(BaseTypeToken.Position, "类名称 '" + ClassName + "'和文件名称'" + fileName + "'不一致");
                }
            }
            this.ClassContext.SetClassName(ClassName);
        }

        public override void AnalyType()
        {
            bool IsStatic = false;
            ZClassType BaseZType = null;
            if (BaseTypeToken == null)
            {
                IsStatic = false;
            }
            else
            {
                BaseTypeName = BaseTypeToken.GetText();
                if (BaseTypeName == "唯一")
                {
                    IsStatic = true;
                }
                else if (BaseTypeName == "普通" || BaseTypeName == "一般")
                {
                    IsStatic = false;
                    BaseZType = ZLangBasicTypes.ZOBJECT;
                }
                else
                {
                    IsStatic = false;
                    var ztypes = ZTypeManager.GetByMarkName(BaseTypeName);
                    if (ztypes.Length == 0)
                    {
                        ErrorF(BaseTypeToken.Position, "类型'{0}'不存在", BaseTypeName);
                    }
                    else if (ztypes.Length == 1)
                    {
                        BaseZType = ztypes[0] as ZClassType;
                    }
                    else if (ztypes.Length == 0)
                    {
                        ErrorF(BaseTypeToken.Position, "'{0}'存在{1}个同名类型", BaseTypeName, ztypes.Length.ToString());
                    }
                }
            }

            this.FileContext.ClassContext.SetIsStatic(IsStatic);
            this.FileContext.ClassContext.SetSuperClass(BaseZType);
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
            buf.Append(BaseTypeToken.GetText());
            buf.Append("类型");
            buf.Append(":");
            buf.Append(NameToken.GetText());
            return buf.ToString();
        }
    }
}
