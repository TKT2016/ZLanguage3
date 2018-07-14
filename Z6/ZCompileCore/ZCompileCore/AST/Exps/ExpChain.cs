using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.AST.Exps;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileCore.Parsers;
using ZCompileCore.Parsers.Exps;

namespace ZCompileCore.AST.Exps
{
    public class ExpChain:Exp
    {
        public List<object> RawElements { get;private set; }

        public ExpChain(ContextExp expContext)
            : base(expContext)
        {
            RawElements = new List<object>();
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public void Add(LexToken tok)
        {
            RawElements.Add(tok);
        }

        public void Add(ExpBracket bracket)
        {
            bracket.ParentExp = this;
            RawElements.Add(bracket);

        }

        public void Add(ExpLiteral literal)
        {
            literal.ParentExp = this;
            RawElements.Add(literal);         
        }

        public bool IsAssignTo { get; set; }

        public override Exp Analy()
        {
            //if (this.IsAnalyed) return this;
            ChainParser parser = new ChainParser();
            //if(DebugStatics.IsInDebug)
            //{
            //    DebugStatics.TempFunc(RawElements);
            //}
            Exp exp = parser.Parse(RawElements, this.ExpContext, IsAssignTo); //结果已经Analy过
            Exp exp2 = exp.Analy();
            IsAnalyed = true;
            return exp2;
        }

        public int SubCount
        {
            get
            {
                return this.RawElements.Count;
            }
        }

        public override void Emit()
        {
            throw new CCException();
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
                return (obj as LexToken).Text;
            return obj.ToString();
        }
    }
}
