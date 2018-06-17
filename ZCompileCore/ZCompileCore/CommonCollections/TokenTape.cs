using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Reports;

namespace ZCompileCore.CommonCollections
{
    public class TokenTape : ArrayTape<LexToken>
    {
        ContextFile fileContext;

        public TokenTape(IEnumerable<LexToken> tokens, ContextFile fileContext)
            : base(tokens.ToArray())
        {
            this.fileContext = fileContext;
        }

        //public bool isBracketEnd(TokenKindSymbol kind)
        //{
        //    return (kind == TokenKindSymbol.EOF || kind == TokenKindSymbol.Semi 
        //        || kind == TokenKindSymbol.RBS
        //       // || kind == TokenKindSymbol.NewLine
        //        );// || isNewLine());
        //}

        //public TokenKindSymbol CurrentKind
        //{
        //    get { return this.Current.Kind; }
        //}

        private bool CurrentIsKind(TokenKindSymbol kind)
        {
            return this.Current.IsKind(kind);
            //if( this.Current is LexTokenSymbol)
            //{
            //    LexTokenSymbol tokensymbol = (LexTokenSymbol)this.Current;
            //    if(tokensymbol.Kind==kind)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        public void skipToSemi()
        {
            while(true)
            {
                if(CurrentIsKind(TokenKindSymbol.Semi)  || CurrentIsKind(TokenKindSymbol.EOF))
                {
                     break;
                }
                else
                {
                    MoveNext();
                }
            }
            //if (CurrentIsKind(TokenKindSymbol.Semi))
            //{
            //    MoveNext();
            //}
            //while (CurrentIsKind( TokenKindSymbol.Semi && CurrentKind != TokenKindSymbol.EOF)
            //{
            //    MoveNext();
            //}
            //if (CurrentKind == TokenKindSymbol.Semi)
            //{
            //    MoveNext();
            //}
        }

        public bool Match(TokenKindSymbol tokKind)
        {
            if (!CurrentIsKind(tokKind)) 
            {
                error(this.Current, this.Current.ToCode() + "不正确,应该是" + LexTokenSymbol.GetTextByKind(tokKind));
                return false;
            }
            else
            {
                MoveNext();
                return true;
            }
        }

        public bool Match(TokenKindKeyword tokKind)
        {
            if (!this.Current.IsKind(tokKind))
            {
                error(this.Current, this.Current.ToCode() + "不正确,应该是" + LexTokenText.GetTextByKind(tokKind));
                return false;
            }
            else
            {
                MoveNext();
                return true;
            }
        }

        public void error(LexToken tok, string str)
        {
           this.fileContext.Errorf(tok.Position, str);
        }

        public void error(string str)
        {
            error(this.Current, str);
        }

        //public void report(string str, int color = 1)
        //{
        //    return;
        //    /*
        //    ConsoleColor c = Console.ForegroundColor;
        //    if (color == 1)
        //    {
        //        Console.ForegroundColor = ConsoleColor.White;
        //    }
        //    else if (color == 2)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Green;
        //    }
        //    else if (color == 3)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Yellow;
        //    }
        //    else if (color == 4)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Blue;
        //    }
        //    Console.WriteLine("DEBUG:" + this.Index + " [" + this.Current.Line + "," + this.Current.Col + "]:" + this.Current.ToCode() + " --- " + str);
        //    Console.ForegroundColor = c;*/
        //}

    }
}
