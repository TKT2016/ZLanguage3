using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

namespace ZCompileCore.AST
{
    public abstract class SectionPartFile
    {
        protected ContextFile FileContext { get;set; }

        protected virtual void ErrorF(CodePosition postion, string msgFormat, params string[] msgParams)
        {
            this.FileContext.Errorf(postion, msgFormat, msgParams);
        }

        public abstract void AnalyText();
        public abstract void AnalyType();
        public abstract void AnalyBody();
        public abstract void EmitName();
        public abstract void EmitBody();

        //public virtual void SetFileContext(ContextFile fileContext)
        //{
        //    FileContext = fileContext;
        //}
    }

    public abstract class SectionPartClass : SectionPartFile
    {
        public ContextClass ClassContext { get; protected set; }

        //public virtual void SetClassContext(ContextClass classContext)
        //{
        //    ClassContext = classContext;
        //    FileContext = ClassContext.FileContext;
        //}
    }

    public abstract class SectionPartProc : SectionPartClass
    {
        //public ContextMethod ProcContext { get; protected set; }
        public ContextProc ProcContext { get; protected set; }

        //public virtual void SetProcContext(ContextProc procContext)
        //{
        //    ProcContext = procContext;
        //    ClassContext = ProcContext.ClassContext;
        //    FileContext = ClassContext.FileContext;
        //}
    }
}
