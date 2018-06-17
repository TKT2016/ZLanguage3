using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.ASTRaws
{
    public abstract class StmtRaw
    {
        public int Deep { get; set; }

        protected string GetStmtPrefix()
        {
            StringBuilder buff = new StringBuilder();
            int temp = Deep + 1;
            while (temp > 0)
            {
                buff.Append("  ");
                temp--;
            }
            return buff.ToString();
        }
    }
}
