using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileCore.Contexts;
using System.Reflection.Emit;

namespace ZCompileCore.AST.Exps
{
    /// <summary>
    /// 程序中定义的函数参数
    /// </summary>
    public class ExpArg : ExpLocal
    {
        public ZCParamInfo ArgSymbol { get; protected set; }
        //private int EmitIndex;
        public ExpArg(ContextExp expContext,LexToken token)//,int emitIndex)
            : base(expContext)
        {
            VarToken = token;
            //EmitIndex = emitIndex;
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            VarName = VarToken.Text;
            ArgSymbol = this.ProcContext.GetParameter(VarName);
            RetType = ArgSymbol.ZParamType;
            IsAnalyed = true;
            return this;
        }

        #region Emit
        public override void Emit()
        {
            EmitGet();
        }

        public void EmitGet()
        {
            if (this.ClassContext is ContextNestedClass)
            {
                FieldBuilder fieldBuilder = this.ProcContext.ClassContext.GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;

                EmitHelper.EmitThis(IL, false);
                EmitHelper.LoadField(IL, fieldBuilder);
                base.EmitConv();
            }
            else
            {
                //Console.WriteLine(ArgSymbol.EmitIndex);
                EmitSymbolHelper.EmitLoad(IL, ArgSymbol);
                base.EmitConv();
            }
        }

        public override void EmitSet(Exp valueExp)
        {
            if (this.ClassContext is ContextNestedClass)
            {
                FieldBuilder fieldBuilder = this.ProcContext.ClassContext.GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;

                EmitHelper.EmitThis(IL, false);
                EmitValueExp(valueExp);
                EmitHelper.StormField(IL, fieldBuilder);
                base.EmitConv();
            }
            else
            {
                EmitValueExp(valueExp);
                EmitSymbolHelper.EmitStorm(IL, ArgSymbol);
            }
        }

        public void EmitLoadArga()
        {
            if (this.ClassContext is ContextNestedClass)
            {
                FieldBuilder fieldBuilder = this.ProcContext.ClassContext.GetZCompilingType()
                    .SearchDeclaredZField(VarName).FieldBuilder;

                EmitHelper.EmitThis(IL, false);
                EmitHelper.LoadFielda(IL, fieldBuilder);
            }
            else
            {
                //Console.WriteLine(ArgSymbol.EmitIndex);
                EmitHelper.LoadArga(IL, ArgSymbol.EmitIndex);
            }
        }

        #endregion

        #region 覆盖

        public override bool CanWrite
        {
            get
            {
                return ArgSymbol.GetCanWrite();
            }
        }

        #endregion
    }
}
