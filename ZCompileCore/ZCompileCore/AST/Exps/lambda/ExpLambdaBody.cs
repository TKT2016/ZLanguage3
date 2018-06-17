using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileCore.Tools;
using ZCompileDesc.Utils;
using ZCompileDesc;
using ZCompileCore.AST.Exps;
using ZCompileCore.AST;

namespace ZCompileCore.AST.Exps
{
    public class ExpLambdaBody : Exp
    {
        private LambdaOutModel lambdaInfo;
        public LambdaBodyModel lambdaBody;
        //private ZCLocalVar retSymbol;
        //internal ContextNestedClass NestedClassContext;
        //private ContextMethod NestedProcContext;
        //private ContextExp NestedExpContext;
        private LambdaProcMethod LambdaProc;

        public ExpLambdaBody(ContextExp outExpContext, LambdaOutModel lambdaInfo)
            : base(outExpContext)
        {
            this.lambdaInfo = lambdaInfo;
            lambdaBody = new LambdaBodyModel();
        }

        protected override void AnalyBody()
        {
            CreateContext();
            AnalyFields();

            //if (LambdaProc.NestedExpContext != lambdaInfo.ActionExp.ExpContext)
            //{
            //    throw new CCException();
            //}
            //if (LambdaProc.NestedProcContext != lambdaInfo.ActionExp.ProcContext)
            //{
            //    throw new CCException();
            //}
            //if (ProcBuilder.GetILGenerator() != lambdaInfo.ActionExp.IL)
            //{
            //    throw new CCException();
            //}
          
            if (lambdaInfo.FnRetType == ZLangBasicTypes.ZBOOL)
            {
                LambdaProc.RetSymbol = new ZCLocalVar("$RetResult", lambdaInfo.FnRetType);
            }
        }

        private void CreateContext()
        {
            LambdaProc = new LambdaProcMethod() { ActionExp = lambdaInfo.ActionExp };
            LambdaProc.NestedClassContext = this.ExpContext.ProcContext.CreateNestedClassContext();
            LambdaProc.NestedMethodContext = LambdaProc.NestedClassContext.CreateContextMethod();
            LambdaProc.NestedExpContext = new ContextExp(LambdaProc.NestedMethodContext);            
            CreateEmitContext();
            LambdaProc.ActionExp.SetContextExpForce(LambdaProc.NestedExpContext);
        }

        //internal TypeBuilder NestedClassBuilder;
        internal MethodBuilder ProcBuilder { get { return LambdaProc.NestedMethodContext.GetBuilder(); } }

        private void CreateEmitContext()
        {
            var NestedClassBuilder = this.LambdaProc.NestedClassContext.SelfCompilingType.ClassBuilder;
            Type retType = null;
            if (lambdaInfo.FnRetType == ZLangBasicTypes.ZBOOL)
            {
                retType = typeof(bool);
            }
            else if (lambdaInfo.FnRetType == ZLangBasicTypes.ZACTION)
            {
                retType = typeof(void);
            }
            var nestedAttr =  MethodAttributes.Public | MethodAttributes.HideBySig;
            var argTypes = new Type[] { };
            var ProcBuilder = NestedClassBuilder.DefineMethod(LambdaProc.NestedMethodContext.ProcName, nestedAttr, retType, argTypes);
            LambdaProc.NestedMethodContext.SetBuilder(ProcBuilder);
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
        }

        public override void Emit()
        {
            LambdaProc.EmitBody();
            //EmitCall();
            //base.EmitConv();
        }

        public override void AnalyDim()
        {
            LambdaProc.AnalyExpDim();
        }

        private void AnalyFields( )
        {
            AnalyOutLocalField();
        }

        private void AnalyOutLocalField()
        {
            foreach (ZCLocalVar symbol in lambdaInfo.BodyZVars)
            {
                string varname = symbol.ZName;
                this.LambdaProc.NestedClassContext.ReplaceLocalToField(varname);
            }
        }

        #region 辅助
        public override Exp[] GetSubExps()
        {
            return lambdaInfo.ActionExp.GetSubExps();
        }

        public override string ToString()
        {
            return lambdaInfo.ActionExp.ToString();
        }

        public override CodePosition Position
        {
            get
            {
                return lambdaInfo.ActionExp.Position; ;
            }
        }
        #endregion
    }
}
