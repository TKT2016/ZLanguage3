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
    /// 程序中定义的函数内部变量
    /// </summary>
    public class ExpTypeCompiling : ExpTypeSingleBase
    { 
        protected ZCClassInfo CompilingType;

        public ExpTypeCompiling(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.Text;
            CompilingType = SearchValue(VarName);
            RetType = CompilingType;
            IsAnalyed = true;
            return this;
        }

        private ZCClassInfo SearchValue(string zname)
        {
            return this.ClassContext.GetZCompilingType();
        }

        #region Emit

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override void Emit()
        {
            throw new CCException();
        }

        #endregion
    }
}
