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

using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 调用基类单个词方法
    /// </summary>
    public class ExpCallSingleSuper : ExpCallSingle
    {
        protected ZLMethodInfo Method;
        public ExpCallSingleSuper(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            VarName = VarToken.GetText();
            Method = SearchZMethod(VarName);
            RetType = Method.RetZType;
            return this;
        }

        private ZLMethodInfo SearchZMethod(string name)
        {
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            var methods = this.ClassContext.SearchSuperProc(calldesc);
            return methods[0];
        }

        #region Emit
        public override void Emit()
        {
            EmitSubject();
            EmitHelper.CallDynamic(IL, Method.SharpMethod);
            EmitConv(); 
        }

        private void EmitSubject()
        {
            if (Method.GetIsStatic() == false)
            {
                IL.Emit(OpCodes.Ldarg_0);
            }
        }

        #endregion

    }
}
