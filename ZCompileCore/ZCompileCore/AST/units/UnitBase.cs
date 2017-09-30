using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

namespace ZCompileCore.AST
{
    public abstract class UnitBase
    {
        public ContextFile FileContext { get; set; }

        protected virtual void ErrorE(CodePosition postion, string msgFormat, params string[] msgParams)
        {
            this.FileContext.Errorf(postion, msgFormat, msgParams);
        }

    }
}
