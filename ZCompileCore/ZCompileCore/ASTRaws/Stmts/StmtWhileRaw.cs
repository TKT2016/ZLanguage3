using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class StmtWhileRaw:StmtRaw
    {
        public LexTokenText DangToken { get; set; }
        public LexTokenText RepeatToken { get; set; }
        public ExpRaw ConditionExp { get; set; }
        public StmtBlockRaw WhileBody { get; set; }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.Append(GetStmtPrefix());
            buff.AppendFormat("当{0}重复\n", ConditionExp);
            buff.AppendLine(WhileBody.ToString());
            return buff.ToString();
        }
    }
}
