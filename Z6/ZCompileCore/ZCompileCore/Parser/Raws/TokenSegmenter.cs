using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Collections;
using ZCompileCore;
using ZCompileNLP;

namespace ZCompileCore.Parsers.Raws
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
            string src = token.Text;
            //if (segementer.IWDict.ContainsText(src))
            //{
            //    return new LexToken[]{ token};
            //}
            //{
            //    string[] strarr3 = segementer.Cut("之和");
            //    Debugr.WriteLine(strarr3.Length);
            //    string[] strarr2 = segementer.Cut("之差");
            //    Debugr.WriteLine(strarr2.Length);
            //}

            string[] strarr = segementer.Cut(src);
            if (strarr.Length == 1) return new LexToken[] { token };
            List<LexToken> list = new List<LexToken>();
            int col = token.Col;
            int line = token.Line;
            foreach (string text in strarr)
            {
                //LexToken tok = null;
                if (StringHelper.IsInt(text))
                {
                    LexToken tok = new LexTokenLiteral(line, col, TokenKindLiteral.LiteralInt, text);
                    list.Add(tok);
                }
                else if (StringHelper.IsFloat(text))
                {
                    LexToken tok = new LexTokenLiteral(line, col, TokenKindLiteral.LiteralFloat, text);
                    list.Add(tok);
                }
                else if (text.Length==2)
                {
                    if (segementer.ContainerWord(text[0].ToString()) || segementer.ContainerWord(text[1].ToString()))
                    {
                        LexToken tok1 = new LexTokenText(line, col, text[0].ToString());
                        list.Add(tok1);
                        LexToken tok2 = new LexTokenText(line, col, text[1].ToString());
                        list.Add(tok2);
                    }
                    else
                    {
                        LexToken tok = new LexTokenText(line, col, text);
                        list.Add(tok);
                    }
                }
                else
                {
                    LexToken tok = new LexTokenText(line, col, text);
                    //new LexToken() { Line = line, Col = col, Kind = token.Kind, Text = text };
                    list.Add(tok);
                }


                col += text.Length;
            }
            return list.ToArray();
        }
       
    }
}
