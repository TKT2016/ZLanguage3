using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
    /// 调用本类this单个词方法
    /// </summary>
    public class ExpCallSingleThis : ExpCallSingle
    {
        private ZCMethodInfo Method;

        public ExpCallSingleThis(ContextExp expContext, LexToken token)
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

        private ZCMethodInfo SearchZMethod(string name)
        {
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            var methods = this.ClassContext.SearchThisProc(calldesc);
            if (methods.Length == 0) throw new CCException();
            return methods[0];
        }

        #region Emit
        public override void Emit()
        {
            EmitLoadMain();
            EmitHelper.CallDynamic(IL, Method.MethodBuilder);
            EmitConv();
        }

        #endregion

    }
}
