using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZLangRT;

namespace ZCompileCore.Symbols
{
    public interface ISymbolTable  //<T> where T : SymbolBase
    {
        ISymbolTable ParentTable { get; set; }
        string TableName { get; }
        bool Contains(string symbolName);
        //void Add(SymbolBase info);
        SymbolBase Get(string symbolName);
        ISymbolTable Push(ISymbolTable iSymbolTable);
        ISymbolTable Pop();

        bool CurrentContains(string symbolName);
        SymbolBase CurrentGet(string symbolName);
    }
}
