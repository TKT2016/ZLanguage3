using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    public class ExpBracketWrapOne : ExpBracket
    {
        private Exp _VarExp;
        public Exp VarExp { get { return _VarExp; } set { _VarExp = value; _VarExp.ParentExp = this; } }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { VarExp };
        }

        public ExpBracketWrapOne(ContextExp expContext,Exp exp, bool isAnalyed):base(expContext)
        {
            VarExp = exp;
            IsAnalyed = isAnalyed;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            VarExp.SetParent(this);
        }

        public override ZType RequireType
        {
            get { 
                return this.VarExp.RequireType; 
            }
            set { 
                this.VarExp.RequireType = value;
            }
        }

        public override void AddInnerExp(Exp exp)
        {
            throw new CCException();
        }

        public override Exp Analy( )
        {
            //if (IsAnalyed) return this;
            Exp newExp = VarExp.Analy();
            VarExp = newExp;
            this.AnalyCorrect = true;
            RetType = VarExp.RetType;
            return this;  
        }

        public override void Emit()
        {
            VarExp.Emit();
        }

        public override List<ZType> GetInnerTypes()
        {
            List<ZType> list = new List<ZType>();
            return list;
        }

        public override ZBracketCall GetCallDesc()
        {
            ZBracketCall zbc = new ZBracketCall();

            var type = VarExp.RetType;
            ZArgCall zargdesc = new ZArgCall() { ZArgType = VarExp.RetType };
            zbc.Add(zargdesc);
            return zbc;
        }

        public override int Count
        {
            get
            {
                return 1;
            }
        }

        #region 覆盖
        public override string ToString()
        {
            return ("( " + VarExp.ToString() + " )");
        }

        public override CodePosition Position
        {
            get
            {
                return VarExp.Position;
            }
        }
        #endregion
    }
}
