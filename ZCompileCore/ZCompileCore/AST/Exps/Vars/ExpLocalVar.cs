using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore.Contexts;
using System.Reflection.Emit;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 程序中定义的函数内部变量
    /// </summary>
    public class ExpLocalVar : ExpLocal
    {
        public ZCLocalVar LocalVarSymbol { get; set; }
        public bool IsDim { get; set; }

        public ExpLocalVar(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            VarToken = token;
            VarName = VarToken.Text;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            if (!IsDim)
            {
                LocalVarSymbol = this.ProcContext.LocalManager.GetDefLocal(VarName);
                if(LocalVarSymbol==null)
                {
                    throw new CCException();
                }
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
            if (this.ClassContext is ContextNestedClass)
            {
                FieldBuilder fieldBuilder = this.ProcContext.ClassContext.GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;

                EmitHelper.EmitThis(IL, false);
                EmitHelper.LoadField(IL, fieldBuilder);
                base.EmitConv();
            }
            else if (LocalVarSymbol.IsReplaceToNestedFiled)
            {
                //var IL2 = this.ProcContext.GetNestedClassContext().getIL
                FieldBuilder fieldBuilder = this.ProcContext.GetNestedClassContext().GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;
                ZCLocalVar instanceVar = this.ProcContext.NestedInstance;

                EmitSymbolHelper.EmitLoad(IL, instanceVar);
                EmitHelper.LoadField(IL, fieldBuilder);
                base.EmitConv();
            }
            else
            {
                EmitSymbolHelper.EmitLoad(IL, LocalVarSymbol);
                base.EmitConv();
            }
        }

        public void EmitLoadLocala()
        {
            //EmitHelper.LoadVara(IL, LocalVarSymbol.VarBuilder);
            if (this.ClassContext is ContextNestedClass)
            {
                FieldBuilder fieldBuilder = this.ProcContext.ClassContext.GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;

                EmitHelper.EmitThis(IL, false);
                EmitHelper.LoadFielda(IL, fieldBuilder);
            }
            else if (LocalVarSymbol.IsReplaceToNestedFiled)
            {
                FieldBuilder fieldBuilder = this.ProcContext.GetNestedClassContext().GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;
                ZCLocalVar instanceVar = this.ProcContext.NestedInstance;

                EmitSymbolHelper.EmitLoad(IL, LocalVarSymbol);
                EmitHelper.LoadFielda(IL, fieldBuilder);
            }
            else
            {
                //EmitGetLocal();
                EmitHelper.LoadVara(IL, LocalVarSymbol.VarBuilder);
            }
        }

        //private void EmitGetNested()
        //{
        //    if (this.NestedFieldSymbol != null)
        //    {
        //        EmitHelper.EmitThis(IL, false);
        //        EmitSymbolHelper.EmitLoad(IL, this.NestedFieldSymbol);
        //        base.EmitConv();
        //    }
        //    else
        //    {
        //        throw new CCException();
        //    }
        //}

        public override void EmitSet(Exp valueExp)
        {
            if (this.ClassContext is ContextNestedClass)
            {
                FieldBuilder fieldBuilder = this.ProcContext.ClassContext.GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;

                EmitHelper.EmitThis(IL, false);
                EmitValueExp(valueExp);
                EmitHelper.StormField(IL, fieldBuilder); 

            }
            else if (LocalVarSymbol.IsReplaceToNestedFiled)
            {
                ZCClassInfo classType = this.ProcContext.GetNestedClassContext().GetZCompilingType();
                FieldBuilder fieldBuilder = classType.SearchDeclaredZField(VarName).FieldBuilder;
                ZCLocalVar instanceVar = this.ProcContext.NestedInstance;

                EmitSymbolHelper.EmitLoad(IL, instanceVar);
                EmitValueExp(valueExp);
                EmitHelper.StormField(IL, fieldBuilder);
                base.EmitConv();
            }
            else
            {
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, LocalVarSymbol);
            }
        }

        //ZCFieldInfo NestedFieldSymbol;
        //public override void SetAsLambdaFiled(ZCFieldInfo fieldSymbol)
        //{
        //    NestedFieldSymbol = fieldSymbol;
        //}

        //private void EmitSetNested(Exp valueExp)
        //{
        //    if (this.NestedFieldSymbol != null)
        //    {
        //        EmitHelper.EmitThis(IL, false);
        //        EmitValueExp(valueExp);
        //        EmitSymbolHelper.EmitStorm(IL, this.NestedFieldSymbol);
        //    }
        //    else
        //    {
        //        throw new CCException();
        //    }
        //}

        //private void EmitSetLocal(Exp valueExp)
        //{
        //    EmitValueExp(valueExp);
        //    EmitSymbolHelper.EmitStorm(IL, LocalVarSymbol);
        //    base.EmitConv();
        //}

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
