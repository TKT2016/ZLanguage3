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
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.Words;
using ZCompileDesc.ZTypes;
using ZCompileKit;

namespace ZCompileCore.AST
{
    public class FileClass : FileType
    {
        public SectionImport ImporteSection;
        public SectionUse UseSection;
        public SectionDim DimSection;

        public SectionClassName ClassSection;
        public SectionProperties PropertiesesSection;
        public List<SectionProc> Proces;
        List<SectionConstructor> Constructors = new List<SectionConstructor> ();

        public FileClass(ContextFile fileContext, FileMutilType fmt)
        {
            this.FileContext = fileContext;
            ClassSection = fmt.Classes[0];
            ImporteSection = fmt.ImporteSection;
            Proces = fmt.Proces;
            UseSection = fmt.UseSection;
            Constructors = fmt.Constructors;
            if (fmt.Dimes.Count > 0)
            {
                DimSection = fmt.Dimes[0];
            }
            if (fmt.Propertieses.Count > 0)
            {
                PropertiesesSection = fmt.Propertieses[0];
            }
        }

        public void AnalyTypeName()
        {
            if (ImporteSection != null) ImporteSection.FileContext = this.FileContext;
            if (UseSection != null) UseSection.FileContext = this.FileContext;
           

            if (ClassSection != null) ClassSection.FileContext = this.FileContext;

            string fileName = this.FileContext.FileModel.GetFileNameNoEx();

            if (DimSection != null)
            {
                DimSection.FileContext = this.FileContext;
                DimSection.IsInClass = true;
                DimSection.AnalyName(fileName);
            }

            ClassSection.Analy(this.FileModel.GetFileNameNoEx(), this.FileContext.ClassContext);
            this.FileContext.ClassContext.IsStaticClass = ClassSection.IsStatic;
        }

        public void CompileDim()
        {
            ProjectCompileResult compileResult = this.ProjectContext.CompileResult;
            if(this.DimSection!=null)
            {
                DimSection.AnalyBody();
                if (!HasError())
                {
                    DimSection.EmitBody();
                }

                if (!HasError())
                {
                    var ztype = DimSection.GetCreatedZType();
                    this.ProjectContext.CompileResult.CompiledTypes.Add(ztype);
                }
            }
        }

        public void AnalyDim()
        {
            if (DimSection != null)
            {
                DimSection.AnalyBody();
            }
        }

        public void EmitDim()
        {
            if (DimSection != null)
            {
                if (!HasError())
                {
                    DimSection.EmitBody();
                }
            }
        }

        public void EmitTypeName()
        {
            ModuleBuilder moduleBuilder= this.ProjectContext.EmitContext.ModuleBuilder;
            string packageName = this.ProjectContext.ProjectModel.ProjectPackageName;
            if (DimSection!=null)
                DimSection.EmitName(moduleBuilder, packageName);
            TypeBuilder typeBuilder = ClassSection.Emit(moduleBuilder, packageName);
            this.FileContext.ClassContext.EmitContext.ClassBuilder = typeBuilder;
        }

        public void AnalyImport()
        {
            ImporteSection.FileContext = this.FileContext;
            ImporteSection.Analy();
        }

        public void AnalyUse()
        {
            UseSection.FileContext = this.FileContext;
            UseSection.Analy();
        }

        IWordDictionaryList _TypeDimCollection;
        public IWordDictionary GetTypeDimWords()
        {
            if(_TypeDimCollection==null)
            {
                _TypeDimCollection = new IWordDictionaryList();
                _TypeDimCollection.Add(this.FileContext.ImportContext.TypeNameDict);
                if (DimSection != null)
                {
                    this.FileContext.UseContext.UseZDimList.Insert(0, DimSection.GetCreatedZType());
                }
                _TypeDimCollection.Add(this.FileContext.UseContext.GetUseDimWords() );
            }
            return _TypeDimCollection;
        }

        public void AnalyClassMemberName()
        {
            var tyedimNames = GetTypeDimWords();
            NameTypeParser parser = new NameTypeParser(tyedimNames);
            if (PropertiesesSection != null)
            {
                PropertiesesSection.ClassContext = this.FileContext.ClassContext;
                PropertiesesSection.FileContext = this.FileContext;
                PropertiesesSection.AnalyName(parser);
            }
            if (Constructors.Count > 0)
            {
                foreach (SectionConstructor item in this.Constructors)
                {
                    item.FileContext = this.FileContext;
                    item.ProcContext = new ContextProc(this.FileContext.ClassContext);
                    item.ProcContext.IsConstructor = true;
                    this.FileContext.ClassContext.ProcManagerContext.AddContext(item.ProcContext);
                    item.AnalyName(parser);
                }
            }
            else
            {
                DefaultConstructor = new SectionConstructorDefault();
                DefaultConstructor.FileContext = this.FileContext;
                DefaultConstructor.ProcContext = new ContextProc(this.FileContext.ClassContext);
                DefaultConstructor.ProcContext.IsConstructor = true;
                this.FileContext.ClassContext.ProcManagerContext.AddContext(DefaultConstructor.ProcContext);
            }

            foreach (SectionProc item in Proces)
            {
                item.FileContext = this.FileContext;
                item.ProcContext = new ContextProc(this.FileContext.ClassContext);
                this.FileContext.ClassContext.ProcManagerContext.AddContext(item.ProcContext);//this.ClassContext.ProcManagerContext.Add(item.ProcContext);
                item.AnalyName(parser);
            }
        }
        SectionConstructorDefault DefaultConstructor;
        public void EmitClassMemberName()
        {
            if (PropertiesesSection != null)
                PropertiesesSection.EmitName(ClassSection.IsStatic, ClassSection.Builder);
            if (Constructors.Count > 0)
            {
                foreach (SectionConstructor item in Constructors)
                {
                    item.EmitName();
                }
            }
            else
            {
                DefaultConstructor.EmitName();
            }
            foreach (SectionProc item in Proces)
            {
                item.EmitName();
            }
        }

        public void EmitPropertiesBody()
        {
            if (PropertiesesSection != null)
                PropertiesesSection.EmitValue(ClassSection.IsStatic, ClassSection.Builder);
        }

        public void AnalyProcBody()
        {
            if (Constructors.Count > 0)
            {
                foreach (SectionConstructor item in Constructors)
                {
                    item.AnalyBody();
                }
            }
            else
            {
                DefaultConstructor.EmitBody();
            }
            foreach (SectionProc item in Proces)
            {
                item.AnalyBody();
            }
        }

        public void EmitProcBody()
        {
            if (!HasError())
            {
                foreach (SectionConstructor item in Constructors)
                {
                    item.EmitBody();
                }
                foreach (SectionProc item in Proces)
                {
                    item.EmitBody();
                }
            }
        }

        public void CreateZType()
        {
            ProjectCompileResult compileResult = this.ProjectContext.CompileResult;
            if (!HasError())
            {
                Type type = this.ClassSection.Builder.CreateType();
                IZDescType ztype = ZTypeManager.GetByMarkType(type);
                this.FileContext.EmitedZClass = ztype as ZType;
                this.ProjectContext.CompileResult.CompiledTypes.Add(ztype);
            }
        }

        private bool HasError()
        {
            ProjectCompileResult compileResult = this.ProjectContext.CompileResult;
            var fi = this.FileContext.FileModel.ZFileInfo;
            bool b= compileResult.Errors.ContainsKey(fi);
            return b;
        }
    }
}
