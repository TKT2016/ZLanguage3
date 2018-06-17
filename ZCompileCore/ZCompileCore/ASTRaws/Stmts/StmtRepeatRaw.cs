using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class StmtRepeatRaw:StmtRaw
    {
        public LexTokenText RepeatToken { get; set; }
        public LexTokenText TimesToken { get; set; }
        public ExpRaw TimesExp { get; set; }
        public StmtBlockRaw RepeatBody { get; set; }

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            buff.AppendFormat("重复{0}次\n", TimesExp);
            buff.AppendLine(RepeatBody.ToString());
            return buff.ToString();
        }
    }
}
