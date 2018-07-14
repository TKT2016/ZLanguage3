using ZCompileCore.Lex;

using ZCompileCore.Tools;
using System.Reflection.Emit;
using ZCompileDesc.Descriptions;
using System.Reflection;
using ZCompileCore;
using ZCompileCore.Contexts;


namespace ZCompileCore.AST.Exps
{
    public class ExpNameValue : Exp
    {
        public LexToken NameToken { get; set; }
        private Exp _ValueExp;
        public Exp ValueExp { get { return _ValueExp; } set { _ValueExp = value; _ValueExp.ParentExp = this; } }
        public string ArgName { get { return NameToken.Text; } }

        public ExpNameValue(ContextExp expContext, LexToken left, Exp right)
            : base(expContext)
        {
            NameToken = left;
            ValueExp = right;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            ValueExp = ValueExp.Analy();
            RetType = ValueExp.RetType;
            IsAnalyed = true;
            return this;
        }

        #region Emit
        public override void Emit()
        {
            ValueExp.Emit();
        }

        #endregion

        #region 覆盖

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            ValueExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ValueExp };
        }

        public override string ToString()
        {
            return NameToken.Text + ":" + ValueExp.ToString();
        }

        public override CodePosition Position
        {
            get
            {
                return NameToken.Position;
            }
        }
        #endregion
    }
}
