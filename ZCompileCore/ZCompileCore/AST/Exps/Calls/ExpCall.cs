using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT;
using ZCompileDesc.Descriptions;
using ZCompileCore;
using ZCompileCore.Parsers;
using ZCompileCore.Parsers.Exps;

namespace ZCompileCore.AST.Exps
{
    public class ExpCall:Exp
    {
        public List<Exp> Elements { get; set; }

        public override Exp[] GetSubExps()
        {
            return Elements.ToArray();
        }

        public ExpCall(ContextExp expContext, IEnumerable<Exp> elements)
            : base(expContext)
        {
            Elements = elements.ToList();
            foreach (Exp sub in Elements)
            {
                sub.ParentExp = this;
            }
            //this.SetContextExp(expContext);
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            foreach (var item in Elements)
            {
                item.SetParent(this);
            }
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            ExpCallParser callPaser = new ExpCallParser( this.ExpContext,this);
            
            var expNew = callPaser.Parse(this.Elements);
            IsAnalyed = true;
            return expNew.Analy();
        }

        public override void Emit()
        {
            throw new CCException();
        }

        #region 辅助
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            List<string> tempcodes = new List<string>();
            foreach (var expr in Elements)
            {
                if (expr != null)
                {
                    tempcodes.Add(expr.ToString());
                }
                else
                {
                    tempcodes.Add(" ");
                }
            }
            buf.Append(string.Join("", tempcodes));
            return buf.ToString();
        }

        public override CodePosition Position
        {
            get
            {
                return Elements[0].Position; ;
            }
        }
        #endregion
    }
}
