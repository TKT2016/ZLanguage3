using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;

namespace ZCompileCore.Contexts
{
    public class ContextNestedClass : ContextClass
    {
        public static string MasterClassFieldName = "$MasterClass";
        public ContextProc MasterProcContext { get; private set; }
        public bool MasterClassIsStatic { get { return this.MasterProcContext.ClassContext.IsStatic(); } }
        public ZCFieldInfo MasterClassField { get; set; }
        //public List<ZCFieldInfo> MasterArgFields  { get; private set; } 
        public Dictionary<string, ZCFieldInfo> MasterArgDict { get; private set; }
        public ConstructorBuilder DefaultConstructorBuilder { get; set; }

        public ContextNestedClass(ContextProc procContext)
        {
           IsNested = true;
           //MasterArgFields = new List<ZCFieldInfo>();
           MasterArgDict = new Dictionary<string, ZCFieldInfo>();
           MasterProcContext = procContext;
           //this.IsStatic = false;
           this.SelfCompilingType.IsStatic = false;
           this.FileContext = procContext.ClassContext.FileContext;
        }

        private int methodIndex = 0;
        public ContextNestedMethod CreateContextMethod()
        {
            methodIndex++;
            ContextNestedMethod NestedProcContext = new ContextNestedMethod(this);
            NestedProcContext.ProcName = this.ClassName + "$CALL" + methodIndex;
            return NestedProcContext;
        }

        public bool ReplaceLocalToField(string varName)
        {
            if (this.ContainsPropertyName(varName)) return false;
            ZCLocalVar localVar = this.MasterProcContext.LocalManager.GetDefLocal(varName);
            ZType ztype = localVar.GetZType();
            Type varSharpType = ZTypeUtil.GetTypeOrBuilder(ztype);
            var NestedClassBuilder = this.SelfCompilingType.ClassBuilder;
            ZCFieldInfo zf = this.SelfCompilingType.DefineFieldPublic(varName, (ZAClassInfo)ztype);
            localVar.IsReplaceToNestedFiled = true;
            this.MasterProcContext.LocalManager.DecLocalIndex(varName);       
            return true;
        }

        public override string ToString()
        {
            return string.Format("ContextNestedClass[{0}]->{1}",ClassName,this.MasterProcContext );
        }
    }
}
