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
    /// 程序中定义的本类Field
    /// </summary>
    public class ExpDefField : ExpVarBase
    {
        ZCFieldInfo FieldCompiling;

        public ExpDefField(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            FieldCompiling = this.ProcContext.ClassContext.SeachZField(VarName);
            RetType = FieldCompiling.ZPropertyType;
            return this;
        }

        ZCFieldInfo NestedFieldSymbol;
        public void SetAsLambdaFiled(ZCFieldInfo fieldSymbol)
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
            bool isstatic = this.FieldCompiling.IsStatic;
            EmitHelper.EmitThis(IL, isstatic);
            EmitSymbolHelper.EmitLoad(IL, FieldCompiling);
            base.EmitConv();
        }

        public void EmitLoadFielda()
        {
            bool isstatic = this.FieldCompiling.IsStatic;
            EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.LoadFielda(IL, FieldCompiling.FieldBuilder);
        }

        private void EmitGetNested()
        {
            bool isstatic = this.FieldCompiling.IsStatic;
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.EmitThis(IL, isstatic);
                EmitSymbolHelper.EmitLoad(IL, this.NestedFieldSymbol);
                base.EmitConv();
            }
            else
            {
                if (this.ClassContext.IsStatic() == false)
                {
                    EmitHelper.EmitThis(IL, isstatic);
                    EmitSymbolHelper.EmitLoad(IL, this.ClassContext.NestedOutFieldSymbol);
                }
                EmitSymbolHelper.EmitLoad(IL, FieldCompiling);
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
            bool isstatic = false; // this.PropertyCompiling.IsStatic;
            if (this.NestedFieldSymbol != null)
            {
                EmitHelper.EmitThis(IL, isstatic);
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
            }
            else
            {
                if (this.ClassContext.IsStatic() == false)
                {
                    EmitHelper.EmitThis(IL, isstatic);
                    EmitSymbolHelper.EmitLoad(IL, this.ClassContext.NestedOutFieldSymbol);//EmitHelper.LoadField(IL, this.ClassContext.NestedOutFieldSymbol.Field);
                }
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, FieldCompiling);
            }
        }

        private void EmitSetProperty(Exp valueExp)
        {
            bool isstatic = this.FieldCompiling.IsStatic;
            EmitHelper.EmitThis(IL, isstatic);
            EmitValueExp(valueExp);
            EmitSymbolHelper.EmitStorm(IL, FieldCompiling);
            base.EmitConv();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return FieldCompiling.GetCanWrite();//.CanWrite;
            }
        }

        #endregion
    }
}
