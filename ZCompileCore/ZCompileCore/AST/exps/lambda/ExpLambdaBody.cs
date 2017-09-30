using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZLangRT;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class ExpLambdaBody : Exp
    {
        List<SymbolBase> BodyVars;
        public List<SymbolDefField> FieldSymbols { get; private set; }
        public List<ExpVar> FieldExpVars { get; private set; }

        public ExpLambdaBody(Exp exp, ZType fnRetType, List<SymbolBase> bodyVars, List<ExpVar> fieldExpVars, ContextExp outExpContext)
        {
            BodyExp = exp;
            FnRetType = fnRetType;
            BodyVars = bodyVars;
            FieldExpVars = fieldExpVars;
            ExpContext = outExpContext;
        }

        public Exp BodyExp { get; set; }
        public ZType FnRetType { get; set; }

        public override Exp[] GetSubExps()
        {
            return BodyExp.GetSubExps();
        }

        SymbolLocalVar retSymbol;
        Type NestedType { get; set; }
        public ConstructorBuilder NewBuilder { get; private set; }

        public override Exp Analy( )
        {
            CreateContext();
            AnalyFields();
            BodyExp.SetContext(NestedExpContext);
            BodyExp.SetIsNested(true);
            if (FnRetType == ZLangBasicTypes.ZBOOL)
            {
                retSymbol = new SymbolLocalVar("$RetResult", FnRetType);
            }
            return this;
        }

        internal ContextClass NestedClassContext;
        private ContextProc NestedProcContext;
        private StmtCall NestedStmt;
        private ContextExp NestedExpContext;

        private void CreateContext()
        {
            NestedClassContext = new ContextClass(this.ExpContext.FileContext);
            NestedClassContext.ClassName = this.ExpContext.ProcContext.CreateNestedClassName();

            NestedProcContext = new ContextProc(NestedClassContext);
            NestedProcContext.ProcName = NestedClassContext.ClassName + "$CALL";
            NestedProcContext.ProcManagerContext = NestedClassContext.ProcManagerContext;
            NestedProcContext.ProcManagerContext.AddContext(NestedProcContext);

            NestedStmt = new StmtCall();
            NestedStmt.ProcContext = NestedProcContext;

            NestedExpContext = new ContextExp(NestedProcContext,NestedStmt);
            BodyExp.SetContext(NestedExpContext);
            CreateEmitContext();
        }

        internal TypeBuilder ClassBuilder;
        internal MethodBuilder ProcBuilder;

        private void CreateEmitContext()
        {
            string fullName = this.ExpContext.FileContext.ProjectContext.ProjectModel.ProjectPackageName + "." + NestedClassContext.ClassName;
            TypeAttributes typeAttrs = TypeAttributes.NestedPrivate | TypeAttributes.Sealed;
            ClassBuilder = this.ExpContext.ClassContext.EmitContext.ClassBuilder.DefineNestedType(fullName, typeAttrs);
            NestedClassContext.EmitContext.ClassBuilder = ClassBuilder;

            NestedClassContext.EmitContext.IDoc = this.ExpContext.FileContext.ProjectContext.EmitContext.ModuleBuilder.DefineDocument(NestedClassContext.ClassName, Guid.Empty, Guid.Empty, Guid.Empty);
            ProcBuilder = NestedClassContext.EmitContext.ClassBuilder.DefineMethod(NestedProcContext.ProcName, MethodAttributes.Public | MethodAttributes.HideBySig, typeof(void), new Type[] { });
            NestedProcContext.EmitContext.SetBuilder(ProcBuilder);
            NestedProcContext.EmitContext.ILout = ProcBuilder.GetILGenerator();
        }

        public override void Emit()
        {
            var il = NestedProcContext.EmitContext.ILout;
            BodyExp.Emit();
            if (retSymbol == null)
            {
                if (BodyExp.RetType.SharpType != typeof(void))
                {
                    il.Emit(OpCodes.Pop);
                }
            }
            else
            {
                EmitHelper.StormVar(il, retSymbol.VarBuilder);
            }

            il.Emit(OpCodes.Ret);
            EmitConstructor();
            NestedType = NestedClassContext.EmitContext.ClassBuilder.CreateType();
            base.EmitConv();
        }

        private void EmitConstructor()
        {
            NewBuilder = NestedClassContext.EmitContext.ClassBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { });
            var il = NewBuilder.GetILGenerator();
            il.Emit(OpCodes.Ret);
        }
        string OutClassFieldName = "_$OutClass";
        private void AnalyFields( )
        {
            var symbols = NestedClassContext.Symbols;
            FieldSymbols = new List<SymbolDefField>();
            if (ExpContext.ClassContext.IsStaticClass == false)
            {
                TypeBuilder builder = ExpContext.ClassContext.EmitContext.ClassBuilder;
                FieldBuilder field = ClassBuilder.DefineField(OutClassFieldName,builder, FieldAttributes.Public);
                ZType ztype = new ZClassType(builder,builder,false);
                SymbolDefField fieldSymbol = new SymbolDefField(OutClassFieldName , ztype, false);
                fieldSymbol.Field = field;
                FieldSymbols.Add(fieldSymbol);
                symbols.Add(fieldSymbol);
                this.NestedClassContext.NestedOutFieldSymbol = fieldSymbol;
            }

            foreach (SymbolBase symbol in BodyVars)
            {
                ZType ztype = symbol.SymbolZType;
                FieldBuilder field = ClassBuilder.DefineField(symbol.SymbolName, ztype.SharpType, FieldAttributes.Public);
                SymbolDefField fieldSymbol = new SymbolDefField(symbol.SymbolName, ztype, false);
                fieldSymbol.Field = field;
                FieldSymbols.Add(fieldSymbol);
                symbols.Add(fieldSymbol);
            }

            foreach (ExpVar expVar in FieldExpVars)
            {
                SymbolDefField symbol = symbols.Get(expVar.VarName) as SymbolDefField;
                expVar.SetAsLambdaFiled(symbol);
            }
        }

        #region 辅助

       

        public override string ToString()
        {
            return BodyExp.ToString();
        }

        public override CodePosition Postion
        {
            get
            {
                return BodyExp.Postion; ;
            }
        }
        #endregion
    }
}
