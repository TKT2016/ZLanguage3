using System;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using Z语言系统;
using ZCompileKit.Tools;
using ZCompileDesc.Utils;
using ZCompileDesc.ZTypes;
using ZLangRT;
using ZCompileDesc;

namespace ZCompileCore.AST
{
    public class ExpEach : Exp
    {
        public Exp SubjectExp;
        SymbolLocalVar ListSymbol;
        SymbolLocalVar IndexSymbol;
        SymbolLocalVar CountSymbol;
       
        ExpEachItem ItemExp;
        public Exp BodyExp { get; set; }

        protected MethodInfo LEMethod = typeof(Calculater).GetMethod(CompileConstant.Calculater_LEInt, new Type[] { typeof(int), typeof(int) });
        protected MethodInfo getCountMethod;

        public ExpEach(ContextExp expContext, Exp subjectExp)
        {
            ExpContext = expContext;
            SubjectExp = subjectExp;
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] { BodyExp };
        }

        public override Exp Analy()
        {
            SubjectExp.SetContext(this.ExpContext);
            SubjectExp = SubjectExp.Analy();
            if(SubjectExp.RetType==null)
            {
                Type newType =typeof(列表<>).MakeGenericType(typeof(object));
                ZType newZtype = ZTypeManager.RegNewGenericType(newType);
                SubjectExp.RetType = newZtype;
            }
            CreateEachSymbols();
            AnalyCountMethod();
            ItemExp = new ExpEachItem(this.ExpContext, this.ListSymbol, this.IndexSymbol);
            this.RetType = ZLangBasicTypes.ZVOID;
            return this;
        }

        private void AnalyCountMethod()
        {
            ZType subjectZType = SubjectExp.RetType;
            Type mainType = subjectZType.SharpType;
            PropertyInfo countProperty = mainType.GetProperty(CompileConstant.ZListCountPropertyName);//"Count");
            getCountMethod = countProperty.GetGetMethod();
        }

        public ExpEachItem GetItemExp()
        {
            return this.ItemExp;
        }

        protected void CreateEachSymbols()
        {
            var procContext = this.ExpContext.ProcContext;
            var symbols = procContext.Symbols;

            int foreachIndex = this.ExpContext.ProcContext.CreateEachIndex();
            var listSymbolName = "@each" + foreachIndex + "_list";
            var indexName = "@each" + foreachIndex + "_index";
            var elementName = "@each" + foreachIndex + "_item";
            var countName = "@each" + foreachIndex+"_count";

            ListSymbol = new SymbolLocalVar(listSymbolName, SubjectExp.RetType);
            ListSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(ListSymbol.SymbolName);
            symbols.Add(ListSymbol);

            Type[] genericTypes = GenericUtil.GetInstanceGenriceType(SubjectExp.RetType.SharpType, typeof(列表<>));
            Type ElementType = genericTypes[0];

            IndexSymbol = new SymbolLocalVar(indexName, ZLangBasicTypes.ZINT);
            IndexSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(indexName);
            symbols.Add(IndexSymbol);

            CountSymbol = new SymbolLocalVar(countName, ZLangBasicTypes.ZINT);
            CountSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(countName);
            symbols.Add(CountSymbol);
        }

        int START_INDEX = 1;
        public override void Emit()
        {
            SubjectExp.Emit();
            EmitHelper.StormVar(IL, ListSymbol.VarBuilder);
            EmitHelper.LoadInt(IL, START_INDEX);
            EmitHelper.StormVar(IL, IndexSymbol.VarBuilder);

            EmitHelper.LoadVar(IL, ListSymbol.VarBuilder);
            EmitHelper.CallDynamic(IL, getCountMethod);
            EmitHelper.StormVar(IL, CountSymbol.VarBuilder);

            var True_Label = IL.DefineLabel();
            var False_Label = IL.DefineLabel();

            EmitCondition();
            IL.Emit(OpCodes.Brfalse, False_Label);

            //定义一个标签，表示从下面开始进入循环体
            IL.MarkLabel(True_Label);
            BodyExp.Emit();
            if (BodyExp.RetType.SharpType != typeof(void))
            {
                IL.Emit(OpCodes.Pop);
            }
            EmitHelper.Inc(IL, IndexSymbol.VarBuilder);
            EmitCondition();
            IL.Emit(OpCodes.Brtrue, True_Label);
            IL.MarkLabel(False_Label); 
        }

        protected void EmitCondition( )
        {
            EmitHelper.LoadVar(IL, IndexSymbol.VarBuilder);
            EmitHelper.LoadVar(IL, CountSymbol.VarBuilder);
            EmitHelper.CallDynamic(IL, LEMethod);
            EmitHelper.LoadInt(IL, 1);
            IL.Emit(OpCodes.Ceq);
        }     

        public override string ToString()
        {
            return BodyExp.ToString();
        }
    }
}
