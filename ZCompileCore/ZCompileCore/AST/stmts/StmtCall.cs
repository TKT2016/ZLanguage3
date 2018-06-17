using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileDesc.Utils;

namespace ZCompileCore.AST
{
    public class StmtCall:Stmt
    {
        public Exp CallExp { get; set; }
        private StmtCallRaw Raw;
        private ExpEach _eachExp;

        public StmtCall(StmtCallRaw raw, Stmt parentStmt)
        {
            Raw = raw;
            ParentStmt = parentStmt;
        }

        public void SetEachExp(ExpEach eachExp)
        {
            _eachExp = eachExp;
        }

        public override Stmt Analy()
        {
            //if (Raw.CallExp.ToString().StartsWith("显示9与3之差"))
            //{
            //    Debugr.WriteLine("显示9与3之差");
            //}
            //if (Raw.CallExp.ToString().EndsWith("之和"))
            //{
            //    Debugr.WriteLine("之和");
            //}
            CallExp = ParseAnalyRawExp(Raw.CallExp);
            if (_eachExp != null)
            {
                _eachExp.BodyExp = CallExp;
                CallExp = _eachExp;
            }
            else
            {
                //CallExp = CallExp3;
            }
            return this;
        }

        public override void AnalyExpDim()
        {
            if (_eachExp != null)
            {
                _eachExp.AnalyDim();
            }
            else
            {
                CallExp.AnalyDim();
            }
        }

        public override void Emit()
        {
            //if (Raw.CallExp.ToString().StartsWith("子弹群添加Z"))
            //{
            //    Debugr.WriteLine("StmtCall Emit 子弹群添加Z");
            //}
            CallExp.Emit();
            if (!ZTypeUtil.IsVoid(CallExp.RetType))
            {
                IL.Emit(OpCodes.Pop);
            }
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}
