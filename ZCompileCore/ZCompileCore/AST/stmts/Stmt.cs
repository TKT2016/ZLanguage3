using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public abstract  class Stmt:Tree
    {
        #region Context
        public ContextProc ProcContext { get; set; }
        public ContextClass ClassContext { get { return this.ProcContext.ClassContext; } }
        public override ContextFile FileContext { get { return this.ProcContext.ClassContext.FileContext; } }
        public ContextProject ProjectContext { get { return this.ProcContext.ClassContext.FileContext.ProjectContext; } }
        #endregion

        public bool HasEach { get; set; }
        public int Deep { get; set; }

        public virtual void Analy() { }
        public virtual void Emit() { throw new NotImplementedException(); }

        protected virtual Exp ParseExp(Exp exp)
        {
            Exp exp2 = null;
            ContextExp context = new ContextExp(this.ProcContext, this);
            exp.SetContext(context);
            exp2 = exp.Parse();
            return exp2;
        }

        protected string getStmtPrefix()
        {
            StringBuilder buff = new StringBuilder();
            int temp = Deep;
            while (temp > 0)
            {
                buff.Append("  ");
                temp--;
            }
            return buff.ToString();
        }

        public ILGenerator IL
        {
            get
            {
                return this.ProcContext.EmitContext.ILout;
            }
        }

        protected Exp AnalyCondition(Exp condition,CodePosition nullPosition)
        {
            if (condition==null)
            {
                ErrorE(nullPosition, "条件表达式不是判断表达式");
                return null;
            }
            condition = ParseExp(condition);
            condition = condition.Analy();
            if (condition.AnalyCorrect)
            {
                if (condition.RetType != ZLangBasicTypes.ZBOOL)
                {
                    ErrorE(condition.Position, "条件表达式不是判断表达式");
                }
            }
            return condition;
        }

        //protected void MarkSequencePoint( )
        //{
        //    var idoc = this.ProcContext.ClassContext.EmitContext.IDoc;
        //    if (idoc != null)
        //    {
        //        IL.MarkSequencePoint(idoc, this.Position.Line, this.Position.Col, this.Position.Line, this.Position.Col + this.ToString().Length);
        //    }
        //}
    }
}
