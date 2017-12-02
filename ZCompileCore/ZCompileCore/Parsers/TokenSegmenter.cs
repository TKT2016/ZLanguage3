using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Lex;
using ZCompileKit.Tools;
using ZCompileDesc.Collections;
using ZCompileKit;
using ZNLP;

namespace ZCompileCore.Parser
{
   public class TokenSegmenter
    {
       UserWordsSegementer segementer;

       public TokenSegmenter(UserWordsSegementer segementer)
        {
            //Tree = tree;
            this.segementer = segementer;
        }

        public LexToken[] Split(LexToken token)
        {
            string src = token.GetText();
            //if (segementer.IWDict.ContainsText(src))
            //{
            //    return new LexToken[]{ token};
            //}

            string[] strarr = segementer.Cut(src);
            if (strarr.Length == 1) return new LexToken[] { token };
            List<LexToken> list = new List<LexToken>();
            int col = token.Col;
            int line = token.Line;
            foreach (string text in strarr)
            {
                LexToken tok = null;

                if (StringHelper.IsInt(text))
                {
                    tok = new LexToken() { Line = line, Col = col, Kind = TokenKind.LiteralInt, Text = text };
                }
                else if (StringHelper.IsFloat(text))
                {
                    tok = new LexToken() { Line = line, Col = col, Kind = TokenKind.LiteralFloat, Text = text };
                }
                else
                {
                    tok = new LexToken() { Line = line, Col = col, Kind = token.Kind, Text = text };
                }

                list.Add(tok);
                col += tok.GetText().Length;
            }
            return list.ToArray();
        }
       
    }
}
