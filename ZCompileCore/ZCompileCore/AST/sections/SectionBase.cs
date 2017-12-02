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

        protected virtual void ErrorF(CodePosition postion, string msgFormat, params object[] msgParams)
        {
            this.FileContext.Errorf(postion, msgFormat, msgParams);
        }

        public abstract void AnalyText();
        public abstract void AnalyType();
        public abstract void AnalyBody();
        public abstract void EmitName();
        public abstract void EmitBody();
    }
}
