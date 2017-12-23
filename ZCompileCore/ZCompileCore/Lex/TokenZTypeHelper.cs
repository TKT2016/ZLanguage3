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
        public static ZLClassInfo GetLiteralZType(LexToken LiteralToken)
        {
            ZLClassInfo RetType = null; ;
            var LiteralKind = LiteralToken.Kind;
            var LiteralValue = LiteralToken.GetText();

            if (LiteralKind == TokenKind.LiteralInt)
            {
                RetType = ZLangBasicTypes.ZINT;
            }
            else if (LiteralKind == TokenKind.LiteralFloat)
            {
                RetType = ZLangBasicTypes.ZFLOAT;
            }
            else if (LiteralKind == TokenKind.LiteralString)
            {
                RetType = ZLangBasicTypes.ZSTRING;
            }
            else if (LiteralKind == TokenKind.True || LiteralKind == TokenKind.False)
            {
                RetType = ZLangBasicTypes.ZBOOL;
            }
            else if (LiteralKind == TokenKind.NULL)
            {
                RetType = null; //ZTypeCache.CreateZRealType(typeof(整数));// null;
            }
            return RetType;
        }
    }
}
