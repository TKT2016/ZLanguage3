using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;

namespace ZCompileCore.Lex
{
    public class LexTokenLiteral:LexToken
    {
        private string _Text;
        public override string Text { get { return _Text; } }
        public TokenKindLiteral Kind { get; set; }

        public LexTokenLiteral( int line, int col, TokenKindLiteral kind,string text)
        {
            _Text = text;
            Kind = kind;
            Line = line;
            Col = col;
        }

        public override string ToString()
        {
            return string.Format("({0},{1}){2}:{3}", Line, Col, Kind,Text);
        }

        public override string ToCode()
        {
            if (this.Kind == TokenKindLiteral.LiteralString)
            {
                return "\"" + this.Text +"\"";
            }
            else if (this.Kind == TokenKindLiteral.NULL)
            {
                return "空";
            }
            else if (this.Kind == TokenKindLiteral.False)
            {
                return "否";
            }
            else if (this.Kind == TokenKindLiteral.True)
            {
                return "是";
            }
            else
            {
                return this.Text ;
            }
        }
    }
}
