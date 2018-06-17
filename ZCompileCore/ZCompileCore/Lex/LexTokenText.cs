using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Collections;

namespace ZCompileCore.Lex
{
    public class LexTokenText:LexToken
    {
        public static PairDict<string, TokenKindKeyword> Dict{get;private set;}

        private string _Text;
        public override string Text
        {
            get
            {
                if (Dict.ContainsV(this.Kind))
                {
                    return Dict.GetK(this.Kind);
                }
                else
                {
                    return _Text;
                }
            }
        }
        public TokenKindKeyword Kind { get; set; }

        public LexTokenText(int line ,int col,string text)
        {
            _Text = text;
            Kind =  TokenKindKeyword.Ident;
            Line = line;
            Col = col;
        }

        public LexTokenText(int line, int col, TokenKindKeyword kind)
        {
            Kind = kind;
            Line = line;
            Col = col;
        }

        public void CheckKind()
        {
            if (!string.IsNullOrWhiteSpace(this._Text))
            {
                if (Dict.ContainsK(this._Text))
                {
                    var kind = Dict.GetV(this._Text);
                    this.Kind = kind;
                }
            }
        }

        static LexTokenText()
        {
            Dict = new PairDict<string, TokenKindKeyword>();

            //Dict.Add("并且", TokenKindKeyword.AND);
            //Dict.Add("或者", TokenKindKeyword.OR);
            Dict.Add("是", TokenKindKeyword.True);
            Dict.Add("否", TokenKindKeyword.False);
            Dict.Add("的", TokenKindKeyword.DE);
            Dict.Add("如果", TokenKindKeyword.IF);
            Dict.Add("否则", TokenKindKeyword.ELSE);
            Dict.Add("否则如果", TokenKindKeyword.ELSEIF);
            //Dict.Add("循环当", TokenKind.While);
            Dict.Add("重复", TokenKindKeyword.Repeat);
            Dict.Add("当", TokenKindKeyword.Dang);
            //Dict.Add("次", TokenKind.Times);
            Dict.Add("循环", TokenKindKeyword.Loop);
            Dict.Add("每一个", TokenKindKeyword.Each);
            Dict.Add("捕捉", TokenKindKeyword.Catch);
            Dict.Add("说明:", TokenKindKeyword.Caption);
            Dict.Add("第", TokenKindKeyword.DI);
            Dict.Add("约定", TokenKindKeyword.Enum);
            Dict.Add("新的", TokenKindKeyword.NewDefault);
            Dict.Add("个", TokenKindKeyword.Ge);
        }

        public override string ToString()
        {
            return string.Format("({0},{1}){2}:{3}", Line, Col, Kind,Text);
        }

        public override string ToCode()
        {
            if (Dict.ContainsV(this.Kind))
            {
                return Dict.GetK(this.Kind);
            }
            else
            {
                return _Text;
            }
        }

        public static string GetTextByKind(TokenKindKeyword kind)
        {
            if (kind == TokenKindKeyword.Ident)
            {
                return null;
            }
            if (Dict.ContainsV(kind))
            {
                return Dict.GetK(kind);
            }
            return null;
        }

        //public static TokenKindKeyword GetKindByText(string text)
        //{
        //    if (Dict.ContainsK(text))
        //    {
        //        return Dict.GetV(text);
        //    }
        //    return TokenKindKeyword.Ident;
        //}

        //public string GetText()
        //{
        //    if (Dict.Containsv(this.Kind))
        //    {
        //        return Dict.Getk(this.Kind);
        //    }
        //    else
        //    {
        //        return this.Text;
        //    }         
        //}
       
    }
}
