﻿using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileDesc.Descriptions;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
   public  class StmtWhile:Stmt
    {
       public Token DangToken { get; set; }
       public Token RepeatToken { get; set; }
       public Exp ConditionExp { get; set; }
       public StmtBlock WhileBody { get; set; }

       public override void Analy( )
       {
           ConditionExp = AnalyExpRaw();//Condition = AnalyCondition(Condition, DangToken.Position);
           ConditionExp = ConditionExp.Analy(); 
           WhileBody.ProcContext = this.ProcContext;
           WhileBody.Analy();
       }

       private Exp AnalyExpRaw()
       {
           ExpRaw rawExp = (ExpRaw)ConditionExp;
           ContextExp context = new ContextExp(this.ProcContext, this);
           rawExp.SetContext(context);
           List<Token> tokens = rawExp.WordSegment();
           if (tokens.Count > 0)
           {
               var lastIndex = tokens.Count - 1;
               RepeatToken = tokens[lastIndex];
               if (RepeatToken.GetText() == "重复" || RepeatToken.IsKeyIdent("重复"))
               {
                   tokens.RemoveAt(lastIndex);
               }
           }
           ExpParser parser = new ExpParser();
           Exp exp = parser.Parse(tokens, this.FileContext);
           exp.SetContext(rawExp.ExpContext);
           return exp;
       }

       public override void Emit()
       {
           var True_Label = IL.DefineLabel();
           var False_Label = IL.DefineLabel();

           IL.MarkLabel(True_Label);
           ConditionExp.Emit();
           EmitHelper.LoadInt(IL, 1);
           IL.Emit(OpCodes.Ceq);
           IL.Emit(OpCodes.Brfalse, False_Label);
           WhileBody.Emit();
           IL.Emit(OpCodes.Br, True_Label);
           IL.MarkLabel(False_Label);
       }

       public override string ToString()
       {
           StringBuilder buff = new StringBuilder();
           buff.AppendFormat("当{0}重复\n", ConditionExp);
           buff.AppendLine(WhileBody.ToString());
           return buff.ToString();
       } 
    }
}
