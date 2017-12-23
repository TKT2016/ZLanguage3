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
    /// 程序中定义的本类Field
    /// </summary>
    public class ExpUseField : ExpVarBase
    {
        protected ZLFieldInfo ZField;

        public ExpUseField(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            ZField = SearchZMember(VarName);
            RetType = ZField.ZFieldType;
            return this;
        }

        private ZLFieldInfo SearchZMember(string zname)
        {
            ContextImportUse importUseContext = this.FileContext.ImportUseContext;
            return importUseContext.SearchUseZField(zname);
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

        public void EmitLoadFielda()
        {
            bool isstatic = this.ZField.IsStatic;
            EmitHelper.EmitThis(IL, isstatic);
            EmitHelper.LoadFielda(IL, ZField.SharpField);
        }

        public override void Emit()
        {
            EmitGetField();
        }

        private void EmitGetField()
        {
            EmitHelper.LoadField(IL, ZField.SharpField);
            base.EmitConv();
        }

        public override void EmitSet(Exp valueExp)
        {
            EmitSetField(valueExp);
        }

        private void EmitSetField(Exp valueExp)
        {
            EmitHelper.EmitThis(IL, true);
            EmitValueExp(valueExp);
            EmitHelper.StormField(IL, ZField.SharpField);
            base.EmitConv();
        }
        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ZField.CanWrite;
            }
        }

        #endregion
    }
}
