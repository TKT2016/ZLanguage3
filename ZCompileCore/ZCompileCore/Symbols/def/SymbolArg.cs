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

        public SymbolArg(string name, ZType argType, int argIndex )
        {
            this.SymbolName = name;
            ArgZType = argType;
            ArgIndex = argIndex;
            SymbolZType = ArgZType;

            CanRead = true;
            CanWrite = true;
        }

        public override string ToString()
        {
            return "参数(" +  ":" + SymbolName + ")";
        }
    }
}
