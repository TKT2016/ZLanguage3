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
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Compilings;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 调用本类this单个词方法
    /// </summary>
    public class ExpCallSingleThis : ExpCallSingle
    {
        public ExpCallSingleThis(LexToken token)
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

        private ZMethodInfo SearchZMethod(string name)
        {
            //if (this.ToString().StartsWith("清除出界子弹"))
            //{
            //    Console.WriteLine("清除出界子弹");
            //}
            ZCallDesc calldesc = new ZCallDesc();
            calldesc.Add(name);
            var methods = this.ClassContext.SearchThisProc(calldesc);
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
            if (Method.IsStatic == false)
            {
                IL.Emit(OpCodes.Ldarg_0);
            }
        }

        #endregion

    }
}
