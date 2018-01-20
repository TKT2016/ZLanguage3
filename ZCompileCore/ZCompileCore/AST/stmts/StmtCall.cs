using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.ASTExps;
using ZCompileCore.Contexts;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public  class StmtCall:Stmt
    {
        public Exp CallExp { get; set; }
        ExpEach _eachExp;

        public override void DoAnaly()
        {
            var tempExp1 = CallExp;
            var CallExp2 = ParseExp(tempExp1);
            var CallExp3 = CallExp2.Analy();
            if (_eachExp != null)
            {
                _eachExp.BodyExp = CallExp3;
                CallExp = _eachExp;
            }
            else
            {
                CallExp = CallExp3;
            }
        }
   
        public void SetEachExp(ExpEach eachExp)
        {
            _eachExp = eachExp;
        }

        public override void Emit()
        {
            CallExp.Emit();
            if (!ZTypeUtil.IsVoid(CallExp.RetType))
            {
                IL.Emit(OpCodes.Pop);
            }
        }

        public override string ToString()
        {
            return CallExp.ToString() + ";";
        }
    }
}
