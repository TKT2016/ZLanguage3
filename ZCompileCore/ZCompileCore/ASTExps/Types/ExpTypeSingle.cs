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
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的函数内部变量
    /// </summary>
    public class ExpTypeSingle : ExpTypeSingleBase
    {
        protected ZType SubjZType;

        public ExpTypeSingle(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            SubjZType = SearchValue(VarName);
            RetType = SubjZType;
            return this;
        }

        private ZType SearchValue(string zname)
        {
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            var ztypes = contextiu.SearchZTypesByClassNameOrDimItem(zname);
            return ztypes[0];
        }

        #region Emit
        public override void Emit()
        {
            throw new CCException();
        }

        #endregion

    }
}
