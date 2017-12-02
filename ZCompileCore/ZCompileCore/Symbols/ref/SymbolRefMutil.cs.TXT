using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;

namespace ZCompileCore.Symbols
{
    public class SymbolRefMutil : SymbolRefBase
    {
        public ZMemberInfo[] SymbolArray { get; private set; }

        public SymbolRefMutil(string name, ZMemberInfo[] symbolArray)
        {
            this.SymbolName = name;
            SymbolArray = symbolArray;

            CanRead = false;
            CanWrite = false;
        }

        public int Count
        {
            get
            {
                return SymbolArray.Length;
            }
        }
    }
}
