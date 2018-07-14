using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.Contexts
{
    public class ContextNestedMethod : ContextMethod
    {
        public ContextNestedMethod(ContextNestedClass classContext)
            : base(classContext)
        {
            IsNested = true;
            //ZCClassInfo cclass = this.ClassContext.GetZCompilingType();
            //ZMethodInfo = new ZCMethodInfo(cclass);
            //cclass.AddMethod(ZMethodInfo);
        }

        public override string ToString()
        {
            return string.Format("ContextNestedMethod[{0}]->{1}",ProcName, this.ClassContext.ToString() );
        }
    }
}
