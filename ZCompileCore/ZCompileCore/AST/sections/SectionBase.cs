using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

namespace ZCompileCore.AST
{
    public abstract class SectionBase
    {
        public ContextFile FileContext { get; set; }

        protected virtual void errorf(CodePosition postion, string msgFormat, params object[] msgParams)
        {
            this.FileContext.Errorf(postion, msgFormat, msgParams);
        }
        
    }
}
