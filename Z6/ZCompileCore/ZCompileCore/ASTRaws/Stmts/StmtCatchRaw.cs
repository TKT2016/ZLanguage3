using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class StmtCatchRaw:StmtRaw
    {
        public LexTokenText CatchToken { get; set; }
        public LexTokenText ExceptionTypeVarToken { get; set; }
        public StmtBlockRaw CatchBody { get; set; }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(GetStmtPrefix());
            //buf.AppendFormat("{0}({1}:{2})", CatchToken.GetText(), ExceptionTypeToken.GetText(), ExceptionVarToken.GetText());
            buf.AppendFormat("{0}({1})", CatchToken.Text, ExceptionTypeVarToken.Text);
            buf.AppendLine();
            buf.Append(CatchBody.ToString());
            return buf.ToString();
        }
    }
}
