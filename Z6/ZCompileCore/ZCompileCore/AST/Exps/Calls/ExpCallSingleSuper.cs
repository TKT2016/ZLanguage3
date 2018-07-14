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
    /// 调用基类单个词方法
    /// </summary>
    public class ExpCallSingleSuper : ExpCallSingle
    {
        protected ZLMethodInfo Method;
        public ExpCallSingleSuper(ContextExp expContext, LexToken token)
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
            var methods = this.ClassContext.SearchSuperProc(calldesc);
            return methods[0];
        }

        public override void Emit()
        {
            EmitLoadMain();
            EmitHelper.CallDynamic(IL, Method.SharpMethod);
            EmitConv(); 
        }
    }
}
