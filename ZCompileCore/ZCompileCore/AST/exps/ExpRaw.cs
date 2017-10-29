using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parser;
using ZCompileCore.Parsers;
using ZCompileDesc.Words;

namespace ZCompileCore.AST
{
    public class ExpRaw : Exp
    {
        public List<Token> RawTokens { get; set; }

        public ExpRaw()
        {
            RawTokens = new List<Token>();
        }

        public override Exp Parse()
        {
            //var wordTree = new ExpParseDictionary(this.ExpContext);
            //WordSegmenter segmenter = new WordSegmenter(wordTree);
            //List<Token> tokens = new List<Token>();
            //foreach (var tok in RawTokens)
            //{
            //    if (tok.Kind == TokenKind.Ident)
            //    {
            //        Token[] newTokens = segmenter.Split(tok);
            //        tokens.AddRange(newTokens);
            //    }
            //    else if (tok.Kind != TokenKind.NewLine)
            //    {
            //        tokens.Add(tok);
            //    }
            //}
            List<Token> tokens = WordSegment();
            ExpParser parser = new ExpParser();
            Exp exp = parser.Parse(tokens,this.FileContext);
            exp.SetContext(this.ExpContext);
            return exp;
        }

        public List<Token> WordSegment()
        {
            var wordTree = new ExpParseDictionary(this.ExpContext);
            WordSegmenter segmenter = new WordSegmenter(wordTree);
            List<Token> tokens = new List<Token>();
            foreach (var tok in RawTokens)
            {
                if (tok.Kind == TokenKind.Ident)
                {
                    Token[] newTokens = segmenter.Split(tok);
                    tokens.AddRange(newTokens);
                }
                else if (tok.Kind != TokenKind.NewLine)
                {
                    tokens.Add(tok);
                }
            }
            return tokens;
        }

        #region 覆盖方法

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
            return string.Join("", RawTokens.Select(p=>p.GetText()));
        }
        #endregion
    }
}
