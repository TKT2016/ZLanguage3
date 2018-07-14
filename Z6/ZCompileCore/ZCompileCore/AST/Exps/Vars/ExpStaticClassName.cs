﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;

using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore.Contexts;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 程序中定义的函数内部变量
    /// </summary>
    public class ExpStaticClassName : ExpVarBase
    {
        ZLClassInfo classType;
        public ExpStaticClassName(ContextExp expContext, LexToken nameToken, ZLClassInfo classType)
            : base(expContext)
        {
            this.VarToken = nameToken;
            this.classType = classType;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            if (this.ExpContext == null) throw new CCException();
            this.RetType = classType;
            IsAnalyed = true;
            return this;
        }

        #region Emit
        public override void Emit()
        {
            IL.Emit(OpCodes.Nop);
        }

        public override void EmitSet(Exp valueExp)
        {
            throw new CCException();
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            return VarToken.Text;
        }

        #endregion
    }
}
