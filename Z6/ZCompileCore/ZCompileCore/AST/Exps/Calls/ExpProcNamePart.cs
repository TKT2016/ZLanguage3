using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZLangRT.Utils;

namespace ZCompileCore.AST
{
    public class ExpProcNamePart:Exp
    {
        public LexToken PartNameToken { get; set; }
        public string PartName{ get;private set; }

        public ExpProcNamePart(ContextExp expContext, LexToken PartNameToken)
            : base(expContext)
        {
            this.PartNameToken = PartNameToken;
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            PartName = PartNameToken.Text;
            IsAnalyed = true;
            return this;
        }

        #region 覆盖

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        //public override Exp[] GetSubExps()
        //{
        //    return new Exp[] { };
        //}

        public override string ToString()
        {
            return PartNameToken.Text;
        }

        public override CodePosition Position
        {
            get
            {
                return PartNameToken.Position;
            }
        }
        #endregion
    }
}
