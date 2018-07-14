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
using ZCompileCore.AST.Exps;
using ZCompileCore.ASTRaws;

namespace ZCompileCore.AST
{
    public class StmtRepeat: Stmt
    {
        private StmtRepeatRaw Raw;
        private StmtBlock RepeatBody;
        private Exp TimesExp;

        public StmtRepeat(StmtRepeatRaw raw, Stmt parentStmt)
        {
            Raw = raw;
            ParentStmt = parentStmt;
            RepeatBody = new StmtBlock(this, raw.RepeatBody);
        }

        ZCLocalVar IndexSymbol;
        ZCLocalVar CountSymbol;
        ZCLocalVar CondiSymbol;
        protected MethodInfo LTMethod = typeof(Calculater).GetMethod(CompileConst.Calculater_LTInt, new Type[] { typeof(int), typeof(int) });

        public override Stmt Analy()
        {
            //TimesExp.IsTopExp = true;
            TimesExp = AnalyRepeateExpRaw(Raw.TimesExp);// ParseAnalyRawExp(Raw.TimesExp);
            if (TimesExp == null)
            {
                Errorf(Raw.RepeatToken.Position, "重复语句没有表达式");
            }
            else
            {
                TimesExp = TimesExp.Analy();
                if (TimesExp != null && TimesExp.AnalyCorrect)
                {
                    if (!ZTypeUtil.IsInt(TimesExp.RetType))
                    {
                        Errorf(TimesExp.Position, "结果不是整数");
                    }
                }
            }
            CreateEachSymbols();
            //RepeatBody.ProcContext = this.ProcContext;
            RepeatBody.Analy();
            return this;
        }

        public override void AnalyExpDim()
        {
            TimesExp.AnalyDim();
            RepeatBody.AnalyExpDim();
        }

        private Exp AnalyRepeateExpRaw(ExpRaw rawExp)
        {
            //ExpRaw rawExp = (ExpRaw)TimesExp;
            //ContextExp context = new ContextExp(this.ProcContext, this);
            //rawExp.SetContext(context);
            List<LexToken> tokens = rawExp.RawTokens;
            if (tokens.Count > 0)
            {
                var lastIndex = tokens.Count - 1;
                var TimesToken = tokens[lastIndex];
                if (TimesToken is LexTokenText && TimesToken.Text == "次")//|| TimesToken.IsKeyIdent("次"))
                {
                    tokens.RemoveAt(lastIndex);
                }
                else
                {
                    Errorf(TimesToken.Position, "循环语句的条件末尾缺少‘次’");
                }
            }
            //var lastIndex = tokens.Count - 1;
            //tokens.RemoveAt(lastIndex);
            //ExpParser parser = new ExpParser();
            //Exp exp = parser.Parse(tokens, this.FileContext);
            //exp.SetContext(rawExp.ExpContext);
            Exp exp = ParseAnalyRawExp(rawExp);
            return exp;
        }

        protected void CreateEachSymbols()
        {
            var procContext = this.ProcContext;

            int foreachIndex = procContext.CreateRepeatIndex();
            var indexName = "@loop" + foreachIndex + "_index";
            var countName = "@loop" + foreachIndex + "_count";
            var condiName = "@loop" + foreachIndex + "_bool";

            IndexSymbol = new ZCLocalVar(indexName, ZLangBasicTypes.ZINT, true);
            //IndexSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(indexName);
            this.ProcContext.AddLocalVar(IndexSymbol);

            CountSymbol = new ZCLocalVar(countName, ZLangBasicTypes.ZINT, true);
            //CountSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(countName);
            this.ProcContext.AddLocalVar(CountSymbol);

            CondiSymbol = new ZCLocalVar(condiName, ZLangBasicTypes.ZBOOL, true);
            //CondiSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(condiName);
            this.ProcContext.AddLocalVar(CondiSymbol);
        }

        int START_INDEX = 0;
        public override void Emit()
        {
            TimesExp.Emit();
            EmitHelper.StormVar(IL, CountSymbol.VarBuilder);

            EmitHelper.LoadInt(IL, START_INDEX);
            EmitHelper.StormVar(IL, IndexSymbol.VarBuilder);

            var True_Label = IL.DefineLabel();
            var False_Label = IL.DefineLabel();

            EmitCondition();
            IL.Emit(OpCodes.Brfalse, False_Label);

            //定义一个标签，表示从下面开始进入循环体
            IL.MarkLabel(True_Label);
            RepeatBody.Emit();
            EmitHelper.Inc(IL, IndexSymbol.VarBuilder);
            EmitCondition();

            IL.Emit(OpCodes.Brtrue, True_Label);
            IL.MarkLabel(False_Label);
        }

        protected void EmitCondition()
        {
            EmitHelper.LoadVar(IL, IndexSymbol.VarBuilder);
            EmitHelper.LoadVar(IL, CountSymbol.VarBuilder);
            IL.Emit(OpCodes.Clt);
            EmitHelper.StormVar(IL, CondiSymbol.VarBuilder);
            EmitHelper.LoadVar(IL, CondiSymbol.VarBuilder);
        }

        public override string ToString()
        {
            return Raw.ToString();
        }
    }
}
