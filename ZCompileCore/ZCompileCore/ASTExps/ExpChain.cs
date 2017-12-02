using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.ASTExps;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileCore.Parsers;

namespace ZCompileCore.ASTExps
{
    public class ExpChain:Exp
    {
        List<object> RawElements { get; set; }
        public ExpChain()
        {
            RawElements = new List<object>();
        }

        public void Add(LexToken tok)
        {
            RawElements.Add(tok);
        }

        public void Add(ExpBracket bracket)
        {
            RawElements.Add(bracket);
        }

        public void Add(ExpLiteral literal)
        {
            RawElements.Add(literal);
        }

        public override Exp Analy()
        {
            //Console.WriteLine(this.ToString());
            ChainParser parser = new ChainParser();
            //if (RawElements.Count == 3 && RawElements[0].ToString().IndexOf( "子弹管理器")!=-1)
            //{
            //    Console.WriteLine("子弹管理器加入ZD");
            //}
           
            Exp exp = parser.Parse(RawElements, this.ExpContext);
            var exp2 =  exp.Analy();
            return exp2;
        }

        public int SubCount
        {
            get
            {
                return this.RawElements.Count;
            }
        }

        public override void SetContext(ContextExp context)
        {
            this.ExpContext = context;
            foreach (var item in this.RawElements)
            {
                if (item is Exp)
                {
                    (item as Exp).SetContext(context);
                }
            }
        }

        public override void Emit()
        {
            throw new NotImplementedException();
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }

        public override string ToString()
        {
            return string.Join("", RawElements.Select(p => ObjToString(p)));
        }

        private string ObjToString(object obj)
        {
            if (obj is LexToken)
                return (obj as LexToken).GetText();
            return obj.ToString();
        }
    }
}
