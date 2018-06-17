using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.ASTRaws
{
    public class StmtCallRaw:StmtRaw
    {
        public ExpRaw CallExp { get; set; }

        public override string ToString()
        {
            return CallExp.ToString() + ";";
        }
    }
}
