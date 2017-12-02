using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileCore.Symbols;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.Symbols
{
    public class SuperSymbolTable : SymbolTableBase
    {
        ZClassType classType;

        public SuperSymbolTable(string name,ZClassType zclass)
        {
            TableName = name;
            classType = zclass;
        }

        public SuperSymbolTable(string name, ZClassType zclass, ISymbolTable parentTable)
        {
            TableName = name;
            classType = zclass;
            ParentTable = parentTable;
        }

        public ZMemberInfo SearchMember(string zname)
        {
            if (classType == null) return null;
            ZMemberInfo zmember = classType.SearchZMember(zname);
            if (zmember == null) return null;

            if (zmember.AccessAttribute != AccessAttributeEnum.Private)
            {
                return zmember;
            }
            return null;
        }

        public override bool CurrentContains(string symbolName)
        {
            if (classType == null) return false;
            ZMemberInfo zmember = SearchMember(symbolName);
            return zmember != null;
        }

        public override SymbolBase CurrentGet(string symbolName)
        {
            if (classType == null) return null;
            ZMemberInfo zmember = SearchMember(symbolName);
            if (zmember != null)
            {
                SymbolDefMember symbol = SymbolDefMember.Create(symbolName, zmember);
                return symbol;
            }
            return null;
        }

    }
}
