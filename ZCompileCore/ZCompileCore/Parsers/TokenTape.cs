using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileKit.Collections;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

namespace ZCompileCore.Parsers
{
    public class TokenTape : ArrayTape<Token>
    {
        ContextFile fileContext;

        public TokenTape(List<Token> tokens, ContextFile fileContext)
            : base(tokens.ToArray(), Token.EOF)
        {
            this.fileContext = fileContext;
        }

        public bool isBracketEnd(TokenKind kind)
        {
            return (kind == TokenKind.EOF || kind == TokenKind.Semi 
                || kind == TokenKind.RBS
                || kind == TokenKind.NewLine
                );// || isNewLine());
        }

        public TokenKind CurrentKind
        {
            get { return this.Current.Kind; }
        }

        public void skipToSemi()
        {
            while (CurrentKind != TokenKind.Semi && CurrentKind != TokenKind.EOF)
            {
                MoveNext();
            }
            if (CurrentKind == TokenKind.Semi)
            {
                MoveNext();
            }
        }

        public bool Match(TokenKind tokKind)
        {
            if (CurrentKind != tokKind)
            {
                error(this.Current, this.Current.ToCode() + "不正确,应该是" + Token.GetTextByKind(tokKind));
                return false;
            }
            else
            {
                MoveNext();
                return true;
            }
        }

        public void error(Token tok, string str)
        {
           this.fileContext.Errorf(tok.Position, str);
        }

        public void error(string str)
        {
            error(this.Current, str);
        }

        public void report(string str, int color = 1)
        {
            return;
            /*
            ConsoleColor c = Console.ForegroundColor;
            if (color == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (color == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (color == 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (color == 4)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine("DEBUG:" + this.Index + " [" + this.Current.Line + "," + this.Current.Col + "]:" + this.Current.ToCode() + " --- " + str);
            Console.ForegroundColor = c;*/
        }

    }
}
