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
    /// 父类中的成员
    /// </summary>
    public class ExpSuperProperty : ExpVarBase
    {
        ZLPropertyInfo ZMember;
        public ExpSuperProperty(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.ExpContext == null) throw new CCException();
            VarName = VarToken.GetText();
            ZLClassInfo zbase = this.ClassContext.GetSuperZType();
            ZMember = zbase.SearchProperty(VarName);
            if (ZMember == null) throw new CCException();
            RetType = ZMember.ZPropertyType;//.MemberZType;
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
            EmitGetProperty();
        }

        private void EmitGetProperty()
        {
            EmitHelper.EmitThis(IL, false);
            MethodInfo getMethod = (this.ZMember as ZLPropertyInfo).SharpProperty.GetGetMethod();
            EmitHelper.CallDynamic(IL, getMethod);
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
            MethodInfo setMethod = (this.ZMember as ZLPropertyInfo).SharpProperty.GetSetMethod();
            EmitHelper.CallDynamic(IL, setMethod);
            base.EmitConv();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ZMember.GetCanWrite();
            }
        }

        #endregion
    }
}
