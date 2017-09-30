using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.AST
{
    public class StmtBlock : Stmt
    {
        public List<Stmt> StmtList { get; set; }

        public StmtBlock()
        {
            StmtList = new List<Stmt>();
        }

        List<int> catchStmtIndexList;

        public override void Analy()
        {
            catchStmtIndexList = new List<int>();
            //catchStmtIndexList.Add(0);
            for (var i = 0; i < StmtList.Count; i++)// Stmt stmt in StmtList)
            {
                Stmt stmt = StmtList[i];
                stmt.ProcContext = this.ProcContext;//.SetProcContext(this.ProcContext);
                stmt.Analy();
                if (stmt is StmtCatch)
                {
                    catchStmtIndexList.Add(i+1);
                }
            }
            if (catchStmtIndexList.Count > 0)
            {
                InsertCatch();
            }
        }

        private void InsertCatch()
        {
            catchStmtIndexList.Insert(0, 0);
            catchStmtIndexList.RemoveAt(catchStmtIndexList.Count - 1);
            for (var i = 0; i < catchStmtIndexList.Count; i++)
            {
                StmtTry tryStmt = new StmtTry();
                tryStmt.ProcContext = this.ProcContext;
                int index = catchStmtIndexList[i] + i;
                StmtList.Insert(index, tryStmt);
            }
        }

        public override void Emit()
        {
            foreach (Stmt stmt in StmtList)
            {
                stmt.Emit();
            }
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, StmtList);
        } 
    }
}
