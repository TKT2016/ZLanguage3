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
    public class UseSymbolTable : SymbolTableBase
    {
        List<ZClassType> UseZClassList { get;  set; }
        List<ZEnumType> UseZEnumList { get;  set; }

        public UseSymbolTable(string name, ISymbolTable parentTable, List<ZClassType> classList, List<ZEnumType> enumList)
        {
            this.ParentTable = parentTable;
            UseZClassList = classList;
            UseZEnumList = enumList;
        }

        public ZMemberInfo[] SearchMember(string zname)
        {
            List<ZMemberInfo> list = new List<ZMemberInfo>();
            foreach(ZEnumType zenum in UseZEnumList )
            {
                ZEnumItemInfo zenumElement = zenum.SearchValue(zname);
                if(zenumElement!=null)
                {
                    list.Add(zenumElement);
                }
            }
            foreach (ZClassType zclass in UseZClassList)
            {
                ZMemberInfo zmember = zclass.SearchZMember(zname);
                if (zmember != null && zmember.AccessAttribute== AccessAttributeEnum.Public)
                {
                    list.Add(zmember);
                }
            }
            return list.ToArray();
        }

        public override bool CurrentContains(string symbolName)
        {
            ZMemberInfo[] zmembers = SearchMember(symbolName);
            return zmembers.Length>0;
        }

        public override SymbolBase CurrentGet(string symbolName)
        {
            ZMemberInfo[] zmembers = SearchMember(symbolName);
            if(zmembers.Length==0)
            {
                return null;
            }
            else if(zmembers.Length==1)
            {
                ZMemberInfo zmember = zmembers[0];
                SymbolRefStaticMember symbol = new SymbolRefStaticMember(symbolName, zmember);// SymbolStaticMember.Create(symbolName, zmembers[0]);
                return symbol;
            }
            else
            {
                SymbolRefMutil symbolm = new SymbolRefMutil(symbolName, zmembers);
                return symbolm;
            }
        }

    }
}
