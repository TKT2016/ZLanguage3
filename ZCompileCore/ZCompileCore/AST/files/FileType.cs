using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Engines;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST
{
    public abstract class FileType
    {
        public ContextFile FileContext { set; get; }
        public ContextProject ProjectContext { set; get; }
        public ZFileModel FileModel { set; get; }
        //public void SetContext(ContextProject projectContext, ZyyFileModel fileModel)
        //{
        //    if(FileContext==null)
        //    {
        //        FileContext = new ContextFile(fileModel);
        //    }
        //    FileContext.ProjectContext = projectContext;
        //}
    }
}
