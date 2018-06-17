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
using Z语言系统;

namespace ZCompileCore.AST.Exps
{
    public  class ExpLiteral:Exp
    {
        public LexTokenLiteral LiteralToken { get;private set; }
        private TokenKindLiteral LiteralKind;
        private string LiteralValue;

        public ExpLiteral(ContextExp expContext, LexTokenLiteral literalToken)
            : base(expContext)
        {
            LiteralToken = literalToken;
        }

        public string IdentName
        {
            get
            {
                return LiteralToken.ToCode();
            }
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public ZLClassInfo AnalyLiteralZType( )
        {
            LiteralKind = LiteralToken.Kind;
            LiteralValue = LiteralToken.Text;
            return TokenZTypeHelper.GetLiteralZType(LiteralToken);
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            RetType = AnalyLiteralZType();
            
            if (RetType == null)
            {
                Errorf(this.Position, LiteralToken.ToCode() + "不是正确的值");
            }
            IsAnalyed = true;
            return this;
        }
        
        public override void Emit()
        {
            if (LiteralKind == TokenKindLiteral.LiteralString)
            {
                EmitHelper.LoadString(IL, LiteralValue);
            }
            else if (LiteralKind == TokenKindLiteral.LiteralInt)
            {
                GenerateInt();
            }
            else if (LiteralKind == TokenKindLiteral.LiteralFloat)
            {
                GenerateFloat();
            }
            else if (LiteralKind == TokenKindLiteral.True || LiteralKind == TokenKindLiteral.False)
            {
                GenerateBool();
            }
            else if (LiteralKind == TokenKindLiteral.NULL)
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
            if (LiteralKind == TokenKindLiteral.True)
            {
                IL.Emit(OpCodes.Ldc_I4_1);
            }
            else if (LiteralKind == TokenKindLiteral.False)
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
