using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class StmtIfRaw : StmtRaw
    {
        public LexTokenText IfToken;
        public ExpRaw IfExp;
        public List<IfElseStmt> ElseIfParts = new List<IfElseStmt>();
        public ElseStmt ElsePart;

        public class IfElseStmt
        {
            public LexTokenText KeyToken { get; set; }
            public ExpRaw ElseIfExp;
            public StmtBlockRaw Body { get; set; }

            public override string ToString()
            {
                //return Raw.ToString();
                StringBuilder buf = new StringBuilder();
                //buf.Append(GetStmtPrefix());
                buf.AppendFormat("{0}{1}", KeyToken.ToCode(), this.ElseIfExp.ToString());
                buf.AppendLine();
                buf.Append(Body.ToString());
                buf.AppendLine();
                return buf.ToString();
            }
        }

        public class ElseStmt
        {
            public LexTokenText KeyToken { get; set; }
            public StmtBlockRaw Body { get; set; }

            public override string ToString()
            {
                //return Raw.ToString();
                StringBuilder buf = new StringBuilder();
                //buf.Append(GetStmtPrefix());
                buf.AppendFormat("否则");
                buf.AppendLine();
                buf.Append(Body.ToString());
                buf.AppendLine();
                return buf.ToString();
            }
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            foreach (var tpart in ElseIfParts)
            {
                buf.AppendLine(tpart.ToString());
            }
            if (ElsePart != null)
            {
                buf.Append(ElsePart.ToString());
            }
            return buf.ToString();
        }
    }
}
