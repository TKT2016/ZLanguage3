using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class ExpBinary:Exp
    {
        public Exp LeftExp { get; set; }
        public Exp RightExp { get; set; }
        public Token OpToken { get; set; }

        TokenKind OpKind;
        MethodInfo OpMethod;

        public override Exp[] GetSubExps()
        {
            return new Exp[] { LeftExp, RightExp };
        }

        public override Exp Analy( )
        {
            if (LeftExp == null && RightExp != null)
            {
                ExpUnary unexp = new ExpUnary(OpToken, RightExp, this.ExpContext);
                //unexp.SetContext(this.ExpContext);
                var exp = unexp.Analy();
                return exp;
            }
            else if (LeftExp == null && RightExp == null)
            {
                ErrorE(this.OpToken.Position, "运算符'{0}'两边缺少表达式", OpToken.GetText());
                //AnalyResult = false;
            }

            OpKind = OpToken.Kind;
            LeftExp = AnalySubExp(LeftExp);
            RightExp = AnalySubExp(RightExp);

            if (RightExp == null)
            {
                ErrorE(OpToken.Position, "运算符'{0}'右边缺少运算元素", OpToken.GetText());
            }
            else
            {
                this.AnalyCorrect = this.LeftExp.AnalyCorrect && RightExp.AnalyCorrect && this.AnalyCorrect;
                if(LeftExp.AnalyCorrect && RightExp.AnalyCorrect)
                {
                    ZType ltype = LeftExp.RetType;
                    ZType rtype = RightExp.RetType;

                    OpMethod = ExpBinaryUtil.GetCalcMethod(OpKind, ltype.SharpType, rtype.SharpType);
                    if (OpMethod != null)
                    {
                        RetType = ZTypeManager.GetBySharpType(OpMethod.ReturnType) as ZType;
                    }
                    else
                    {
                        ErrorE(OpToken.Position, "两种类型无法进行'{0}'运算", OpToken.ToCode());
                    }
                }
                else
                {
                    this.RetType = ZLangBasicTypes.ZOBJECT;
                }
            }
            return this;
        }

        public override void Emit()
        {
            base.EmitArgsExp(new Exp[] { LeftExp, RightExp },OpMethod);
            EmitHelper.CallDynamic(IL, OpMethod);
            base.EmitConv();
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append(LeftExp != null ? LeftExp.ToString() : "");
            buf.Append(OpToken != null ? OpToken.ToCode() : "");
            buf.Append(RightExp != null ? RightExp.ToString() : "");
            return buf.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return LeftExp.Postion; ;
            }
        }
    }
}
