using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Tools;

namespace ZCompileCore.AST
{
    public class StmtIf_ElseIf : Stmt
    {
        private StmtIfRaw.IfElseStmt Raw;
        private Exp ConditionExp;
        private StmtBlock StmtBody;

        public StmtIf_ElseIf(StmtIf parentStmt, StmtIfRaw.IfElseStmt raw)
        {
            ParentStmt = parentStmt;
            Raw = raw;
            StmtBody = new StmtBlock(this, raw.Body);
        }

        public Label CurrentLabel { get; set; }
        public Label NextLabel { get; set; }
        public Label EndLabel { get; set; }

        public override Stmt Analy()
        {
            //Condition.IsTopExp = true;
            //Condition = AnalyCondition(Condition, KeyToken.Position);
            //Body.ProcContext = this.ProcContext;
            ConditionExp = ParseAnalyRawExp(Raw.ElseIfExp);
            StmtBody.Analy();
            return this;
        }

        public override void AnalyExpDim()
        {
            ConditionExp.AnalyDim();
            StmtBody.AnalyExpDim();
        }

        public override void Emit()
        {
            IL.MarkLabel(CurrentLabel);
            ConditionExp.Emit();
            EmitHelper.LoadInt(IL, 1);
            IL.Emit(OpCodes.Ceq);
            IL.Emit(OpCodes.Brfalse, NextLabel);
            StmtBody.Emit();
            IL.Emit(OpCodes.Br, EndLabel);
        }

        //public override CodePosition Position
        //{
        //    get { return KeyToken.Position; }
        //}

        #region 覆盖
        public override string ToString()
        {
            return Raw.ToString();
            //StringBuilder buf = new StringBuilder();
            //buf.Append(getStmtPrefix());
            //buf.AppendFormat("{0}{1}", KeyToken.ToCode(), this.Condition.ToString());
            //buf.AppendLine();
            //buf.Append(Body.ToString());
            //buf.AppendLine();
            //return buf.ToString();
        }

        //public override CodePosition Position
        //{
        //    get
        //    {
        //        return KeyToken.Position;
        //    }
        //}
        #endregion
    }
}
