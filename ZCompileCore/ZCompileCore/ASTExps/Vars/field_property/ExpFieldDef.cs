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
    public class ExpFieldDef : ExpFieldBase
    {
        ZCFieldInfo FieldCompiling;

        public ExpFieldDef(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.GetText();
            FieldCompiling = this.ProcContext.ClassContext.SeachZField(VarName);
            RetType = FieldCompiling.ZPropertyType;
            IsAnalyed = true;
            return this;
        }

        protected override System.Reflection.FieldInfo GetFieldInfo()
        {
            return FieldCompiling.FieldBuilder;
        }

        protected override bool GetIsStatic()
        {
            return this.FieldCompiling.IsStatic;
        }

        #region Emit

        //public override void EmitLoadFielda()
        //{
        //    bool isstatic = this.FieldCompiling.IsStatic;
        //    EmitHelper.EmitThis(IL, isstatic);
        //    EmitHelper.LoadFielda(IL, FieldCompiling.FieldBuilder);
        //}

        //public override void EmitGetField()
        //{
        //    bool isstatic = this.FieldCompiling.IsStatic;
        //    EmitHelper.EmitThis(IL, isstatic);
        //    EmitSymbolHelper.EmitLoad(IL, FieldCompiling);
        //    base.EmitConv();
        //}

        //public override void EmitGetNestedField()
        //{
        //    EmitHelper.EmitThis(IL, false);
        //    EmitSymbolHelper.EmitLoad(IL,LambdaThis);
        //    EmitSymbolHelper.EmitLoad(IL, FieldCompiling);
        //}

        //public override void EmitSetNestedField(Exp valueExp)
        //{
        //    EmitHelper.EmitThis(IL, false);
        //    EmitSymbolHelper.EmitLoad(IL, LambdaThis);
        //    EmitValueExp(valueExp);
        //    EmitSymbolHelper.EmitStorm(IL, FieldCompiling);
        //}

        //public override void EmitSetField(Exp valueExp)
        //{
        //    bool isstatic = this.FieldCompiling.IsStatic;
        //    EmitHelper.EmitThis(IL, isstatic);
        //    EmitValueExp(valueExp);
        //    EmitSymbolHelper.EmitStorm(IL, FieldCompiling);
        //    base.EmitConv();
        //}

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
