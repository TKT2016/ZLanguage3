using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Reports;
using ZCompileDesc;
using ZCompileDesc.Collections;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class FileClass : FileSource
    {
        public SectionImport ImporteSection;
        public SectionUse UseSection;
        public SectionDim DimSection;
        public SectionClassNameBase ClassNameSection;
        public SectionProperties PropertiesSection;
        public List<SectionProc> Proces;
        public List<SectionConstructorBase> Constructors = new List<SectionConstructorBase>();
        ContextClass ClassContext;
        public FileClass(ContextFile fileContext, FileMutilType fmt)
        {
            this.FileContext = fileContext;
            this.ClassContext = new ContextClass(this.FileContext);
            this.FileContext.ClassContext = this.ClassContext;

            ClassNameSection = fmt.Classes[0];
            ImporteSection = fmt.ImporteSection;
            Proces = fmt.Proces;
            UseSection = fmt.UseSection;
            Constructors.AddRange(fmt.Constructors);
            if (fmt.Dimes.Count > 0)
            {
                DimSection = fmt.Dimes[0];
            }
            if (fmt.Propertieses.Count > 0)
            {
                PropertiesSection = fmt.Propertieses[0];
            }
        }

        public ZClassType Compile()
        {
            ParseAddDefault();//分析补充默认
            SetPartContext(this.FileContext);
            Analy_ImportUse();//分析导入使用

            CompileClassName();//编译类名
            CompileClassStruct();//编译结构
            CompileBody();//编译属性值、构造函数体、过程体

            var ztype = this.CreateZType();
            return ztype;
        }

        private void CompileBody()
        {
            AnalyCompilingBody();
            EmitCompilingBody();
        }

        private void CompileClassStruct()
        {
            ParseClassStructText();
            AnalyClassStructType();
            EmitCompilingStruct();
            this.FileContext.ImportUseContext.ImportCompiling_Body(this.ClassContext.GetZCompilingType());
        }

        private void CompileClassName()
        {
            ClassNameSection.AnalyText();
            ClassNameSection.AnalyType();
            ClassNameSection.EmitName();
            this.FileContext.ImportUseContext.AddCompiling_Name(this.ClassContext.GetZCompilingType());
        }

        private void ParseAddDefault()
        {
            if (this.Constructors.Count == 0)
            {
                SectionConstructorDefault scd = new SectionConstructorDefault();
                this.Constructors.Add(scd);
            }
            if (ClassNameSection == null)
            {
                ClassNameSection = new SectionClassNameDefault();
                ClassNameSection.SetContext(this.ClassContext);
            }
        }

        private void EmitCompilingBody( )
        {
            if (PropertiesSection != null)
            {
                PropertiesSection.EmitBody();
            }
            if (Proces.Count > 0)
            {
                foreach (var item in Proces)
                {
                    item.EmitBody();
                }
            }
            foreach (var item in Constructors)
            {
                item.EmitBody();
            }
        }

        private void AnalyCompilingBody( )
        {
            if (PropertiesSection != null)
            {
                PropertiesSection.AnalyBody();
            }
            if (Proces.Count > 0)
            {
                foreach (var item in Proces)
                {
                    item.AnalyBody();
                }
            }
            foreach (var item in Constructors)
            {
                item.AnalyBody();
            }
        }

        private void EmitCompilingStruct( )
        {
            if (PropertiesSection != null)
            {
                PropertiesSection.EmitName();
            }
            if (Proces.Count > 0)
            {
                foreach (var item in Proces)
                {
                    item.EmitName();
                }
            }
            foreach (var item in Constructors)
            {
                item.EmitName();
            }
        }

        private void AnalyClassStructType( )
        {
            if (PropertiesSection != null)
            {
                PropertiesSection.AnalyType();
            }
            if (Proces.Count > 0)
            {
                foreach (var item in Proces)
                {
                    item.AnalyType();
                }
            }
            foreach (var item in Constructors)
            {
                item.AnalyType();
            }
        }

        private void EmitTypeName()
        {
            ClassNameSection.EmitName();
        }

        private void Analy_ImportUse()
        {
            //AddDefaultPackage();//在ContextFileTypes中已经导入
            base.AnalyImportStruct(ImporteSection);
            base.AnalyImportTypes(ImporteSection);

            if (this.UseSection != null)
            {
                this.UseSection.AnalyText();
                this.UseSection.AnalyType();
            }
        }

        private void ParseClassStructText()
        {
            if (DimSection != null)
            {
                DimSection.AnalyText();
            }
            //if (ClassNameSection != null)
            //{
            //    ClassNameSection.AnalyText();
            //}
            //else
            //{
            //    string className = className = this.FileContext.FileModel.GetFileNameNoEx();
            //    this.FileContext.ClassContext.SetClassName(className);
            //}

            if (PropertiesSection != null)
            {
                PropertiesSection.AnalyText();
            }

            if (Proces.Count > 0)
            {
                foreach (var item in Proces)
                {
                    item.AnalyText();
                }
            }

            foreach (var item in Constructors)
            {
                item.AnalyText();
            }
        }

        private void SetPartContext(ContextFile fileContext)
        {
            if (ImporteSection != null)
            {
                ImporteSection.SetContext(fileContext);
            }
            if (this.UseSection != null)
            {
                this.UseSection.SetContext(fileContext);
            }
            if (DimSection != null)
            {
                DimSection.SetContext(fileContext);
            }
            ContextClass classContext = fileContext.ClassContext;
            if (ClassNameSection != null)
            {
                ClassNameSection.SetContext(classContext);
            }
            if (PropertiesSection != null)
            {
                PropertiesSection.SetContext(classContext);
            }
            if (Proces.Count > 0)
            {
                foreach (var item in Proces)
                {
                    item.SetContext(classContext);
                }
            }
            foreach (var item in Constructors)
            {
                item.SetContext(classContext);
            }
        }

        private ZClassType CreateZType()
        {
            if (!HasError())
            {
                Type type = this.ClassContext.GetTypeBuilder().CreateType();
                IZDescType ztype = ZTypeManager.GetByMarkType(type);
                ZClassType zclasstype = (ZClassType)ztype;
                this.FileContext.EmitedIZDescType = zclasstype;
                return zclasstype;
            }
            return null;
        }

        private bool HasError()
        {
            var MessageCollection = this.ProjectContext.MessageCollection;
            return (MessageCollection.ContainsErrorSrcKey(this.FileContext.FileModel.ZFileInfo.ZFileName));
        }
    }
}
