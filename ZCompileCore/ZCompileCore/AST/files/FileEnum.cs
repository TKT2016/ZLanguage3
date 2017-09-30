using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.AST
{
    public class FileEnum:FileType
    {
        public List<SectionEnum> EnumSections;

        public FileEnum(ContextFile fileContext,List<SectionEnum> enumSections)
        {
            this.FileContext = fileContext;
            EnumSections = enumSections;
        }

        public void Compile()
        {
            ProjectCompileResult compileResult = this.ProjectContext.CompileResult;
            if (compileResult.Errors.ContainsKey(this.FileContext.FileModel.ZFileInfo))
            {
                return;
            }
            else
            {
                foreach (SectionEnum enumSection in EnumSections)
                {
                    ZType ztype = enumSection.Compile(this.ProjectContext.EmitContext.ModuleBuilder, this.ProjectContext.ProjectModel.ProjectPackageName);
                    if (ztype != null)
                    {
                        this.ProjectContext.CompileResult.CompiledTypes.Add(ztype);
                    }
                }
            }
        }
    }
}
