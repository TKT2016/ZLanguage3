using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    /// 父类中的字段
    /// </summary>
    public class ExpSuperField : ExpVarBase
    {
        ZLFieldInfo ZField;
        public ExpSuperField(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.ExpContext == null) throw new CCException();
            VarName = VarToken.GetText();
            ZLClassInfo zbase = this.ClassContext.GetSuperZType();
            ZField = zbase.SearchField(VarName);
            if (ZField == null) throw new CCException();
            RetType = ZField.ZFieldType;
            return this;
        }

        ZCFieldInfo NestedFieldSymbol;
        public void SetAsLambdaFiled(ZCFieldInfo fieldSymbol)
        {
            NestedFieldSymbol = fieldSymbol;
        }

        #region Emit

        public void EmitLoadFielda()
        {
            bool isstatic = this.ZField.IsStatic;
            EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.LoadFielda(IL, ZField.SharpField);
        }

        public override void Emit()
        {
            EmitGet();
        }

        public void EmitGet()
        {
            EmitHelper.EmitThis(IL, false);
            EmitSymbolHelper.EmitLoad(IL, ZField);
            //MethodInfo getMethod = (this.ZField as ZLFieldInfo).SharpProperty.GetGetMethod();
            //EmitHelper.CallDynamic(IL, getMethod);
            base.EmitConv();
        }

        public override void EmitSet(Exp valueExp)
        {
            EmitSetProperty(valueExp);
        }

        private void EmitSetProperty(Exp valueExp)
        {
            EmitHelper.EmitThis(IL, false);
            EmitValueExp(valueExp);
            EmitSymbolHelper.EmitStorm(IL, ZField);
            //MethodInfo setMethod = (this.ZField as ZLPropertyInfo).SharpProperty.GetSetMethod();
            //EmitHelper.CallDynamic(IL, setMethod);
            base.EmitConv();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ZField.GetCanWrite();
            }
        }

        #endregion
    }
}
