using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpCallBase:Exp
    {
        //protected ZCMethodInfo Method;

        protected void EmitArgExp(ZLParamInfo parameter, Exp argExp)
        {
            argExp.RequireType = parameter.ZParamType;
            argExp.Emit();
        }

        protected void EmitArgExp(ZCParamInfo parameter, Exp argExp)
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
        //protected ZLMethodInfo[] Methods;
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
