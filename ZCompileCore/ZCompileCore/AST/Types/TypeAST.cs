using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST
{
    public abstract class TypeAST
    {
        //public SectionName NameSection { get; protected set; }
        public FileAST ASTFile { get; protected set; }

        public ContextFile FileContext { get { return this.ASTFile.FileContext; } }
        public ContextProject ProjectContext { get { return this.ASTFile.FileContext.ProjectContext; } }
        //public SectionExtends ExtendsSection;
        //public SectionProperties PropertiesSection;

        public TypeAST(FileAST astFile)//, SectionNameRaw nameSection) //, SectionExtends extendsSection)//, SectionProperties propertiesSection)
        {
            ASTFile = astFile;
            //NameSection = new SectionName ( nameSection);
        }

        public bool HasError()
        {
            var MessageCollection = this.ProjectContext.MessageCollection;
            if (MessageCollection.ContainsErrorSrcKey(this.FileContext.FileModel.ShowKeyPath))//.ZFileInfo.ZFileName))
            {
                return true;
            }
            return false;
        }

        public abstract void AnalyNameSection();
        public abstract string GetTypeName();
        //{
        //    if(NameSection==null)
        //    {
        //        NameSection = new SectionName();
        //    }

        //    NameSection.Analy(this);
        //}

        protected string GetTypeFullName()
        {
            string packageName = this.FileContext.ProjectContext.PackageName;

            var fullName = packageName + "." + GetTypeName();
            return fullName;
        }

        public virtual void AnalyTypeName()
        {
            AnalyNameSection();
        }
    }
}
