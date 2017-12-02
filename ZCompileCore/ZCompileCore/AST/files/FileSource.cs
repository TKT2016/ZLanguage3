using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Engines;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST
{
    public abstract class FileSource
    {
        public ContextProject ProjectContext { set; get; }
        public ContextFile FileContext { set; get; }
        public ZFileModel FileModel { set; get; }

        protected void AnalyImportStruct(SectionImport importeSection)
        {
            if (importeSection == null) return;
            importeSection.AnalyText();
        }

        protected void AnalyImportTypes(SectionImport importeSection)
        {
            if (importeSection == null) return;
            importeSection.AnalyType();
        }
    }
}
