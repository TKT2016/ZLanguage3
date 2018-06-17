using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class ExpBinary:Exp
    {
        private Exp _LeftExp;
        public Exp LeftExp { get { return _LeftExp; } set { _LeftExp = value;if(_LeftExp!=null) _LeftExp.ParentExp = this; } }
        private Exp _RightExp;
        public Exp RightExp { get { return _RightExp; } set { _RightExp = value; if (_RightExp != null) _RightExp.ParentExp = this; } }
        public LexTokenSymbol OpToken { get; set; }

        private TokenKindSymbol OpKind;
        private MethodInfo OpMethod;
        private ZCLocalVar TempStormLocalVarSymbol ;

        public ExpBinary(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            LeftExp.SetParent(this);
            RightExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { LeftExp, RightExp };
        }

        public override Exp Analy( )
        {    
            if (this.IsAnalyed) return this;
            AnalyCorrect = true;
            if (LeftExp == null && RightExp != null)
            {
                ExpUnary unexp = new ExpUnary(this.ExpContext,OpToken, RightExp);
                var exp = unexp.Analy();
                return exp;
            }
            else if (LeftExp == null && RightExp == null)
            {
                Errorf(this.OpToken.Position, "运算符'{0}'两边缺少表达式", OpToken.Text);
            }
            OpKind = OpToken.Kind;
            LeftExp = AnalySubExp(LeftExp);
            RightExp = AnalySubExp(RightExp);

            if (RightExp == null)
            {
                Errorf(OpToken.Position, "运算符'{0}'右边缺少运算元素", OpToken.Text);
            }
            else
            {
                this.AnalyCorrect = this.LeftExp.AnalyCorrect && RightExp.AnalyCorrect && this.AnalyCorrect;
                if(LeftExp.AnalyCorrect && RightExp.AnalyCorrect)
                {
                    ZType ltype = LeftExp.RetType;
                    ZType rtype = RightExp.RetType;
                    if (ZTypeUtil.IsVoid(ltype) || ZTypeUtil.IsVoid(rtype))
                    {
                        Errorf(OpToken.Position, "没有结果的表达式无法进行'{0}'运算", OpToken.ToCode());
                    }
                    else
                    {
                        OpMethod = ExpBinaryUtil.GetCalcMethod(OpKind, ltype, rtype);
                        if (OpMethod != null)
                        {
                            RetType = ZTypeManager.GetBySharpType(OpMethod.ReturnType) as ZType;
                        }
                        else
                        {
                            Errorf(OpToken.Position, "两种类型无法进行'{0}'运算", OpToken.ToCode());
                        }
                    }
                }
                else
                {
                    this.RetType = ZLangBasicTypes.ZOBJECT;
                }
            }
            //AnalyResultLocal();
            IsAnalyed = true;
            return this;
        }

        public override void AnalyDim()
        {
            AnalyResultLocal();
            LeftExp.AnalyDim();
            RightExp.AnalyDim();
        }

        private static int TempStormIndex = 1;

        private void AnalyResultLocal()
        {
            var VarName = "@binary_temp_storm_" + TempStormIndex;
            TempStormLocalVarSymbol = new ZCLocalVar(VarName, this.RetType);
            this.ProcContext.AddLocalVar(TempStormLocalVarSymbol);
            TempStormIndex++;
        }

        public override void Emit()
        {
            base.EmitArgsExp(new Exp[] { LeftExp, RightExp },OpMethod);
            EmitHelper.CallDynamic(IL, OpMethod);
            EmitSymbolHelper.EmitStorm(IL, TempStormLocalVarSymbol);
            EmitSymbolHelper.EmitLoad(IL, TempStormLocalVarSymbol);
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

        public override CodePosition Position
        {
            get
            {
                return LeftExp.Position; ;
            }
        }
    }
}
