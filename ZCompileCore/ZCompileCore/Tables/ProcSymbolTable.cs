using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZLangRT;

namespace ZCompileCore.Symbols
{
    public class ProcSymbolTable : SymbolTableBase
    {
        Dictionary<string, SymbolArg> ArgsDict = new Dictionary<string, SymbolArg>();
        Dictionary<string, SymbolLocalVar> LocalsDict = new Dictionary<string, SymbolLocalVar>();

        public ProcSymbolTable(string name)
        {
            TableName = name;
        }

        public ProcSymbolTable(string name, ISymbolTable parentTable)
        {
            TableName = name;
            ParentTable = parentTable;
        }

        public override bool CurrentContains(string symbolName)
        {
            return  LocalsDict.ContainsKey(symbolName) || ArgsDict.ContainsKey(symbolName);
        }

        public override SymbolBase CurrentGet(string symbolName)
        {
            if (LocalsDict.ContainsKey(symbolName))
            {
                return LocalsDict[symbolName];
            }
            else if (ArgsDict.ContainsKey(symbolName))
            {
                return ArgsDict[symbolName];
            }
            return null;
        }

        public void Add(SymbolArg argSymbol)
        {
            ArgsDict.Add(argSymbol.Name, argSymbol);
        }

        public void Add(SymbolLocalVar localSymbol)
        {
            LocalsDict.Add(localSymbol.Name, localSymbol);
        }

        public bool CurrentContainsArg(string symbolName)
        {
            return  ArgsDict.ContainsKey(symbolName);
        }

        public bool CurrentContainsLocalVar(string symbolName)
        {
            return LocalsDict.ContainsKey(symbolName);
        }
    }
}
