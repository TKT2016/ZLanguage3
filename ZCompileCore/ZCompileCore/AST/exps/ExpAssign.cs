using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Symbols;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class ExpAssign:Exp
    {
        public Exp ToExp { get; set; }
        public Exp ValueExp { get; set; }
        public bool IsAssignTo { get; set; }

        Exp NewValueExp;
        public override Exp[] GetSubExps()
        {
            return new Exp[] { ToExp, ValueExp };
        }

        public override Exp Analy( )
        {
            ValueExp = AnalySubExp(ValueExp);
            
            if(ToExp is ExpVar )
            {
                AnalyToExp_Var();
            }
            else if (ToExp is ExpDe || ToExp is ExpDi)
            {
                AnalyToExp_DeDi();
            }
            else
            {
                ErrorE(this.ToExp.Postion, "该表达式不能被赋值");
            }
            this.RetType = ZLangBasicTypes.ZVOID;
            AnalyArgLambda();
            return this;
        }

        private void AnalyToExp_Var()
        {
            var varExp = ToExp as ExpVar;
            var table = this.ProcContext.Symbols;
            if (!table.Contains(varExp.VarName))
            {
                varExp.SetAssigned(ValueExp.RetType);
                ToExp = varExp.AnalyDim();
                AnalyCorrect = AnalyCorrect && ValueExp.AnalyCorrect;
            }
            else
            {
                AnalyToExp_DeDi();
            }
        }

        private void AnalyToExp_DeDi()
        {
            ToExp = ToExp.Analy();
            AnalyCorrect = AnalyCorrect && ToExp.AnalyCorrect;
        }

        protected void AnalyArgLambda()
        {
            if (!ToExp.AnalyCorrect) return;
            if (ZLambda.IsFn(ToExp.RetType.SharpType))
            {
                ExpNewLambda newLambdaExp = new ExpNewLambda(ValueExp, ToExp.RetType);
                newLambdaExp.SetContext(this.ExpContext);
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
            (ToExp as ISetter).EmitSet(NewValueExp);
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
