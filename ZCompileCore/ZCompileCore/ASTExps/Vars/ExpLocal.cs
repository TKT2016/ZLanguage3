using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Compilings;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.ASTExps
{
    /// <summary>
    /// 程序中定义的函数内部变量
    /// </summary>
    public abstract class ExpLocal : ExpVarBase
    {
        public ZMemberCompiling MemberCompiling { get;private set; }

        public void SetAsLambdaFiled(ZMemberCompiling memberCompiling)
        {
            MemberCompiling = memberCompiling;
        }

        //public SymbolDefField NestedFieldSymbol { get; protected set; }
        //public void SetAsLambdaFiled(SymbolDefField fieldSymbol)
        //{
        //    NestedFieldSymbol = fieldSymbol;
        //}

        //public SymbolBase GetSymbol()
        //{
        //    if(this is ExpArg)
        //    {
        //        return (this as ExpArg).ArgSymbol;
        //    }
        //    else if (this is ExpLocalVar)
        //    {
        //        return (this as ExpLocalVar).LocalVarSymbol;
        //    }
        //    throw new CCException();
        //}
    }
}
