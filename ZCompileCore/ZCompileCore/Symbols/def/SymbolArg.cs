using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Reports;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Symbols
{
    public class SymbolArg : SymbolDefBase
    {
        public ZType ArgZType { get; set; }
        public int ArgIndex { get; set; }
        public bool IsGeneric { get; set; }

        public SymbolArg(string name, ZType argType)
        {
            this.SymbolName = name;
            ArgZType = argType;
            SymbolZType = ArgZType;

            CanRead = true;
            CanWrite = true;
        }

        public SymbolArg(string name, ZType argType, int argIndex )
            :this(name,argType)
        {
            ArgIndex = argIndex;
        }

        public override string ToString()
        {
            return "参数(" +  ":" + SymbolName + ")";
        }
    }
}
