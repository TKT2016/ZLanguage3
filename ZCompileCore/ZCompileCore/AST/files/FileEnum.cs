using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;


namespace ZCompileCore.AST
{
    public class FileEnum:FileSource
    {
        public List<SectionEnum> EnumSections;

        public FileEnum(ContextFile fileContext,List<SectionEnum> enumSections)
        {
            this.FileContext = fileContext;
            EnumSections = enumSections;
        }

        public ZLEnumInfo Compile()
        {
            SetContext(this.FileContext);
            var MessageCollection = this.ProjectContext.MessageCollection;
            if (MessageCollection.ContainsErrorSrcKey(this.FileContext.FileModel.ZFileInfo.ZFileName))
            {
                return null;
            }
            else
            {
                foreach (SectionEnum enumSection in EnumSections)
                {
                    var builder = this.ProjectContext.EmitContext.ModuleBuilder;
                    var packageName = this.ProjectContext.ProjectModel.ProjectPackageName;
                    var fileName = this.FileContext.FileModel.GetFileNameNoEx();
                    ZLEnumInfo ztype = enumSection.Compile(builder, packageName, fileName);
                    return ztype;
                }
            }
            return null;
        }

        public void SetContext(ContextFile fileContext)
        {
            this.FileContext = fileContext;
            foreach (SectionEnum itemPackage in this.EnumSections)
            {
                itemPackage.SetContext(fileContext);
            }
        }
    }
}
