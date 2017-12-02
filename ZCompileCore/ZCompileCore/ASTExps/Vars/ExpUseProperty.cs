using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的本类Property
    /// </summary>
    public class ExpUseProperty : ExpVarBase
    {
        protected ZMemberInfo ZMember;

        public ExpUseProperty(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            ZMember = SearchZMember(VarName);
            RetType = ZMember.MemberZType;
            return this;
        }

        private ZMemberInfo SearchZMember(string zname)
        {
            ContextImportUse importUseContext = this.FileContext.ImportUseContext;
            return importUseContext.SearchUseZMember(zname);
            //ContextUse cu = this.ClassContext.FileContext.UseContext;
            //foreach (ZClassType zclass in cu.UseZClassList)
            //{
            //    if (zclass.IsStatic)
            //    {
            //        var zitem = zclass.SearchZMember(zname);
            //        if (zitem != null)
            //        {
            //            return zitem;
            //        }
            //    }
            //}
            //throw new CCException();
        }

        #region Emit
        public override void Emit()
        {
            EmitGetProperty();
        }

        private void EmitGetProperty()
        {
            //EmitSymbolHelper.EmitLoad(IL, ZMember);
            if (this.ZMember is ZPropertyInfo)
            {
                MethodInfo getMethod = (this.ZMember as ZPropertyInfo).SharpProperty.GetGetMethod();
                EmitHelper.CallDynamic(IL, getMethod);
            }
            else if (this.ZMember is ZFieldInfo)
            {
                EmitHelper.LoadField(IL, (this.ZMember as ZFieldInfo).SharpField);
            }
            //else if (this.ZMember is ZEnumItemInfo)
            //{
            //    int enumValue = (int)((this.ZMember as ZEnumItemInfo).Value);
            //    EmitHelper.LoadInt(IL, enumValue);
            //}
            else
            {
                throw new CCException();
            }
            base.EmitConv();
        }

        public override void EmitSet(Exp valueExp)
        {
            EmitSetProperty(valueExp);
        }

        private void EmitSetProperty(Exp valueExp)
        {
            EmitHelper.Emit_LoadThis(IL,true);
            EmitValueExp(valueExp);
            //EmitSymbolHelper.EmitStorm(IL, ZMember);
            if (this.ZMember is ZPropertyInfo)
            {
                MethodInfo setMethod = (this.ZMember as ZPropertyInfo).SharpProperty.GetSetMethod();
                EmitHelper.CallDynamic(IL, setMethod);
            }
            else if (this.ZMember is ZFieldInfo)
            {
                EmitHelper.StormField(IL, (this.ZMember as ZFieldInfo).SharpField);
            }
            //else if (this.ZMember is ZEnumItemInfo)
            //{
            //    int enumValue = (int)((symbol.ZMember as ZEnumItemInfo).Value);
            //    EmitHelper.LoadInt(il, enumValue);
            //}
            else
            {
                throw new CCException();
            }
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
