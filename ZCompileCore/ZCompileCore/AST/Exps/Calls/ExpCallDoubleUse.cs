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
    /// 程序中定义的使用类中的单个词的方法
    /// </summary>
    public class ExpCallDoubleUse : ExpCallDouble
    {
        private ZLMethodInfo Method;
        private ZLMethodInfo[] Methods;

        public ExpCallDoubleUse(ContextExp expContext,ZLMethodInfo[] methods, Exp argExp, Exp srcExp)
            : base(expContext)
        {
            this.Methods = methods;
            this.ArgExp = argExp;
            this.SrcExp = srcExp;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            Method = SearchZMethod();
            RetType = Method.RetZType;
            IsAnalyed = true;
            return this;
        }

        private ZLMethodInfo SearchZMethod( )
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
            EmitArgExp(Method.ZParams[0], ArgExp);//EmitArgExp(Method.ZDesces[0].DefArgs[0], ArgExp);
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
