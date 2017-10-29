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
using ZCompileKit;
using ZCompileDesc.ZMembers;
using ZCompileKit.Tools;
using ZCompileCore.Parsers;

namespace ZCompileCore.AST
{
    public class ExpCall:Exp
    {
        public List<Exp> Elements { get; set; }

        public override Exp[] GetSubExps()
        {
            return Elements.ToArray();
        }

        public ExpCall()
        {
            Elements = new List<Exp>();
        }

        public override void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            foreach (var expr in this.Elements)
            {
                expr.SetContext(context);
            }
        }

        public override Exp Analy( )
        {
            ExpCallParser callPaser = new ExpCallParser(this.Elements, this.ExpContext,this);
            var expNew = callPaser.Parse();
            return expNew.Analy();
        }

        public override void Emit()
        {
            throw new CompileCoreException();
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
