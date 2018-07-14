using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;

using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc;
using ZCompileCore.AST.Exps;

namespace ZCompileCore.AST
{
    public class ExpAssign:Exp
    {
        private Exp _ToExp;
        public Exp ToExp { get { return _ToExp; } set { _ToExp = value; _ToExp.ParentExp = this; } }
        private Exp _ValueExp;
        public Exp ValueExp { get { return _ValueExp; } set { _ValueExp = value; _ValueExp.ParentExp = this; } }
        public bool IsAssignTo { get; set; }
        private Exp NewValueExp;

        public ExpAssign(ContextExp expContext)
            : base(expContext)
        {

        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            ToExp.SetParent(this);
            ValueExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ToExp, ValueExp };
        }

        protected override void AnalyBody()
        {
            ValueExp = AnalySubExp(ValueExp);
            ToExp = AnalyToExp();
            if (ToExp is ExpBracketWrapOne)
            {
                ToExp = ((ExpBracketWrapOne)ToExp).VarExp;
            }

            if (ValueExp is ExpBracketWrapOne)
            {
                ValueExp = ((ExpBracketWrapOne)ValueExp).VarExp;
            }
            else if (ValueExp is ExpBracket)
            {
                ValueExp = ((ExpBracket)ValueExp).UnBracket();
            }

            if (ToExp is ExpDe || ToExp is ExpDi)
            {
                AnalyToExp_DeDi();
            }
            else if (ToExp is ExpVarBase)
            {
                //没有错误，不需要提示错误
            }
            else if (ToExp is ExpEachItem)
            {
                //没有错误，不需要提示错误
            }
            else if (ToExp is ExpUseEnumItem)
            {
                Errorf(this.ToExp.Position, "约定值'{0}'不能被赋值",ToExp.ToString());
            }           
            else
            {
                Errorf(this.ToExp.Position, "该表达式不能被赋值");
            }
            this.RetType = ZLangBasicTypes.ZVOID;
            NewValueExp = ValueExp;
            AnalyArgLambda();
        }

        private Exp AnalyToExp()
        {
            if (ToExp is ExpChain)
            {
                var t2 = (ExpChain)ToExp;
                t2.IsAssignTo = true;
            }
            else
            {
                throw new CCException();
            }

            ToExp = AnalySubExp(ToExp);

            if (ToExp is ExpBracketWrapOne)
            {
                ToExp = (ToExp as ExpBracketWrapOne).VarExp;
            }

            if (ToExp is ExpLocalVar)
            {
                var varExp = ToExp as ExpLocalVar;
                if (!this.ProcContext.ContainsVarName(varExp.VarName))
                {
                    varExp.AnalyDim(ValueExp.RetType);
                    //varExp.SetAssigned(ValueExp.RetType);
                    //varExp.SetContextExp(this.ExpContext);
                    //ToExp = AnalyDim(varExp);
                    AnalyCorrect = AnalyCorrect && ValueExp.AnalyCorrect;
                }
            }
            //if (ToExp is ExpErrorToken)
            //{
            //    ExpLocalVar localVarExp = new ExpLocalVar( this.ExpContext, (ToExp as ExpErrorToken).Token);
            //    localVarExp.IsDim = true;
            //    (ToExp as ExpErrorToken).CopyFieldsToExp(localVarExp);
            //    ToExp = localVarExp;
            //    AnalyToExp_Var();
            //}
            return ToExp;
        }

        //private void AnalyToExp_Var()
        //{
        //    var varExp = ToExp as ExpLocalVar;
        //    if (!this.ProcContext.ContainsVarName(varExp.VarName))
        //    {
        //        varExp.SetAssigned(ValueExp.RetType);
        //        //varExp.SetContextExp(this.ExpContext);
        //        ToExp = AnalyDim(varExp);
        //        AnalyCorrect = AnalyCorrect && ValueExp.AnalyCorrect;
        //    }
        //    else
        //    {
        //        AnalyToExp_DeDi();
        //    }
        //}

        //private Exp AnalyDim(ExpLocalVar varExp)
        //{
        //    var VarName=varExp.VarName;
        //    ZCLocalVar localVarSymbol = new ZCLocalVar(VarName, ValueExp.RetType);
        //    varExp.LocalVarSymbol = localVarSymbol;
        //    this.ProcContext.AddLocalVar(localVarSymbol);
        //    Exp varExp2 = varExp.Analy();
        //    return varExp2;
        //}

        private void AnalyToExp_DeDi()
        {
            ToExp = ToExp.Analy();
            AnalyCorrect = AnalyCorrect && ToExp.AnalyCorrect;
            ValueExp.RequireType = ToExp.RetType;
        }

        protected void AnalyArgLambda()
        {
            if (!ToExp.AnalyCorrect) return;
            if (ToExp.RetType==null) return;
            if (ZTypeUtil.IsFn(ToExp.RetType))
            {
                ExpNewLambda newLambdaExp = new ExpNewLambda(this.ExpContext,ValueExp, ToExp.RetType);
                //newLambdaExp.SetContext(this.ExpContext);
                Exp exp2 = newLambdaExp.Analy();
                NewValueExp = exp2;
            }
            else
            {
                NewValueExp = ValueExp;
            }
        }

        public override void Emit()
        {
            IEmitSet ies = (IEmitSet)ToExp;
            ies.EmitSet(NewValueExp);
        }

        public override string ToString()
        {
           if(IsAssignTo)
           {
               return ValueExp.ToString()+"=>"+ToExp.ToString(); 
           }
           else
           {
               return ToExp.ToString() + "=" + ValueExp.ToString();
           }
        }
    }
}
