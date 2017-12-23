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
    public class ExpTypeCompiling : ExpTypeSingleBase
    { 
        protected ZCClassInfo CompilingType;

        public ExpTypeCompiling(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            CompilingType = SearchValue(VarName);
            RetType = CompilingType;
            return this;
        }

        private ZCClassInfo SearchValue(string zname)
        {
            return this.ClassContext.GetZCompilingType();
            //ContextFile cu = this.ClassContext.FileContext;
            //var ztypes = cu.ContextFileManager.SearchTypes(zname);
            //foreach (var item in ztypes)
            //{
            //    if (item is ZClassCompilingType)
            //    {
            //        return (item as ZClassCompilingType);
            //    }
            //}
            ////return true;
            ////ContextFile cu = this.ClassContext.FileContext;
            ////if (cu.ContextFileManager.ZCompilingTypes.ContainsKey(zname))
            ////{
            ////    return cu.ContextFileManager.ZCompilingTypes.Get(zname)[0];
            ////}
            //throw new CCException();
        }

        #region Emit
        public override void Emit()
        {
            throw new CCException();
        }

        #endregion
    }
}
