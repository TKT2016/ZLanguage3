using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 使用的Enum项
    /// </summary>
    public class ExpUseEnumItem : ExpVarBase
    {
        protected ZLEnumItemInfo ZEnumItem;

        public ExpUseEnumItem(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            VarToken = token;
        }		

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.Text;
            ZEnumItem = SearchValue(VarName);
            RetType = ZEnumItem.ZEnum;
            IsAnalyed = true;
            return this;
        }

        private ZLEnumItemInfo SearchValue(string zname)
        {
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            ZLEnumItemInfo[] cu = contextiu.SearchUsedZEnumItems(zname);
            return cu[0];
            //ContextUse cu = this.ClassContext.FileContext.UseContext;
            //foreach (var zenum in cu.UseZEnumList)
            //{
            //    var zitem = zenum.SearchValue(zname);
            //    if (zitem != null)
            //    {
            //        return zitem;
            //    }
            //}
            throw new CCException();
        }

        #region Emit
        public override void Emit()
        {
            EmitGet();
        }

        public void EmitGet()
        {
            int enumValue = (int)((ZEnumItem).Value);
            EmitHelper.LoadInt(IL, enumValue);
            base.EmitConv();
        }

        public override void EmitSet(Exp valueExp)
        {
            throw new CCException();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        #endregion
    }
}
