using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的函数内部变量
    /// </summary>
    public class ExpLocalVar : ExpLocal
    {
        public ZCLocalVar LocalVarSymbol { get; protected set; }
        

        public ExpLocalVar(LexToken token)
        {
            VarToken = token;
            VarName = VarToken.GetText();
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            if (!IsDim)
            {
                LocalVarSymbol = this.ProcContext.GetDefLocal(VarName);
            }
            else
            {

            }
            RetType = LocalVarSymbol.GetZType();
            IsAnalyed = true;
            return this;
        }

        #region Emit
        public override void Emit()
        {
            EmitGet();
        }
       
        public void EmitGet()
        {
            if (IsNested)
            {
                EmitGetNested();
            }
            else
            {
                EmitGetLocal();
            }
        }

        private void EmitGetLocal()
        {
            EmitSymbolHelper.EmitLoad(IL, LocalVarSymbol);
            base.EmitConv();
        }

        public void EmitLoadLocala()
        {
            EmitHelper.LoadVara(IL, LocalVarSymbol.VarBuilder);
        }

        private void EmitGetNested()
        {
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.EmitThis(IL, false);
                EmitSymbolHelper.EmitLoad(IL, this.NestedFieldSymbol);
                base.EmitConv();
            }
            else
            {
                throw new CCException();
            }
        }

        public override void EmitSet(Exp valueExp)
        {
            if (IsNested)
            {
                EmitSetNested(valueExp);
            }
            else
            {
                EmitSetLocal(valueExp);
            }
        }

        //ZCFieldInfo NestedFieldSymbol;
        //public override void SetAsLambdaFiled(ZCFieldInfo fieldSymbol)
        //{
        //    NestedFieldSymbol = fieldSymbol;
        //}

        private void EmitSetNested(Exp valueExp)
        {
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.EmitThis(IL, false);
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
            }
            else
            {
                throw new CCException();
            }
        }

        private void EmitSetLocal(Exp valueExp)
        {
            EmitValueExp(valueExp);
            EmitSymbolHelper.EmitStorm(IL, LocalVarSymbol);
            base.EmitConv();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return LocalVarSymbol.GetCanWrite();
            }
        }
        #endregion
    }
}
