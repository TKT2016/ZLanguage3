using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;

namespace ZCompileCore.Symbols
{
    public class SymbolLocalVar : SymbolDefBase
    {
        //public bool IsAssigned { get; set; }
        public int LoacalVarIndex { get; set; }
        public LocalBuilder VarBuilder { get; set; }
        //public Type DimType { get; set; }

        //public SymbolLocalVar(string name)
        //{
        //    this.SymbolName = name;
        //}

        public SymbolLocalVar(string name, ZType type)
        {
            this.SymbolName = name;
            SymbolZType = type;
            CanRead = true;
            CanWrite = true;
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override string ToString()
        {
            //return "变量(" + DimType.Name + ":" + SymbolName + ")";
            throw new NotImplementedException();
        }

        public bool IsInBlock { get; set; }

        
    }
}
