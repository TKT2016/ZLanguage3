using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.Lex
{
    public class LineTokenCollection
    {
        public int StartCol { get;private set; }
        private List<LexToken> Tokens { get; set; }

        public LineTokenCollection()
        {
            StartCol = 1;
            Tokens = new List<LexToken>();
        }

        public LexToken Get(int i)
        {
            return Tokens[i];
        }

        public List<LexToken> ToList()
        {
            return Tokens;
        }

        public void Add(LexToken tok)
        {
            if (Tokens.Count==0)
            {
                StartCol = tok.Col;
            }
            Tokens.Add(tok);
        }

        public int Count
        {
            get
            {
                return Tokens.Count;
            }
        }

        public LexToken FirstToken
        {
            get
            {
                if(this.Count>0)
                {
                    return Tokens[0];
                }
                return null;
            }
        }

        public LexToken SecondToken
        {
            get
            {
                if (this.Count >= 2)
                {
                    return Tokens[1];
                }
                return null;
            }
        }

        public LexToken LastToken
        {
            get
            {
                if (this.Count > 0)
                {
                    return Tokens[this.Count-1];
                }
                return null;
            }
        }

        public bool Has(TokenKindSymbol kind)
        {
            foreach(var item in Tokens)
            {
                if(item.IsKind(kind))
                {
                    return true;
                }
            }
            return false;
        }

        public List<LexToken> GetSubs(int startIndex)
        {
            var packagesTokens = new List<LexToken>();
            for (; startIndex < this.Tokens.Count; startIndex++)
            {
                packagesTokens.Add(this.Tokens[startIndex]);
            }
            return packagesTokens;
        }

        public override string ToString()
        {
            List<string> buff = new List<string>();
            foreach (var item in Tokens)
            {
                buff.Add(item.Text);
            }
            string str = string.Join(" ", buff);
            return str;
        }
    }
}
