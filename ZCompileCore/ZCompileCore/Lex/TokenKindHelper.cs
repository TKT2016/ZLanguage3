using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZCompileCore.Lex
{
    public static class TokenKindHelper
    {
        public static bool IsLiteral(TokenKind Kind)
        {
            return Kind == TokenKind.LiteralInt
                || Kind == TokenKind.LiteralFloat
                || Kind == TokenKind.NULL
                || Kind == TokenKind.LiteralString
                || Kind == TokenKind.True
                || Kind == TokenKind.False
                   ;
        }

        public static bool IsOp(TokenKind kind)
        {
            return kind == TokenKind.ADD
                || kind == TokenKind.SUB
                || kind == TokenKind.MUL
                || kind == TokenKind.DIV
                || kind == TokenKind.AND
                || kind == TokenKind.OR
                || IsCompareOp(kind);
                   ;
        }

        public static bool IsCompareOp(TokenKind kind)
        {
            return kind == TokenKind.GT || kind == TokenKind.LT || kind == TokenKind.GE
                || kind == TokenKind.LE || kind == TokenKind.NE || kind == TokenKind.EQ;
        }
    }
}
