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
    public class ExpBracketTagNew : ExpBracket
    {
        public ExpTagNew TagExp{ get;  set; }

        public ExpBracketTagNew(ContextExp expContext, LexToken leftBracketToken, LexToken rightBracketToken, ExpTagNew tagExp)
            : base(expContext)
        {
            LeftBracketToken = leftBracketToken;
            RightBracketToken = rightBracketToken;
            TagExp = tagExp;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            TagExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override void AddInnerExp(Exp exp)
        {
            throw new CCException();
        }

        public override Exp Analy( )
        {
            if (IsAnalyed) return this;
            this.AnalyCorrect = true;
            RetType = ZLangBasicTypes.ZVOID;
            return this;  
        }

        public override void Emit()
        {
            return;
        }

        public override List<ZType> GetInnerTypes()
        {
            List<ZType> list = new List<ZType>();
            return list;
        }

        public override ZBracketCall GetCallDesc()
        {
            ZBracketCall zbc = new ZBracketCall();
            return zbc;
        }

        public override int Count
        {
            get
            {
                return 0;
            }
        }

        #region 覆盖
        public override string ToString()
        {
            return ("( )");
        }

        public override CodePosition Position
        {
            get
            {
                return LeftBracketToken!=null ?LeftBracketToken.Position:InneExps[0].Position;
            }
        }
        #endregion
    }
}
