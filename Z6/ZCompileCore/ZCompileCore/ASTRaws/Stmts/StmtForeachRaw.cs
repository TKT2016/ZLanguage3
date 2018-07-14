using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class StmtForeachRaw:StmtRaw
    {
        public LexTokenText LoopToken { get; set; }
        public LexTokenText EachToken { get; set; }
        public ExpRaw ListExp { get; set; }
        public LexTokenText ItemToken { get; set; }
        public StmtBlockRaw Body { get; set; }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(GetStmtPrefix());
            buf.AppendFormat("循环每一个( {0},{1} )", this.ListExp.ToString(),
                ItemToken.ToCode());
            buf.AppendLine();
            buf.Append(Body.ToString());
            return buf.ToString();
        }
    }
}
