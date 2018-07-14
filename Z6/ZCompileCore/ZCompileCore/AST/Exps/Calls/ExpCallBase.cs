using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    public abstract class ExpCallBase:Exp
    {
        public ExpCallBase(ContextExp expContext)
            : base(expContext)
        {

        }

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

        public ExpCallSingle(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override string ToString()
        {
            return VarToken.Text;
        }
    }

    public class ExpCallDouble : ExpCallBase
    {
        protected Exp ArgExp;
        private Exp _SrcExp;
        public Exp SrcExp { get { return _SrcExp; } set { _SrcExp = value; _SrcExp.ParentExp = this; } }

        public ExpCallDouble(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            SrcExp.SetParent(this);
            ArgExp.SetParent(this);
        }

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
