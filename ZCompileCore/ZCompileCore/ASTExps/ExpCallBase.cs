using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpCallBase:Exp
    {
        protected ZMethodInfo Method;

        protected void EmitArgExp(ZParam parameter, Exp argExp)
        {
            argExp.RequireType = parameter.ZParamType;
            argExp.Emit();
        }
    }

    public class ExpCallSingle : ExpCallBase
    {
        protected LexToken VarToken;
        protected string VarName;

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override string ToString()
        {
            return VarToken.GetText();
        }
    }

    public class ExpCallDouble : ExpCallBase
    {
        protected ZMethodInfo[] Methods;
        protected Exp ArgExp;
        public Exp SrcExp;

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ArgExp };
        }

        public override string ToString()
        {
            return SrcExp.ToString();
        }
    }
}
