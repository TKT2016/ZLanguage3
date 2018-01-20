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
    /// 调用本类this单个词方法
    /// </summary>
    public class ExpCallSingleThis : ExpCallSingle
    {
        private ZCMethodInfo Method;
        public ExpCallSingleThis(LexToken token)
        {
            VarToken = token;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.GetText();
            Method = SearchZMethod(VarName);
            RetType = Method.RetZType;
            IsAnalyed = true;
            return this;
        }

        private ZCMethodInfo SearchZMethod(string name)
        {
            //if (this.ToString().StartsWith("清除出界子弹"))
            //{
            //    Console.WriteLine("清除出界子弹");
            //}
            ZMethodCall calldesc = new ZMethodCall();
            calldesc.Add(name);
            var methods = this.ClassContext.SearchThisProc(calldesc);
            return methods[0];
        }

        #region Emit
        public override void Emit()
        {
            EmitSubject();
            EmitHelper.CallDynamic(IL, Method.MethodBuilder);
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
