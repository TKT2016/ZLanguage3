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
    /// 程序中定义的本类Property
    /// </summary>
    public class ExpDefProperty : ExpVarBase
    {
        //protected SymbolDefProperty PropertySymbol;
        ZMemberCompiling PropertyCompiling;

        public ExpDefProperty(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            //if (VarName.ToString() == "子弹群")
            //{
            //    Console.WriteLine("子弹群");
            //}
            PropertyCompiling = this.ProcContext.ClassContext.SeachZProperty(VarName);
            RetType = PropertyCompiling.GetMemberType();// PropertySymbol.SymbolZType;
            return this;
        }

        ZMemberCompiling NestedFieldSymbol;
        public void SetAsLambdaFiled(ZMemberCompiling fieldSymbol)
        {
            NestedFieldSymbol = fieldSymbol;
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
                EmitGetProperty();
            }
        }

        private void EmitGetProperty()
        {
            //if (VarName.ToString() == "子弹群")
            //{
            //    Console.WriteLine("子弹群");
            //}
            bool isstatic = this.PropertyCompiling.IsStatic;
            EmitHelper.Emit_LoadThis(IL, isstatic);
            EmitSymbolHelper.EmitLoad(IL, PropertyCompiling);
            base.EmitConv();
        }

        private void EmitGetNested()
        {
            bool isstatic = this.PropertyCompiling.IsStatic;
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.Emit_LoadThis(IL,isstatic);
                EmitSymbolHelper.EmitLoad(IL, this.NestedFieldSymbol);
                base.EmitConv();
            }
            else
            {
                if (this.ClassContext.IsStatic() == false)
                {
                    EmitHelper.Emit_LoadThis(IL, isstatic);
                    EmitSymbolHelper.EmitLoad(IL, this.ClassContext.NestedOutFieldSymbol);
                }
                EmitSymbolHelper.EmitLoad(IL, PropertyCompiling);
                base.EmitConv();
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
                EmitSetProperty(valueExp);
            }
        }

        private void EmitSetNested(Exp valueExp)
        {
            bool isstatic = false;// this.PropertyCompiling.IsStatic;
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.Emit_LoadThis(IL, isstatic);
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
            }
            else
            {
                if (this.ClassContext.IsStatic() == false)
                {
                    EmitHelper.Emit_LoadThis(IL, isstatic);
                    EmitSymbolHelper.EmitLoad(IL, this.ClassContext.NestedOutFieldSymbol);//EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
                }
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, PropertyCompiling);
            }
        }

        private void EmitSetProperty(Exp valueExp)
        {
            bool isstatic = this.PropertyCompiling.IsStatic;
            EmitHelper.Emit_LoadThis(IL, isstatic);
            EmitValueExp(valueExp);
            EmitSymbolHelper.EmitStorm(IL, PropertyCompiling);
            base.EmitConv();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return PropertyCompiling.CanWrite;
            }
        }

        #endregion
    }
}
