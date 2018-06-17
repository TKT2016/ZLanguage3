using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileCore.Tools;
using ZLangRT;
using ZLangRT.Utils;
using Z语言系统;
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;
using ZCompileCore.Parsers.Exps;

namespace ZCompileCore.AST
{
    public class StmtForeach:Stmt
    {
        private StmtForeachRaw Raw;
        private StmtBlock StmtBody;
        private Exp ForeachListExp;

        public StmtForeach(StmtForeachRaw raw, Stmt parentStmt)
        {
            Raw = raw;
            ParentStmt = parentStmt;

            StmtBody = new StmtBlock(this,raw.Body);
        }

        ZCLocalVar listSymbol;
        ZCLocalVar itemSymbol;
        ZCLocalVar indexSymbol;
        ZCLocalVar countSymbol;

        MethodInfo getCountMethod;
        MethodInfo itemMethod;
        MethodInfo compareMethod;

        int startIndex;

        public override Stmt Analy()
        {
            ForeachListExp = ParseAnalyRawExp(Raw.ListExp);
            CreateEachSymbols();
            var ForeachToken = Raw.LoopToken;
            var ListExp = Raw.ListExp;
            var ItemToken = Raw.ItemToken;

            if (ListExp == null)
            {
                Errorf(ForeachToken.Position, "'循环每一个语句'不存在要循环的列表");
            }
            if (ItemToken == null)
            {
                Errorf(ForeachToken.Position, "'循环每一个语句'不存在成员名称");
            }
            if (ListExp == null || ItemToken == null)
            {
                return null;
            }

            if (!checkCanForeach(ForeachListExp.RetType))
            {
                Errorf(ForeachToken.Position, "该结果不能用作循环每一个");
                return null;
            }

            if (ZTypeUtil.IsExtends(ForeachListExp.RetType, typeof(列表<>)))
            {
                startIndex = 1;
                compareMethod = typeof(Calculater).GetMethod("LEInt", new Type[] { typeof(int), typeof(int) });
            }
            else
            {
                startIndex = 0;
                compareMethod = typeof(Calculater).GetMethod("LTInt", new Type[] { typeof(int), typeof(int) });
            }
            StmtBody.Analy();
            //Body.ProcContext = this.ProcContext;
            //Body.Analy();
            return this;
        }

        public override void AnalyExpDim()
        {
            ForeachListExp.AnalyDim();
            StmtBody.AnalyExpDim();
            
        }

        private bool checkCanForeach(ZType ztype)
        {
            if (ztype is ZLType)
            {
                Type type = ((ZLType)ztype).SharpType;
                return checkCanForeach(type);
            }
            return false;
        }

        private bool checkCanForeach(Type type)
        {
            PropertyInfo countProperty = type.GetProperty("Count");
            PropertyInfo itemProperty = type.GetProperty("Item");
            if (countProperty != null && itemProperty != null)
            {
                getCountMethod = countProperty.GetGetMethod();
                itemMethod = itemProperty.GetGetMethod();
                return true;
            }
            return false;
        }

        //private void AnalyListExp()
        //{
        //    ListExp = AnalyExpRaw();
        //    if (ListExp == null)
        //    {
        //        Errorf(ForeachToken.Position, "循环每一个语句没有表达式");
        //    }
        //    else
        //    {
        //        ListExp = ListExp.Analy();
        //    }
        //}

        //private Exp AnalyExpRaw()
        //{
        //    ExpRaw rawExp = Raw.ListExp;// (Exp)ListExp;
        //    ContextExp context = new ContextExp(this.ProcContext, this);
        //    //rawExp.SetContext(context);
        //    List<LexToken> tokens = rawExp.Seg();
        //    ExpParser parser = new ExpParser();
        //    Exp exp = parser.Parse(tokens, this.ProcContext.ClassContext.FileContext);
        //    //exp.SetContext(rawExp.ExpContext);
        //    return exp;
        //}

        protected void CreateEachSymbols()
        {
            var procContext = this.ProcContext;

            int foreachIndex = procContext.CreateRepeatIndex();
            var indexName = "@foreach" + foreachIndex + "_index";
            var countName = "@foreach" + foreachIndex + "_count";
            var listName = "@foreach" + foreachIndex + "_list";
            var itemName = this.Raw.ItemToken.Text;

            indexSymbol = new ZCLocalVar(indexName, ZLangBasicTypes.ZINT, true);
            //indexSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(indexName);
            this.ProcContext.AddLocalVar(indexSymbol);

            countSymbol = new ZCLocalVar(countName, ZLangBasicTypes.ZINT, true);
            //countSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(countName);
            this.ProcContext.AddLocalVar(countSymbol);

            listSymbol = new ZCLocalVar(listName, this.ForeachListExp.RetType, true);
            //listSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(listName);
            this.ProcContext.AddLocalVar(listSymbol);

            var listType = ZTypeUtil.GetTypeOrBuilder(this.ForeachListExp.RetType);
            Type[] genericTypes = GenericUtil.GetInstanceGenriceType(listType, typeof(列表<>));
            if (genericTypes.Length == 0)
            {
                genericTypes = GenericUtil.GetInstanceGenriceType(listType, typeof(IList<>));
            }

            Type ElementType = genericTypes[0];
            itemSymbol = new ZCLocalVar(itemName, (ZType)(ZTypeManager.GetBySharpType(ElementType)), true);
            //itemSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(itemName);
            this.ProcContext.AddLocalVar(itemSymbol);
        }

        //int START_INDEX = 0;
        public override void Emit()
        {
            var True_Label = IL.DefineLabel();
            var False_Label = IL.DefineLabel();

            ForeachListExp.Emit();
            EmitHelper.StormVar(IL, listSymbol.VarBuilder);

            generateCount();
            genInitIndex();

            EmitCondition();
            IL.Emit(OpCodes.Brfalse, False_Label);

            //定义一个标签，表示从下面开始进入循环体
            IL.MarkLabel(True_Label);
            emitItem();
            StmtBody.Emit();
            EmitHelper.Inc(IL, indexSymbol.VarBuilder);
            EmitCondition();
            IL.Emit(OpCodes.Brtrue, True_Label);
            IL.MarkLabel(False_Label);
        }

        void emitItem()
        {
            EmitHelper.LoadVar(IL, listSymbol.VarBuilder);
            EmitHelper.LoadVar(IL, indexSymbol.VarBuilder);
            EmitHelper.CallDynamic(IL, itemMethod);
            EmitHelper.StormVar(IL, itemSymbol.VarBuilder);
        }

        void genInitIndex()
        {
            EmitHelper.LoadInt(IL, startIndex);
            EmitHelper.StormVar(IL, indexSymbol.VarBuilder);
        }

        void generateCount()
        {
            EmitHelper.LoadVar(IL, listSymbol.VarBuilder);
            EmitHelper.CallDynamic(IL, getCountMethod);
            EmitHelper.StormVar(IL, countSymbol.VarBuilder);
        }

        protected void EmitCondition()
        {
            EmitHelper.LoadVar(IL, indexSymbol.VarBuilder);
            EmitHelper.LoadVar(IL, countSymbol.VarBuilder);
            EmitHelper.CallDynamic(IL, compareMethod);
            EmitHelper.LoadInt(IL, 1);
            IL.Emit(OpCodes.Ceq);
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}
