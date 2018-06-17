
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.AST.Exps
{
    public abstract class ExpTypeSingleBase : ExpTypeBase
    {
        protected LexToken VarToken;
        protected string VarName;

        public ExpTypeSingleBase(ContextExp expContext)
            : base(expContext)
        {
           
        }

        public override LexToken GetMainToken()
        {
            return VarToken;
        } 

        public override string ToString()
        {
            return VarToken.Text;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }
    }
}
