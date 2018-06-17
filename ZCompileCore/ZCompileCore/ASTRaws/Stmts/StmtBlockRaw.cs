using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Lex;

namespace ZCompileCore.ASTRaws
{
    public class StmtBlockRaw : StmtRaw
    {
        //public List<StmtRaw> Stmts = new List<StmtRaw>();
        public List<LineTokenCollection> LineTokens = new List<LineTokenCollection>();

        public override string ToString()
        {
            StringBuilder buff = new StringBuilder();
            string prefix = GetStmtPrefix();
            foreach(var  linet in LineTokens)
            {
                buff.Append(prefix);
                var texts = linet.ToList().Select(p=>p.Text);
                buff.AppendLine(string.Join("",texts));
            }
            //return string.Join(Environment.NewLine, StmtList);
            return buff.ToString();
        } 
    }
}
