using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;
using ZCompileDesc.Utils;
using ZCompileDesc;
using ZCompileCore.ASTExps;
using ZCompileCore.AST;

namespace ZCompileCore.ASTExps
{
    public class ExpLambdaBody : Exp
    {
        private LambdaOutModel lambdaInfo;
        public LambdaBodyModel lambdaBody;
        
        //List<IIdent> BodyVars;
        //public List<ZCFieldInfo> FieldSymbols { get; private set; }
        //public List<ExpLocal> FieldExpVars { get; private set; }
        
        public ExpLambdaBody(ContextExp outExpContext, LambdaOutModel lambdaInfo)
        {
            ExpContext = outExpContext;
            this.lambdaInfo = lambdaInfo;
            lambdaBody = new LambdaBodyModel();
        }

        public override Exp[] GetSubExps()
        {
            return lambdaInfo.ActionExp.GetSubExps();
        }

        ZCLocalVar retSymbol;
        public Type NestedType { get; private set; }
        public ConstructorBuilder NewBuilder { get; private set; }

        public override Exp Analy( )
        {
            if (this.IsAnalyed) return this;
            CreateContext();
            AnalyFields();
            lambdaInfo.ActionExp.SetContext(NestedExpContext);
            lambdaInfo.ActionExp.SetIsNested(true);
            if (lambdaInfo.FnRetType == ZLangBasicTypes.ZBOOL)
            {
                retSymbol = new ZCLocalVar("$RetResult", lambdaInfo.FnRetType);
            }
            IsAnalyed = true;
            return this;
        }

        internal ContextClass NestedClassContext;
        private ContextMethod NestedProcContext;
        private StmtCall NestedStmt;
        private ContextExp NestedExpContext;

        private void CreateContext()
        {
            NestedClassContext = new ContextClass(this.ExpContext.FileContext);
            NestedClassContext.ClassName = this.ExpContext.ProcContext.CreateNestedClassName();

            NestedProcContext = new ContextMethod(NestedClassContext);
            NestedProcContext.ProcName = NestedClassContext.ClassName + "$CALL";
            //NestedProcContext.ProcManagerContext = NestedClassContext.ProcManagerContext;
            //NestedProcContext.ProcManagerContext.AddContext(NestedProcContext);

            NestedStmt = new StmtCall();
            NestedStmt.ProcContext = NestedProcContext;

            NestedExpContext = new ContextExp(NestedProcContext,NestedStmt);
            lambdaInfo.ActionExp.SetContext(NestedExpContext);

            CreateEmitContext();
        }

        internal TypeBuilder NestedClassBuilder;
        internal MethodBuilder ProcBuilder;

        private void CreateEmitContext()
        {
            var packageName = this.ExpContext.FileContext.ProjectContext.ProjectModel.ProjectPackageName;
            string fullName = packageName + "." + NestedClassContext.ClassName;
            TypeAttributes typeAttrs = TypeAttributes.NestedPrivate | TypeAttributes.Sealed;
            NestedClassBuilder = this.ExpContext.ClassContext.EmitContext.ClassBuilder.DefineNestedType(fullName, typeAttrs);
            NestedClassContext.EmitContext.ClassBuilder = NestedClassBuilder;

            NestedClassContext.EmitContext.IDoc = this.ExpContext.FileContext.ProjectContext.EmitContext.ModuleBuilder.DefineDocument(NestedClassContext.ClassName, Guid.Empty, Guid.Empty, Guid.Empty);
            ProcBuilder = NestedClassBuilder.DefineMethod(NestedProcContext.ProcName, MethodAttributes.Public | MethodAttributes.HideBySig, typeof(void), new Type[] { });
            NestedProcContext.SetBuilder(ProcBuilder);
        }

        public override void Emit()
        {
            EmitCall();
            EmitConstructor();
            var classBuilder = NestedClassBuilder; //NestedClassContext.EmitContext.ClassBuilder
            NestedType = classBuilder.CreateType();
            base.EmitConv();
        }

        private void EmitCall()
        {
            var il = ProcBuilder.GetILGenerator();// NestedProcContext.GetILGenerator();
            lambdaInfo.ActionExp.Emit();
            if (retSymbol == null)
            {
                if (!ZTypeUtil.IsVoid(lambdaInfo.ActionExp.RetType))
                {
                    il.Emit(OpCodes.Pop);
                }
            }
            else
            {
                EmitHelper.StormVar(il, retSymbol.VarBuilder);
            }

            il.Emit(OpCodes.Ret);
        }

        private void EmitConstructor()
        {
            NewBuilder = NestedClassBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { });
            var il = NewBuilder.GetILGenerator();
            il.Emit(OpCodes.Ret);
        }

        string OutClassFieldName = "_$OutClass";
        private void AnalyFields( )
        {
            //FieldSymbols = new List<ZCFieldInfo>();
            AnalyOutClassField();
            AnalyOutArgField();
            AnalyOutLocalField();

            foreach (ExpArg expArg in lambdaInfo.BodyArgExps)
            {
                var symbol = lambdaBody.Get(expArg.VarName);
                expArg.SetAsLambdaFiled(symbol);
            }

            foreach (ExpLocal expVar in lambdaInfo.BodyLocalExps)
            {
                var symbol = lambdaBody.Get(expVar.VarName);
                expVar.SetAsLambdaFiled(symbol);
            }

            if (ExpContext.ClassContext.IsStatic() == false)
            {
                foreach (ExpFieldPropertyBase expField in lambdaInfo.BodyFieldExps)
                {
                    expField.SetLambda(lambdaBody.OutClassField);
                }
            }
        }

        private void AnalyOutLocalField()
        {
            foreach (ZCLocalVar symbol in lambdaInfo.BodyZVars)
            {
                ZType ztype = symbol.GetZType();
                Type varSharpType = ZTypeUtil.GetTypeOrBuilder(ztype);
                FieldBuilder field = NestedClassBuilder.DefineField(symbol.ZName, varSharpType, FieldAttributes.Public);
                ZCFieldInfo fieldSymbol = new ZCFieldInfo();
                fieldSymbol.ZPropertyZName = symbol.ZName;
                fieldSymbol.ZPropertyType = (ZAClassInfo)ztype;
                fieldSymbol.FieldBuilder = field;
                lambdaBody.FieldSymbols.Add(fieldSymbol);
            }
        }

        private void AnalyOutArgField()
        {
            foreach (ZCParamInfo symbol in lambdaInfo.BodyZParams)
            {
                ZType ztype = symbol.GetZType();
                Type varSharpType = ZTypeUtil.GetTypeOrBuilder(ztype);
                FieldBuilder field = NestedClassBuilder.DefineField(symbol.ZName, varSharpType, FieldAttributes.Public);
                ZCFieldInfo fieldSymbol = new ZCFieldInfo();
                fieldSymbol.ZPropertyZName = symbol.ZName;
                fieldSymbol.ZPropertyType = (ZAClassInfo)ztype;
                fieldSymbol.FieldBuilder = field;

                lambdaBody. FieldSymbols.Add(fieldSymbol);
            }
        }

        private void AnalyOutClassField()
        {
            if (ExpContext.ClassContext.IsStatic() == false)
            {
                ZCClassInfo outzclass = ExpContext.ClassContext.ThisCompilingType;
                TypeBuilder builder = outzclass.ClassBuilder;
                FieldBuilder field = NestedClassBuilder.DefineField(OutClassFieldName, builder, FieldAttributes.Public);
                ZCFieldInfo fieldSymbol = new ZCFieldInfo();
                fieldSymbol.FieldBuilder = field;
                fieldSymbol.ZPropertyZName = OutClassFieldName;
                fieldSymbol.ZPropertyType = outzclass;
                lambdaBody.OutClassField = fieldSymbol;
                this.NestedClassContext.NestedOutFieldSymbol = fieldSymbol;
            }
        }

        #region 辅助

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
