using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileKit.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Symbols
{
    public abstract class SymbolBase:IName
    {
        public string Name
        {
            get
            {
                return SymbolName;
            }
        }

        public string SymbolName { get; set; }
        public virtual ZType SymbolZType { get; set; }
        public virtual bool CanRead { get; protected set; }
        public virtual bool CanWrite { get; protected set; }

        public override string ToString()
        {
            return this.GetType().Name + "-" + SymbolName + "-" + SymbolZType.ZName;
        }
    }
}
