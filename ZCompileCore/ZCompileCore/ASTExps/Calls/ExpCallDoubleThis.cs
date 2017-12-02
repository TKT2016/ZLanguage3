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
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的使用类中的单个词的方法
    /// </summary>
    public class ExpCallDoubleThis : ExpCallDouble
    {
        public ExpCallDoubleThis(ZMethodInfo[] methods, Exp argExp,Exp srcExp)
        {
            this.Methods = methods;
            this.ArgExp = argExp;
            this.SrcExp = srcExp;
        }

        public override Exp Analy()
        {
            Method = SearchZMethod();
            RetType = Method.RetZType;
            return this;
        }

        private ZMethodInfo SearchZMethod( )
        {
            return Methods[0];
        }

        #region Emit
        public override void Emit()
        {
            EmitSubject();
            EmitArgs();
            EmitHelper.CallDynamic(IL, Method.SharpMethod);
            EmitConv(); 
        }

        protected void EmitArgs()
        {
            EmitArgExp(Method.ZDesces[0].DefArgs[0], ArgExp);
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
