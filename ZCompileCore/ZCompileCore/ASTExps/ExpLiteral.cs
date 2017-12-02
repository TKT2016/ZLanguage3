using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;
using Z语言系统;

namespace ZCompileCore.AST
{
    public  class ExpLiteral:Exp
    {
        public LexToken LiteralToken { get; set; }

        public string IdentName
        {
            get
            {
                return LiteralToken.ToCode();
            }
        }

        TokenKind LiteralKind;
        string LiteralValue;

        public ZClassType AnalyLiteralZType( )
        {
            LiteralKind = LiteralToken.Kind;
            LiteralValue = LiteralToken.GetText();
            return TokenZTypeHelper.GetLiteralZType(LiteralToken);
        }

        public override Exp Analy( )
        {
            //LiteralKind = LiteralToken.Kind;
            //LiteralValue = LiteralToken.GetText();
            RetType = AnalyLiteralZType();
            
            if (RetType == null)
            {
                ErrorF(this.Position, LiteralToken.ToCode() + "不是正确的值");
            }
            return this;
        }
        
        public override void Emit()
        {
            if (LiteralKind == TokenKind.LiteralString)
            {
                EmitHelper.LoadString(IL, LiteralValue);
            } 
            else if (LiteralKind == TokenKind.LiteralInt)
            {
                GenerateInt();
            }
            else if (LiteralKind == TokenKind.LiteralFloat)
            {
                GenerateFloat();
            }
            else if (LiteralKind == TokenKind.True || LiteralKind == TokenKind.False)
            {
                GenerateBool();
            }
            else if (LiteralKind == TokenKind.NULL)
            {
                IL.Emit(OpCodes.Ldnull);
            }
            base.EmitConv();
        }

        private void GenerateInt( )
        {
            int value = int.Parse(LiteralValue);
            EmitHelper.LoadInt(IL, value);
        }

        private void GenerateFloat( )
        {
            var value = float.Parse(LiteralValue);
            IL.Emit(OpCodes.Ldc_R4, value);
        }

        private void GenerateBool( )
        {
            if (LiteralKind == TokenKind.True)
            {
                IL.Emit(OpCodes.Ldc_I4_1);
            }
            else if (LiteralKind == TokenKind.False)
            {
                IL.Emit(OpCodes.Ldc_I4_0);
            }
        }
        
        public override string ToString()
        {
            return LiteralToken.ToCode();
        }

        public override CodePosition Position
        {
            get
            {
                return LiteralToken.Position; ;
            }
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { };
        }
    }
}
