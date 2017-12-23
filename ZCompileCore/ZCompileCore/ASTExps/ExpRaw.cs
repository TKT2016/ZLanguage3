using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parser;
using ZCompileCore.Parsers;

namespace ZCompileCore.AST
{
    public class ExpRaw : Exp
    {
        public List<LexToken> RawTokens { get; set; }

        public ExpRaw()
        {
            RawTokens = new List<LexToken>();
        }

        public override Exp Parse()
        {
            //if (RawTokens.Count > 0 && RawTokens[0].GetText().StartsWith("清除出界子弹"))
            //{
            //    Console.WriteLine("清除出界子弹");
            //}

            List<LexToken> tokens = Seg();
            ExpParser parser = new ExpParser();
            Exp exp = parser.Parse(tokens,this.FileContext);
            exp.SetContext(this.ExpContext);
            return exp;
        }

        public List<LexToken> Seg()
        {
            TokenSegmenter segmenter = new TokenSegmenter(this.ProcContext.ProcSegmenter);
            List<LexToken> tokens = new List<LexToken>();
            foreach (var tok in RawTokens)
            {
                if (tok.Kind == TokenKind.Ident)
                {
                    LexToken[] newTokens = segmenter.Split(tok);
                    tokens.AddRange(newTokens);
                }
                else if (tok.Kind != TokenKind.NewLine)
                {
                    tokens.Add(tok);
                }
            }
            //if (tokens.Count > 1)
            //{
            //    Console.WriteLine(string.Join(" ", RawTokens.Select(P => P.GetText())));
            //    Console.WriteLine(string.Join(" ", tokens.Select(P => P.GetText())));
            //}
            return tokens;
        }

        #region 覆盖方法

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
            return string.Join("", RawTokens.Select(p=>p.GetText()));
        }
        #endregion
    }
}
