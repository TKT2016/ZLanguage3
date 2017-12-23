using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;

namespace ZCompileDesc.Descriptions
{
    public class ZCLocalVar : IIdent, ICompling
    {
        //public bool IsAssigned { get; set; }
        public int LoacalVarIndex { get; set; }
        public LocalBuilder VarBuilder { get; set; }
        public string ZName { get; set; }
        public ZType ZType { get; set; }
        public ZType GetZType() { return this.ZType; } 
        //public bool IsStruct { get { return ZTypeUtil.IsStruct(this.GetZType()); } }
        public bool GetCanWrite() { return true; }
        public bool GetCanRead() { return true; }
        //public Type DimType { get; set; }

        //public SymbolLocalVar(string name)
        //{
        //    this.SymbolName = name;
        //}

        public ZCLocalVar(string name, ZType type)
        {
            this.ZName = name;
            ZType = type;
            //CanRead = true;
            //CanWrite = true;
        }

        //public bool GetCanRead
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        //public bool GetCanWrite
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        public override string ToString()
        {
            return "变量(" + GetZType().ZTypeName + ":" + ZName + ")";
            //throw new CCException();
        }

        public bool IsInBlock { get; set; }

    }
}
