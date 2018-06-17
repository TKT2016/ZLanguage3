using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.Lex
{
    public static class TokenZTypeHelper
    {
        public static ZLClassInfo GetLiteralZType(LexTokenLiteral LiteralToken)
        {
            ZLClassInfo RetType = null; ;
            var LiteralKind = LiteralToken.Kind;
            var LiteralValue = LiteralToken.Text;

            if (LiteralKind == TokenKindLiteral.LiteralInt)
            {
                RetType = ZLangBasicTypes.ZINT;
            }
            else if (LiteralKind == TokenKindLiteral.LiteralFloat)
            {
                RetType = ZLangBasicTypes.ZFLOAT;
            }
            else if (LiteralKind == TokenKindLiteral.LiteralString)
            {
                RetType = ZLangBasicTypes.ZSTRING;
            }
            else if (LiteralKind == TokenKindLiteral.True || LiteralKind == TokenKindLiteral.False)
            {
                RetType = ZLangBasicTypes.ZBOOL;
            }
            else if (LiteralKind == TokenKindLiteral.NULL)
            {
                RetType = null; //ZTypeCache.CreateZRealType(typeof(整数));// null;
            }
            return RetType;
        }
    }
}
