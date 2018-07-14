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
    public class StmtIf_Else : Stmt
    {
        private StmtIfRaw.ElseStmt Raw;
        //private Exp ConditionExp;
        private StmtBlock StmtBody;

        public StmtIf_Else(StmtIf parentStmt, StmtIfRaw.ElseStmt raw)
        {
            ParentStmt = parentStmt;
            Raw = raw;
            StmtBody = new StmtBlock(this, raw.Body);
        }

        public Label CurrentLabel { get; set; }
        public Label EndLabel { get; set; }

        public override Stmt Analy()
        {
            //Condition.IsTopExp = true;
            //Condition = AnalyCondition(Condition, KeyToken.Position);
            //Body.ProcContext = this.ProcContext;
            //ConditionExp = ParseRawExp(Raw.ElseIfExp);
            StmtBody.Analy();
            return this;
        }

        public override void AnalyExpDim()
        {
            StmtBody.AnalyExpDim();
        }

        public override void Emit()
        {
            //EmitHelper.LoadInt(IL, 1);
            //IL.Emit(OpCodes.Ceq);
            //IL.Emit(OpCodes.Brfalse, EndLabel);
            StmtBody.Emit();
            //IL.Emit(OpCodes.Br, EndLabel);
        }

        #region 覆盖
        public override string ToString()
        {
            return Raw.ToString();
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
