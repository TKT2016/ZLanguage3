using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers.Exps;

namespace ZCompileCore.Parsers.Raws
{
    public class ExpRawParser
    {
        private ContextExp ExpContext;

        public Exp Parse(ExpRaw rawExp,ContextExp expContext)
        {
            ExpContext = expContext;
            List<LexToken> tokens = Seg(rawExp.RawTokens);
            ExpParser parser = new ExpParser();

            Exp exp = parser.Parse(tokens, ExpContext);
            Exp exp2 = exp.Analy();
            return exp2;
        }

        private List<LexToken> Seg(List<LexToken> rawTokens)
        {
            TokenSegmenter segmenter = new TokenSegmenter(this.ExpContext.ProcContext.ProcSegmenter);
            List<LexToken> tokens = new List<LexToken>();
            foreach (var tok in rawTokens)
            {
                //if (tok.Text.StartsWith("之差") || tok.Text.StartsWith("之"))
                //{
                //    Debugr.WriteLine(tok.Text);
                //}
                if (tok.IsKind(TokenKindKeyword.Ident))
                {
                    LexToken[] newTokens = segmenter.Split(tok);
                    tokens.AddRange(newTokens);
                }
                else //if (tok.Kind != TokenKindSymbol.NewLine)
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
    }
}
