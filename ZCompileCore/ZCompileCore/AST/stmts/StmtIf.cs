using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.ASTExps;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public  class StmtIf:Stmt
    {
        public LexToken IfToken;
        public List<StmtIfTrue> Parts = new List<StmtIfTrue>();
        public StmtBlock ElsePart { get; set; }

        public override void DoAnaly()
        {
            foreach (var item in Parts)
            {
                item.ProcContext = this.ProcContext;
                item.Analy();
            }

            if (ElsePart != null)
            {
                ElsePart.ProcContext = this.ProcContext;
                ElsePart.Analy();
            }
        }

        public override void Emit()
        {
            Label EndLabel = IL.DefineLabel();
            Label ElseLabel = IL.DefineLabel();
            List<Label> labels = new List<Label>();
            for (int i = 0; i < Parts.Count; i++)
            {
                labels.Add(IL.DefineLabel());
            }
            labels.Add(ElseLabel);

            for (int i = 0; i < Parts.Count; i++)
            {
                var item = Parts[i];
                item.EndLabel = EndLabel;
                item.CurrentLabel = labels[i];
                item.NextLabel = labels[i + 1];
                item.Emit();
            }
            IL.MarkLabel(ElseLabel);
            if (ElsePart != null)
                ElsePart.Emit();
            IL.MarkLabel(EndLabel);
        }

        #region 覆盖
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (var tpart in Parts)
            {
                buf.AppendLine(tpart.ToString());
            }
            if (ElsePart != null)
            {
                buf.Append(getStmtPrefix());
                buf.Append("否则");
                buf.AppendLine();
                buf.Append(ElsePart.ToString());
                buf.AppendLine();
            }
            return buf.ToString();
        }

        //public override CodePostion Postion
        //{
        //    get
        //    {
        //        return Parts[0].Postion;
        //    }
        //}
        #endregion

        public class StmtIfTrue : Stmt
        {
            public LexToken KeyToken { get; set; }
            public Exp Condition { get; set; }
            public StmtBlock Body { get; set; }
            public Label CurrentLabel { get; set; }
            public Label NextLabel { get; set; }
            public Label EndLabel { get; set; }

            public override void DoAnaly()
            {
                Condition.IsTopExp = true;
                Condition = AnalyCondition(Condition, KeyToken.Position);
                Body.ProcContext = this.ProcContext;
                Body.Analy();
            }

            public override void Emit( )
            {
                IL.MarkLabel(CurrentLabel);
                Condition.Emit();
                EmitHelper.LoadInt(IL, 1);
                IL.Emit(OpCodes.Ceq);
                IL.Emit(OpCodes.Brfalse, NextLabel);
                Body.Emit();
                IL.Emit(OpCodes.Br, EndLabel);
            }
            
            #region 覆盖
            public override string ToString()
            {
                StringBuilder buf = new StringBuilder();
                buf.Append(getStmtPrefix());
                buf.AppendFormat("{0}{1}", KeyToken.ToCode(), this.Condition.ToString());
                buf.AppendLine();
                buf.Append(Body.ToString());
                buf.AppendLine();
                return buf.ToString();
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
}
