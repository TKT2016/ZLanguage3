using System;
using System.Reflection;
using System.Reflection.Emit;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Tools;
using ZLangRT.Utils;
using ZCompileDesc.Descriptions;
using Z语言系统;
using ZCompileDesc.Utils;
using ZLangRT;
using ZCompileDesc;

namespace ZCompileCore.AST.Exps
{
    public class ExpEach : Exp
    {
        private Exp _SubjectExp;
        public Exp SubjectExp { get { return _SubjectExp; } set { _SubjectExp = value; _SubjectExp.ParentExp = this; } }
        ZCLocalVar ListSymbol;
        ZCLocalVar IndexSymbol;
        ZCLocalVar CountSymbol;
       
        ExpEachItem ItemExp;
        public Exp BodyExp { get; set; }

        protected MethodInfo LEMethod = typeof(Calculater).GetMethod(CompileConst.Calculater_LEInt, new Type[] { typeof(int), typeof(int) });
        protected MethodInfo getCountMethod;

        public ExpEach(ContextExp expContext, Exp subjectExp)
            : base(expContext)
        {
            SubjectExp = subjectExp;
        }

        public override void SetParent(Exp parentExp)
        {
            ParentExp = parentExp;
            SubjectExp.SetParent(this);
            BodyExp.SetParent(this);
        }

        public override Exp[] GetSubExps()
        {
            return new Exp[] {SubjectExp, BodyExp };
        }

        public override Exp Analy()
        {
            if (this.IsAnalyed) return this;
            SubjectExp = SubjectExp.Analy();
            if(SubjectExp.RetType==null)
            {
                //Type newType =typeof(列表<>).MakeGenericType(typeof(object));
                //ZType newZtype = ZTypeManager.RegNewGenericType(newType);
                ZLClassInfo newZClass = ZTypeManager.MakeGenericType(ZLangBasicTypes.ZLIST, ZLangBasicTypes.ZOBJECT);
                SubjectExp.RetType = newZClass;
            }
            CreateEachSymbols();
            AnalyCountMethod();
            ItemExp = new ExpEachItem(this.ExpContext, this.ListSymbol, this.IndexSymbol);
            this.RetType = ZLangBasicTypes.ZVOID;
            IsAnalyed = true;
            return this;
        }

        private void AnalyCountMethod()
        {
            ZType subjectZType = SubjectExp.RetType;
            Type mainType = ZTypeUtil.GetTypeOrBuilder(subjectZType);// subjectZType.SharpType;
            PropertyInfo countProperty = mainType.GetProperty(ZLangUtil.ZListCountPropertyName);//"Count");
            getCountMethod = countProperty.GetGetMethod();
        }

        public ExpEachItem GetItemExp()
        {
            return this.ItemExp;
        }

        protected void CreateEachSymbols()
        {
            var procContext = this.ExpContext.ProcContext;
            int foreachIndex = this.ExpContext.ProcContext.CreateEachIndex();
            var listSymbolName = "@each" + foreachIndex + "_list";
            var indexName = "@each" + foreachIndex + "_index";
            var elementName = "@each" + foreachIndex + "_item";
            var countName = "@each" + foreachIndex+"_count";

            ListSymbol = new ZCLocalVar(listSymbolName, SubjectExp.RetType,true);
            //ListSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(ListSymbol.ZName);
            this.ProcContext.AddLocalVar(ListSymbol);

            Type[] genericTypes = GenericUtil.GetInstanceGenriceType(ZTypeUtil.GetTypeOrBuilder(SubjectExp.RetType), typeof(列表<>));
            //Type[] genericTypes = GenericUtil.GetInstanceGenriceType(SubjectExp.RetType.SharpType, typeof(列表<>));
            Type ElementType = genericTypes[0];

            IndexSymbol = new ZCLocalVar(indexName, ZLangBasicTypes.ZINT, true);
            //IndexSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(indexName);
            this.ProcContext.AddLocalVar(IndexSymbol);

            CountSymbol = new ZCLocalVar(countName, ZLangBasicTypes.ZINT, true);
            //CountSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(countName);
            this.ProcContext.AddLocalVar(CountSymbol);
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
            if (ZTypeUtil.IsBool(BodyExp.RetType))//(BodyExp.RetType.SharpType != typeof(void))
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
