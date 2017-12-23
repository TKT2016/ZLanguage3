using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST
{
    public class FileDim : FileSource
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
            base.AnalyImportStruct(ImporteSection);
            base.AnalyImportTypes(ImporteSection);
        }

        string typeName;
        public void AnalyTypeName()
        {
            this.FileContext = new ContextFile(this.ProjectContext,this.FileModel);
            DimSection.FileContext = this.FileContext;
            DimSection.AnalyText();
            DimSection.AnalyType();
            typeName = DimSection.GetName();
        }

        public void EmitTypeName()
        {
            DimSection.EmitName();
        }

        public ZLDimInfo Compile()
        {
            SetPartContext();
            AnalyImport();
            DimSection.AnalyBody();
            DimSection.EmitName();
            DimSection.EmitBody();
            var MessageCollection = this.ProjectContext.MessageCollection;
            if (MessageCollection.ContainsErrorSrcKey(this.FileContext.FileModel.ZFileInfo.ZFileName))
            {
                return null;
            }
            else
            {
                var ztype = DimSection.GetCreatedZType();
                return ztype;
            }
            //return null;
        }

        private void SetPartContext()
        {
            var fileContext = this.FileContext;
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
        }
    }
}
