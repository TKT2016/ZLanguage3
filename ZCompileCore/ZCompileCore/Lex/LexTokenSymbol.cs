using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Collections;

namespace ZCompileCore.Lex
{
    public class LexTokenSymbol:LexToken
    {
        public static PairDict<string, TokenKindSymbol> Dict{get;private set;}
        public static readonly LexTokenSymbol EOF = new LexTokenSymbol() { Kind = TokenKindSymbol.EOF };

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
                    return this.Text;
                }
            }
        }
        public TokenKindSymbol Kind { get; set; }

        private LexTokenSymbol()
        {

        }

        public LexTokenSymbol(int line ,int col,TokenKindSymbol kind)
        {
            Kind = kind;
            Line = line;
            Col = col;
        }

        static LexTokenSymbol()
        {
            Dict = new PairDict<string, TokenKindSymbol>();

            Dict.Add("+", TokenKindSymbol.ADD);
            Dict.Add("-", TokenKindSymbol.SUB);
            Dict.Add("/", TokenKindSymbol.DIV);
            Dict.Add("*", TokenKindSymbol.MUL);

            Dict.Add("=", TokenKindSymbol.Assign);
            Dict.Add("=>", TokenKindSymbol.AssignTo);

           
            Dict.Add("==", TokenKindSymbol.EQ);
            Dict.Add("!=", TokenKindSymbol.NE);
            Dict.Add(">=", TokenKindSymbol.GE);
            Dict.Add(">", TokenKindSymbol.GT);
            Dict.Add("<", TokenKindSymbol.LT);
            Dict.Add("<=", TokenKindSymbol.LE);

            Dict.Add("(", TokenKindSymbol.LBS);
            Dict.Add(")", TokenKindSymbol.RBS);
            Dict.Add(",", TokenKindSymbol.Comma);
            Dict.Add(";", TokenKindSymbol.Semi);
            //Dict.Add("::", TokenKind.Colond);
            Dict.Add(":", TokenKindSymbol.Colon);

            Dict.Add("并且", TokenKindSymbol.AND);
            Dict.Add("或者", TokenKindSymbol.OR);
        }

        public static string GetTextByKind(TokenKindSymbol tokKind)
        {
            if (Dict.ContainsV(tokKind))
            {
                return Dict.GetK(tokKind);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("({0},{1}){2}:{3}", Line, Col, Kind,Text);
        }

        public override string ToCode()
        {
            if (this.Kind == TokenKindSymbol.Unknow)
            {
                return this.Text;
            }
            else
            {
                if(Dict.ContainsV(this.Kind))
                {
                    return Dict.GetK(this.Kind);
                }
                else
                {
                    return this.Text;
                }            
            }
        }
    }
}
