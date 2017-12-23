﻿using System;
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
    /// 程序中定义的使用类中的单个词的方法
    /// </summary>
    public class ExpCallDoubleThis : ExpCallDouble
    {
        ZCMethodInfo ZMethod;
        protected ZCMethodInfo[] Methods;
        public ExpCallDoubleThis(ZCMethodInfo[] methods, Exp argExp,Exp srcExp)
        {
            this.Methods = methods;
            this.ArgExp = argExp;
            this.SrcExp = srcExp;
        }

        public override Exp Analy()
        {
            ZMethod = SearchZMethod();
            RetType = ZMethod.RetZType;
            return this;
        }

        private ZCMethodInfo SearchZMethod()
        {
            return (ZCMethodInfo)(Methods[0]);
        }

        #region Emit
        public override void Emit()
        {
            EmitSubject();
            EmitArgs();
            EmitHelper.CallDynamic(IL, ZMethod.MethodBuilder);//.SharpMethod);
            EmitConv(); 
        }

        protected void EmitArgs()
        {
            EmitArgExp(ZMethod.ZParams[0],ArgExp);//.ZDesces[0].DefArgs[0], ArgExp);
        }

        private void EmitSubject()
        {
            if (ZMethod.GetIsStatic() == false)
            {
                IL.Emit(OpCodes.Ldarg_0);
            }
        }

        #endregion

    }
}
