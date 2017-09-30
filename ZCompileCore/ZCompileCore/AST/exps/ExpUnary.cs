using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class ExpUnary:Exp
    {
        public Token OpToken { get; set; }
        public Exp RightExp { get; set; }

        TokenKind OpKind;

        public ExpUnary(Token token, Exp rightExp,ContextExp context)
        {
            OpToken = token;
            RightExp = rightExp;
            this.ExpContext = context;
        }

        public override Exp Analy( )
        {
            OpKind = OpToken.Kind;
            if (OpKind != TokenKind.ADD && OpKind != TokenKind.SUB)
            {
                ErrorE(OpToken.Position, "运算符'{0}'缺少表达式", OpToken.GetText());
                //return null;
            }
            RightExp = AnalySubExp(RightExp);// RightExp.Analy();
            if (RightExp.AnalyCorrect)
            {
                this.RetType = RightExp.RetType;
                Type stype = RetType.SharpType;
                if (stype != typeof(int) && stype != typeof(float) && stype != typeof(double) && stype != typeof(decimal))
                {
                    ErrorE(RightExp.Postion, "不能进行'{0}'运算", OpToken.GetText());
                    //return null;
                }
            }

            if (OpKind == TokenKind.ADD)
            {
                return RightExp;
            }
            else
            {
                return this;
            }
        }

        public override void Emit( )
        {
            if(OpToken.Kind== TokenKind.ADD)
            {
                RightExp.Emit();
            }
            else
            {
                if (RetType.SharpType == typeof(float))
                {
                    IL.Emit(OpCodes.Ldc_R4,0.0);
                }
                else if (RetType.SharpType == typeof(int))
                {
                    EmitHelper.LoadInt(IL, 0);
                }
                RightExp.Emit();
                IL.Emit(OpCodes.Sub);
            }
            base.EmitConv();
        }


        public override Exp[] GetSubExps()
        {
            return new Exp[] { RightExp };
        }
		
        #region 覆盖
        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(OpToken != null ? OpToken.ToCode() : "");
            buf.Append(RightExp != null ? RightExp.ToString() : "");
            return buf.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return OpToken.Position; ;
            }
        }

        //public override void GetNestedFields(Dictionary<string, VarExp> nestedField)
        //{
        //    RightExp.GetNestedFields(nestedField);
        //}
        #endregion

    }
}
