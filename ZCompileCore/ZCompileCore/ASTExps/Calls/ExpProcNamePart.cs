using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileDesc.Descriptions;
using ZLangRT.Utils;

namespace ZCompileCore.AST
{
    public class ExpProcNamePart:Exp
    {
        public LexToken PartNameToken { get; set; }
        public string PartName{ get;private set; }

        public ExpProcNamePart(LexToken PartNameToken)
        {
            this.PartNameToken = PartNameToken;
        }

        public override Exp Analy( )
        {
            PartName = PartNameToken.GetText();
            return this;
        }

        #region 覆盖

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override string ToString()
        {
            return PartNameToken.GetText();
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
