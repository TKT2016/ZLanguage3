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

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 程序中定义的使用类中的单个词的方法
    /// </summary>
    public class ExpCallSingleUse : ExpCallSingle
    {
        protected ZLMethodInfo Method;

        public ExpCallSingleUse(ContextExp expContext, LexToken token)
            : base(expContext)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.Text;
            Method = SearchZMethod(VarName);
            RetType = Method.RetZType;
            IsAnalyed = true;
            return this;
        }

        private ZLMethodInfo SearchZMethod(string name)
        {
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            ContextImportUse contextiu = this.FileContext.ImportUseContext;
            return contextiu.SearchUseMethod(calldesc)[0];
            
        }

        #region Emit
        public override void Emit()
        {
            EmitHelper.CallDynamic(IL, Method.SharpMethod);
            EmitConv(); 
        }

        #endregion
    }
}
