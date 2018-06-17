using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;

namespace ZCompileCore.AST
{
    public class StmtIf:Stmt
    {
        private StmtIfRaw Raw;
        private List<StmtIf_ElseIf> ElseIfParts = new List<StmtIf_ElseIf>();
        private StmtIf_Else ElsePart ; 

        public StmtIf(StmtIfRaw raw, Stmt parentStmt)
        {
            Raw = raw;
            ParentStmt = parentStmt;
        }

        public override Stmt Analy()
        {
            foreach (var item in Raw.ElseIfParts)
            {
                StmtIf_ElseIf ei = new StmtIf_ElseIf(this, item);
                ei.Analy();
                ElseIfParts.Add(ei);
                //item.ProcContext = this.ProcContext;
                //item.Analy();
            }

            if (Raw.ElsePart != null)
            {
                ElsePart = new StmtIf_Else(this, Raw.ElsePart);
                //ElsePart.ProcContext = this.ProcContext;
                ElsePart.Analy();
            }
            return this;
        }

        public override void AnalyExpDim()
        {
            foreach (Stmt stmt in ElseIfParts)
            {
                stmt.AnalyExpDim();
            }
            if (ElsePart != null)
            {
                ElsePart.AnalyExpDim();
            }
        }

        public override void Emit()
        {
            Label EndLabel = IL.DefineLabel();
            Label ElseLabel = IL.DefineLabel();
            List<Label> labels = new List<Label>();
            for (int i = 0; i < ElseIfParts.Count; i++)
            {
                labels.Add(IL.DefineLabel());
            }
            labels.Add(ElseLabel);

            for (int i = 0; i < ElseIfParts.Count; i++)
            {
                var item = ElseIfParts[i];
                item.EndLabel = EndLabel;
                item.CurrentLabel = labels[i];
                item.NextLabel = labels[i + 1];
                item.Emit();
            }
            IL.MarkLabel(ElseLabel);
            if (ElsePart != null)
            {
                ElsePart.EndLabel = EndLabel;
                ElsePart.CurrentLabel = ElseLabel;
                //ElsePart.NextLabel = labels[i + 1];
                ElsePart.Emit();
            }
            IL.MarkLabel(EndLabel);
        }

        #region 覆盖

        //public override CodePosition Position
        //{
        //    get { return IfToken.Position; }
        //}
        public override string ToString()
        {
            return Raw.ToString();
        }

        //public override CodePostion Postion
        //{
        //    get
        //    {
        //        return Parts[0].Postion;
        //    }
        //}
        #endregion


    }
}
