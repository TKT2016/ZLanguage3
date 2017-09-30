﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZLangRT;

namespace ZCompileCore.Symbols
{
    public class ClassSymbolTable : SymbolTableBase
    {
        Dictionary<string, SymbolDefMember> MemberDict = new Dictionary<string, SymbolDefMember>();

        public ClassSymbolTable(string name)
        {
            TableName = name;
        }

        public ClassSymbolTable(string name, ISymbolTable parentTable)
        {
            TableName = name;
            ParentTable = parentTable;
        }

        public override bool CurrentContains(string symbolName)
        {
            return  MemberDict.ContainsKey(symbolName);
        }

        public override SymbolBase CurrentGet(string symbolName)
        {
            if (MemberDict.ContainsKey(symbolName))
            {
                return MemberDict[symbolName];
            }
            return null;
        }

        public void Add(SymbolDefMember symbol)
        {
            MemberDict.Add(symbol.Name, symbol);
        }

        //public bool CurrentContainsMember(string symbolName)
        //{
        //    return  MemberDict.ContainsKey(symbolName);
        //}
    }
}
