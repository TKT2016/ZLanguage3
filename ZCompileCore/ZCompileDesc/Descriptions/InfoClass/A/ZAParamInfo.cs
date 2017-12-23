using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Utils;

namespace ZCompileDesc.Descriptions
{
    public abstract class ZAParamInfo
    {
        public abstract bool GetIsFnParam();
        public abstract bool GetIsGenericParam();
        public abstract string GetZParamName();
        public abstract ZType GetZParamType();
        public abstract ZAClassInfo GetZClass();
        public abstract ZAProcInfo GetZProc();
        public abstract int GetZParameterIndex();
        public abstract int GetEmitIndex();


        public bool IsCallArg() { return false; }

        //public bool IsStruct { get { return ZTypeUtil.IsStruct(this.GetZType()); } }
        public virtual ZType GetZType() { return GetZParamType(); }
        public virtual string ZName { get { return GetZParamName(); } }
        public bool GetCanWrite() { return false; }
        public bool GetCanRead() { return true; }
    }
}
