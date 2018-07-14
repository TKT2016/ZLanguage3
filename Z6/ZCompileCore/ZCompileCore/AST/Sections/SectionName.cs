using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileDesc.Descriptions;
using ZLangRT.Attributes;

namespace ZCompileCore.AST
{
    public abstract class SectionNameBase
    {
        public SectionNameRaw Raw { get; protected set; }
        public TypeAST ASTType { get; protected set; }
        public string TypeName { get; protected set; }

        //public SectionNameBase( )
        //{
            
        //}

        public SectionNameBase(SectionNameRaw raw)
        {
            Raw = raw;
        }

        protected void AnalyRaw(TypeAST ast)
        {
            ASTType = ast;
            if(Raw==null)
            {
                TypeName = this.ASTType.FileContext.FileModel.GeneratedClassName;
                //this.ASTType.FileContext.SetClassName(TypeName);
            }
            else
            {
                string fileName = this.ASTType.FileContext.FileModel.GeneratedClassName;
                if (Raw.NameToken == null)
                {
                    TypeName = fileName;
                }
                else
                {
                    TypeName = Raw.NameToken.Text;
                    //if (TypeName != fileName)
                    //{
                    //    this.FileContext.Errorf(BaseTypeToken.Position, "类名称 '" + ClassName + "'和文件名称'" + fileName + "'不一致");
                    //}
                }
                //this.ClassContext.SetClassName(ClassName);
            }
            
        }

        //protected void SetAttr(TypeBuilder classBuilder, bool IsStatic)
        //{
        //    Type myType = IsStatic ? typeof(ZStaticAttribute) : typeof(ZInstanceAttribute);
        //    ConstructorInfo infoConstructor = myType.GetConstructor(new Type[] { });
        //    CustomAttributeBuilder attributeBuilder = new CustomAttributeBuilder(infoConstructor, new object[] { });
        //    classBuilder.SetCustomAttribute(attributeBuilder);
        //}

        protected ContextFile FileContext
        {
            get { return this.ASTType.FileContext; }
        }
    }
}
