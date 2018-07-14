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
using ZCompileCore.Contexts;


namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 父类中的成员
    /// </summary>
    public class ExpPropertySuper : ExpPropertyBase
    {
        ZLPropertyInfo ZMember;

        public ExpPropertySuper(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            VarName = VarToken.Text;
            ZLClassInfo zbase = this.ClassContext.GetSuperZType();
            ZMember = zbase.SearchProperty(VarName);
            if (ZMember == null) throw new CCException();
            RetType = ZMember.ZPropertyType;
            IsAnalyed = true;
            return this;
        }

        protected override System.Reflection.MethodInfo GetGetMethod()
        {
            return ZMember.SharpProperty.GetGetMethod();
        }

        protected override System.Reflection.MethodInfo GetSetMethod()
        {
            return ZMember.SharpProperty.GetSetMethod();
        }

        protected override bool GetIsStatic()
        {
            return ZMember.IsStatic;
        }


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
