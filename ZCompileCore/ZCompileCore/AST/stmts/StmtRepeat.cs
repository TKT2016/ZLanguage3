﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.ASTExps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parser;
using ZCompileCore.Parsers;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.Utils;
using ZCompileKit.Tools;
using ZLangRT;

namespace ZCompileCore.AST
{
    public class StmtRepeat:Stmt
    {
       public LexToken RepeatToken { get; set; }
       public LexToken TimesToken { get; set; }
       public Exp TimesExp { get; set; }
       public StmtBlock RepeatBody { get; set; }

       ZCLocalVar IndexSymbol;
       ZCLocalVar CountSymbol;
       ZCLocalVar CondiSymbol;
       protected MethodInfo LTMethod = typeof(Calculater).GetMethod(CompileConst.Calculater_LTInt, new Type[] { typeof(int), typeof(int) });

       public override void DoAnaly()
       {
           TimesExp.IsTopExp = true;
           TimesExp = AnalyExpRaw(); 
           if (TimesExp == null)
           {
               ErrorF(RepeatToken.Position, "重复语句没有表达式");
           }
           else
           {
               TimesExp = TimesExp.Analy();
               if(TimesExp!=null &&TimesExp.AnalyCorrect)
               {
                   if (!ZTypeUtil.IsInt(TimesExp.RetType))
                   {
                       ErrorF(TimesExp.Position, "结果不是整数");
                   }
               }
           }
           CreateEachSymbols();
           RepeatBody.ProcContext = this.ProcContext;
           RepeatBody.Analy();
       }

       private Exp AnalyExpRaw()
       {
           ExpRaw rawExp = (ExpRaw)TimesExp;
           ContextExp context = new ContextExp(this.ProcContext, this);
           rawExp.SetContext(context);
           List<LexToken> tokens = rawExp.Seg();
           if (tokens.Count > 0)
           {
               var lastIndex = tokens.Count - 1;
               TimesToken = tokens[lastIndex];
               if (TimesToken.GetText() == "次" || TimesToken.IsKeyIdent("次"))
               {
                   tokens.RemoveAt(lastIndex);
               }
           }
           //var lastIndex = tokens.Count - 1;
           //tokens.RemoveAt(lastIndex);
           ExpParser parser = new ExpParser();
           Exp exp = parser.Parse(tokens, this.FileContext);
           exp.SetContext(rawExp.ExpContext);
           return exp;
       }

       protected void CreateEachSymbols()
       {
           var procContext = this.ProcContext;

           int foreachIndex = procContext.CreateRepeatIndex();
           var indexName = "@repeat" + foreachIndex + "_index";
           var countName = "@repeat" + foreachIndex + "_count";
           var condiName = "@repeat" + foreachIndex + "_bool";

           IndexSymbol = new ZCLocalVar(indexName, ZLangBasicTypes.ZINT);
           IndexSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(indexName);
           this.ProcContext.AddLocalVar(IndexSymbol);

           CountSymbol = new ZCLocalVar(countName, ZLangBasicTypes.ZINT);
           CountSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(countName);
           this.ProcContext.AddLocalVar(CountSymbol);

           CondiSymbol = new ZCLocalVar(condiName, ZLangBasicTypes.ZBOOL);
           CondiSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(condiName);
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
           StringBuilder buff = new StringBuilder();
           buff.AppendFormat("重复{0}次\n", TimesExp);
           buff.AppendLine(RepeatBody.ToString());
           return buff.ToString();
       } 
    }
}
