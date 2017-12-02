using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Compilings;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的函数参数
    /// </summary>
    public class ExpArg : ExpLocal
    {
        public SymbolArg ArgSymbol { get; protected set; }

        public ExpArg(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            //var symbols = this.ProcContext.Symbols;
            ArgSymbol = this.ProcContext.GetDefArg(VarName);// symbols.Get(VarName) as SymbolArg;
            RetType = ArgSymbol.SymbolZType;
            return this;
        }

        ZMemberCompiling NestedFieldSymbol;
        //public void SetAsLambdaFiled(ZMemberCompiling fieldSymbol)
        //{
        //    NestedFieldSymbol = fieldSymbol;
        //}

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
                EmitHelper.Emit_LoadThis(IL,false);
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
                EmitHelper.Emit_LoadThis(IL,true);
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

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ArgSymbol.CanWrite;
            }
        }

        #endregion
    }
}
