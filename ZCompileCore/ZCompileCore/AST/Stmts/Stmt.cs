using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers.Raws;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public abstract class Stmt
    {
        public Stmt ParentStmt { get;protected set; }
        //public ContextProc ProcContext { get; set; }

        protected ContextProc _ProcContext;
        public ContextProc ProcContext
        { get { if (this.ParentStmt != null) return this.ParentStmt.ProcContext; else return _ProcContext; } }

        public ILGenerator IL
        {
            get
            {
                return this.ProcContext.GetILGenerator();
            }
        }

        protected void Errorf(CodePosition postion, string msgFormat, params string[] msgParams)
        {
            ASTUtil.Errorf(this.ProcContext.ClassContext.FileContext, postion, msgFormat, msgParams);
        }

        //public void Analy()
        //{
        //    if (this.IsAnalyed) return;
        //    DoAnaly();
        //    IsAnalyed = true;
        //}
        public abstract Stmt Analy();
        //public abstract void DoAnaly();
        public virtual void Emit() { throw new CCException(); }

        //protected Exp AnalyCondition(ExpRaw condition, CodePosition nullPosition)
        //{
        //    if (condition == null)
        //    {
        //        ASTUtil.Errorf(this.ProcContext.ClassContext.FileContext, nullPosition, "条件表达式不是判断表达式");
        //        return null;
        //    }
        //    var condition2 = ParseRawExp(condition);
        //    Exp condition3 = condition2.Analy();
        //    if (condition3.AnalyCorrect)
        //    {
        //        if (condition3.RetType != ZLangBasicTypes.ZBOOL)
        //        {
        //            ASTUtil.Errorf(this.ProcContext.ClassContext.FileContext, condition.Position, "条件表达式不是判断表达式");
        //        }
        //        else
        //        {
        //            return condition3;
        //        }
        //    }
        //    return null;
        //}

        public abstract void AnalyExpDim();

        protected virtual Exp ParseAnalyRawExp(ExpRaw expRaw)
        {
            ExpRawParser rawparser = new ExpRawParser();
            ContextExp context = new ContextExp(this.ProcContext, this);
            Exp exp2 = rawparser.Parse(expRaw, context);
            Exp exp3 = exp2.Analy();
            return exp3;
        }

        protected string getStmtPrefix()
        {
            StringBuilder buff = new StringBuilder();
            int temp = Deep+1;
            while (temp > 0)
            {
                buff.Append("  ");
                temp--;
            }
            return buff.ToString();
        }
        protected int _Deep=-1;
        protected int Deep
        {
            get
            {
                if(_Deep!=-1)
                {
                    _Deep = 0;
                    Stmt temp = this;
                    while (temp.ParentStmt != null)
                    {
                        temp = temp.ParentStmt;
                        _Deep++;
                    }
                }
                return _Deep;
            }
        }
    }
}
