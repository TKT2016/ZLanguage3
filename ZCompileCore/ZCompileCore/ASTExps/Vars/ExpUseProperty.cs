using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的本类Property
    /// </summary>
    public class ExpUseProperty : ExpVarBase
    {
        protected ZLPropertyInfo ZProperty;

        public ExpUseProperty(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            ZProperty = SearchZMember(VarName);
            RetType = ZProperty.ZPropertyType;
            return this;
        }

        private ZLPropertyInfo SearchZMember(string zname)
        {
            ContextImportUse importUseContext = this.FileContext.ImportUseContext;
            return importUseContext.SearchUseZProperty(zname);
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
            MethodInfo getMethod = ZProperty.SharpProperty.GetGetMethod();
            EmitHelper.CallDynamic(IL, getMethod);
            base.EmitConv();
        }

        public override void EmitSet(Exp valueExp)
        {
            EmitSetProperty(valueExp);
        }

        private void EmitSetProperty(Exp valueExp)
        {
            EmitHelper.EmitThis(IL, true);
            EmitValueExp(valueExp);
            MethodInfo setMethod = ZProperty.SharpProperty.GetSetMethod();
            EmitHelper.CallDynamic(IL, setMethod);
            base.EmitConv();
        }
        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ZProperty.GetCanWrite();
            }
        }

        #endregion
    }
}
