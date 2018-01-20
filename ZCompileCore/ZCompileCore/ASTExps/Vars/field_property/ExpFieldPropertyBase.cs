using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileDesc.Descriptions;

namespace ZCompileCore.ASTExps
{
    public abstract class ExpFieldPropertyBase : ExpVarBase
    {
        protected ZCFieldInfo LambdaThis;

        public void SetLambda(ZCFieldInfo lambdaThis)
        {
            IsNested = true;
            LambdaThis = lambdaThis;
        }

        public abstract void EmitGet();

        public override void Emit()
        {
            EmitGet();
        }
 
    }
}
