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
using ZCompileCore.ASTExps;

namespace ZCompileCore.AST
{
    public class ExpAssign:Exp
    {
        public Exp ToExp { get; set; }
        public Exp ValueExp { get; set; }
        public bool IsAssignTo { get; set; }

        private Exp NewValueExp;

        public override Exp[] GetSubExps()
        {
            return new Exp[] { ToExp, ValueExp };
        }

        public override Exp Analy( )
        {
            ValueExp = AnalySubExp(ValueExp);
            ToExp = AnalyToExp();
            if (ToExp is ExpDe || ToExp is ExpDi)
            {
                AnalyToExp_DeDi();
            }
            else if (ToExp is ExpUseEnumItem)
            {
                ErrorF(this.ToExp.Position, "约定值'{0}'不能被赋值",ToExp.ToString());
            }
            else if (ToExp is ExpVarBase)
            {

            }
            else
            {
                ErrorF(this.ToExp.Position, "该表达式不能被赋值");
            }
            this.RetType = ZLangBasicTypes.ZVOID;
            NewValueExp = ValueExp;
            AnalyArgLambda();
            return this;
        }

        private Exp AnalyToExp()
        {
            ToExp = AnalySubExp(ToExp);
            if (ToExp is ExpErrorToken)
            {
                ExpLocalVar localVarExp = new ExpLocalVar((ToExp as ExpErrorToken).Token);
                localVarExp.SetContext((ToExp as ExpErrorToken).ExpContext);
                ToExp = localVarExp;
                AnalyToExp_Var();
            }
            return ToExp;
        }

        private void AnalyToExp_Var()
        {
            var varExp = ToExp as ExpLocalVar;
            //var table = this.ProcContext.Symbols;
            //if (!table.Contains(varExp.VarName))
            if (!this.ProcContext.ContainsVarName(varExp.VarName))
            {
                varExp.SetAssigned(ValueExp.RetType);
                ToExp = AnalyDim(varExp);
                AnalyCorrect = AnalyCorrect && ValueExp.AnalyCorrect;
            }
            else
            {
                AnalyToExp_DeDi();
            }
        }

        private Exp AnalyDim(ExpLocalVar varExp)
        {
            var VarName=varExp.VarName;
            ZCLocalVar localVarSymbol = new ZCLocalVar(VarName, ValueExp.RetType);
            localVarSymbol.LoacalVarIndex = this.ExpContext.ProcContext.CreateLocalVarIndex(VarName);
            this.ProcContext.AddLocalVar(localVarSymbol);
            Exp varExp2 = varExp.Analy();
            return varExp2;
        }

        private void AnalyToExp_DeDi()
        {
            ToExp = ToExp.Analy();
            AnalyCorrect = AnalyCorrect && ToExp.AnalyCorrect;
        }

        protected void AnalyArgLambda()
        {
            if (!ToExp.AnalyCorrect) return;
            if (ToExp.RetType==null) return;
            if (ZTypeUtil.IsFn(ToExp.RetType))//(ZLambda.IsFn(ToExp.RetType.SharpType))
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
            //if (ToExp.ToString().IndexOf("X坐标")!=-1)
            //{
            //    Console.WriteLine("X坐标");
            //}
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
