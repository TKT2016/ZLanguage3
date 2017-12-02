using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Compilings;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 父类中的成员
    /// </summary>
    public class ExpSuperProperty : ExpVarBase
    {
        ZMemberInfo ZMember;
        public ExpSuperProperty(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.ExpContext == null) throw new CCException();
            VarName = VarToken.GetText();
            ZClassType zbase = this.ClassContext.GetSuperZType();
            ZMember = zbase.SearchZMember(VarName);
            if (ZMember == null) throw new CCException();
            RetType = ZMember.MemberZType;
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
            EmitGetProperty();
        }

        private void EmitGetProperty()
        {
            EmitHelper.Emit_LoadThis(IL,false);
            MethodInfo getMethod = (this.ZMember as ZPropertyInfo).SharpProperty.GetGetMethod();
            EmitHelper.CallDynamic(IL, getMethod);
            base.EmitConv();
        }

        public override void EmitSet(Exp valueExp)
        {
            EmitSetProperty(valueExp);
        }

        private void EmitSetProperty(Exp valueExp)
        {
            EmitHelper.Emit_LoadThis(IL,false);
            EmitValueExp(valueExp);
            MethodInfo setMethod = (this.ZMember as ZPropertyInfo).SharpProperty.GetSetMethod();
            EmitHelper.CallDynamic(IL, setMethod);
            base.EmitConv();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ZMember.CanWrite;
            }
        }

        #endregion
    }
}
