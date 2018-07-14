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
    public abstract class ExpVarBase : Exp , IEmitSet// ExpIdent
    {
        public LexToken VarToken { get; protected set; }
        public string VarName { get; protected set; }


        public ExpVarBase(ContextExp expContext)
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

        //public virtual void SetAssigned(ZType retType)
        //{
        //    RetType = retType;
        //}

        public abstract bool CanWrite { get; }

        public abstract void EmitSet(Exp valueExp);

        public override string ToString()
        {
            return VarToken.Text;
        }

        public override CodePosition Position
        {
            get
            {
                return VarToken.Position;
            }
        }

        protected void EmitValueExp(Exp valueExp)
        {
            valueExp.RequireType = this.RetType;
            valueExp.Emit();
        }
    }
}
