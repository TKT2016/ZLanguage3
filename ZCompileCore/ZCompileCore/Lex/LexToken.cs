using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZLangRT;
using ZLangRT.Collections;

namespace ZCompileCore.Lex
{
    public abstract class LexToken
    {
        public int Line { get;protected set; }
        public int Col { get; protected set; }
        
        private CodePosition _Postion;
        public CodePosition Position
        {
            get
            {
                if (_Postion == null)
                    _Postion = new CodePosition(Line, Col);
                return _Postion;
            }
        }

        public abstract string Text { get; }
        //public abstract string ToString();
        public abstract string ToCode();

        public bool IsKind(params TokenKindSymbol[] kinds)
        {
            if (this is LexTokenSymbol)
            {
                LexTokenSymbol tokensymbol = (LexTokenSymbol)this;
                foreach (var kind in kinds)
                {
                    if (tokensymbol.Kind == kind)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsKind(params TokenKindKeyword[] kinds)
        {
            if (this is LexTokenText)
            {
                LexTokenText tokensymbol = (LexTokenText)this;
                foreach (var kind in kinds)
                {
                    if(kind== TokenKindKeyword.Ident)
                    {
                        return true;
                    }
                    if (LexTokenText.Dict.ContainsV(kind))
                    {
                        string text = LexTokenText.Dict.GetK(kind);
                        if (tokensymbol.Text == text)//if (tokensymbol.Kind == kind)
                        {
                            return true;
                        }
                    }
                    
                }
            }
            return false;
        }
    }
}
