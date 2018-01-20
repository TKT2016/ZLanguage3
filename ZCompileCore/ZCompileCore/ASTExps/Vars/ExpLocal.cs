using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的函数内部变量
    /// </summary>
    public abstract class ExpLocal : ExpVarBase
    {
        public ZCFieldInfo NestedFieldSymbol { get; protected set; }
        public virtual void SetAsLambdaFiled(ZCFieldInfo fieldSymbol)
        {
            NestedFieldSymbol = fieldSymbol;
        }
    }
}
