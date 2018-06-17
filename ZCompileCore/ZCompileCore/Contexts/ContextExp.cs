using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.AST;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using Z语言系统;

namespace ZCompileCore.Contexts
{
    public class ContextExp
    {
        public Stmt Stmt { get;private set; }
        public ContextProc ProcContext { get; private set; }

        public ContextExp(ContextProc procContext)
        {
            ProcContext = procContext;
        }

        public ContextExp(ContextProc procContext,Stmt stmt)
        {
            ProcContext = procContext;
            Stmt = stmt;
        }

        public ContextClass ClassContext
        {
            get
            {
                return this.ProcContext.ClassContext;
            }
        }

        public ContextFile FileContext
        {
            get
            {
                return this.ClassContext.FileContext;
            }
        }

        public override string ToString()
        {
            return string.Format("ContextExp->{0}", this.ProcContext);
        }
    }
}
