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
    /// 程序中定义的函数参数
    /// </summary>
    public class ExpArg : ExpLocal
    {
        public ZCParamInfo ArgSymbol { get; protected set; }

        public ExpArg(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.GetText();
            ArgSymbol = this.ProcContext.GetParameter(VarName);
            RetType = ArgSymbol.ZParamType;
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
                EmitGetArg();
            }
        }

        private void EmitGetArg()
        {
            EmitSymbolHelper.EmitLoad(IL, ArgSymbol);
            base.EmitConv();
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
                EmitSetArg(valueExp);
            }
        }

        private void EmitSetNested(Exp valueExp)
        {
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.EmitThis(IL, true);
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
            }
            else
            {
                throw new CCException();
            }
        }

        private void EmitSetArg(Exp valueExp)
        {
            EmitValueExp(valueExp);
            EmitSymbolHelper.EmitStorm(IL, ArgSymbol);
            base.EmitConv();
        }

        public void EmitLoadArga()
        {
            EmitHelper.LoadArga(IL, ArgSymbol.EmitIndex);
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ArgSymbol.GetCanWrite();
            }
        }

        #endregion
    }
}
