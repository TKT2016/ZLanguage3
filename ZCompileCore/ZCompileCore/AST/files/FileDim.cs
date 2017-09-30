using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class FileDim : FileType
    {
        public SectionImport ImporteSection;
        public SectionUse UseSection;
        public SectionDim DimSection;

        public FileDim(ContextFile fileContext, SectionDim dim, SectionImport importeSection)
        {
            this.FileContext = fileContext;
            DimSection = dim;
            ImporteSection = importeSection;
        }

        public void AnalyImport()
        {
            if (UseSection != null)
            {
                ImporteSection.Analy();
            }
        }

        public void AnalyUse()
        {
            if (UseSection != null)
            {
                UseSection.Analy();
            }
        }

        string typeName;
        public void AnalyTypeName()
        {
            this.FileContext = new ContextFile(this.ProjectContext,this.FileModel);
            DimSection.FileContext = this.FileContext;
            typeName = DimSection.AnalyName(this.FileContext.FileModel.GetFileNameNoEx());
        }

        public void EmitTypeName()
        {
            DimSection.EmitName(this.ProjectContext.EmitContext.ModuleBuilder, this.ProjectContext.ProjectModel.ProjectPackageName);
        }

        public void Compile()
        {
            ImporteSection.Analy();
            DimSection.AnalyBody();
            DimSection.EmitBody();
            ProjectCompileResult compileResult = this.ProjectContext.CompileResult;
            if (compileResult.Errors.ContainsKey(this.FileContext.FileModel.ZFileInfo))
            {
                return;
            }
            else
            {
                var ztype = DimSection.GetCreatedZType();
                this.ProjectContext.CompileResult.CompiledTypes.Add(ztype);
            }
        }
    }
}
