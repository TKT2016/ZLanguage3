using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST.Exps
{
    public class ExpUnary:Exp
    {
        public LexTokenSymbol OpToken { get; set; }
        private Exp _RightExp;
        public Exp RightExp { get { return _RightExp; } set { _RightExp = value; _RightExp.ParentExp = this; } }

        TokenKindSymbol OpKind;

        public ExpUnary(ContextExp expContext, LexTokenSymbol token, Exp rightExp)
            : base(expContext)
        {
            OpToken = token;
            RightExp = rightExp;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            RightExp.SetParent(this);
        }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            OpKind = OpToken.Kind;
            if (OpKind != TokenKindSymbol.ADD && OpKind != TokenKindSymbol.SUB)
            {
                Errorf(OpToken.Position, "运算符'{0}'缺少表达式", OpToken.Text);
                //return null;
            }
            RightExp = AnalySubExp(RightExp);
            if (RightExp.AnalyCorrect)
            {
                this.RetType = RightExp.RetType;
                if(!CanUnary(RetType))
                {
                    Errorf(RightExp.Position, "不能进行'{0}'运算", OpToken.Text);
                }
            }
            IsAnalyed = true;
            if (OpKind == TokenKindSymbol.ADD)
            {
                return RightExp;
            }
            else
            {
                return this;
            }
        }

        private bool CanUnary(ZType ztype)
        {
            if (ztype is ZLType)
            {
                Type stype = ((ZLType)ztype).SharpType;
                if (stype == typeof(int) || stype == typeof(float) || stype == typeof(double) || stype == typeof(decimal))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Emit( )
        {
            if(OpToken.Kind== TokenKindSymbol.ADD)
            {
                RightExp.Emit();
            }
            else
            {
                if (ZTypeUtil.IsFloat(RetType))//(RetType.SharpType == typeof(float))
                {
                    IL.Emit(OpCodes.Ldc_R4,0.0);
                }
                else if (ZTypeUtil.IsInt(RetType))//if (RetType.SharpType == typeof(int))
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

        public override CodePosition Position
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
