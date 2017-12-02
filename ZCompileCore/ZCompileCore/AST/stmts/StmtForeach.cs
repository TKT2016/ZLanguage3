using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parser;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileDesc;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZMembers;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;
using ZLangRT;
using ZLangRT.Utils;
using Z语言系统;

namespace ZCompileCore.AST
{
    public class StmtForeach:Stmt
    {
        public LexToken ForeachToken { get; set; }
        public Exp ListExp { get; set; }
        public LexToken ItemToken { get; set; }
        public StmtBlock Body { get; set; }

       SymbolLocalVar listSymbol;
       SymbolLocalVar itemSymbol;
       SymbolLocalVar indexSymbol;
       SymbolLocalVar countSymbol;

       MethodInfo getCountMethod;
       MethodInfo itemMethod;
       MethodInfo compareMethod;

       int startIndex;

       public override void Analy( )
       {
           AnalyListExp();
           CreateEachSymbols();
           Body.ProcContext = this.ProcContext;
           Body.Analy();

           //base.LoadRefTypes(context);
           //int foreachIndex =  context.MethodContext.CreateForeachIndex();
           if (ListExp == null)
           {
               ErrorF( ForeachToken.Position,"'循环每一个语句'不存在要循环的列表");
           }
           if (ItemToken == null)
           {
               ErrorF(ForeachToken.Position, "'循环每一个语句'不存在成员名称");
           }
           if (ListExp == null || ItemToken == null)
           {
               return;
           }

           //if (ListExp.RetType == null)
           //{
           //    TrueAnalyed = false;
           //    return;
           //}
           //else 
           if (!checkCanForeach(ListExp.RetType.SharpType))
           {
               ErrorF(ForeachToken.Position, "该结果不能用作循环每一个");
               return;
           }

           if (ReflectionUtil.IsExtends(ListExp.RetType.SharpType, typeof(列表<>)))
           {
               startIndex = 1;
               compareMethod = typeof(Calculater).GetMethod("LEInt", new Type[] { typeof(int), typeof(int) });
           }
           else
           {
               startIndex = 0;
               compareMethod = typeof(Calculater).GetMethod("LTInt", new Type[] { typeof(int), typeof(int) });
           }

           Body.ProcContext = this.ProcContext;
           Body.Analy();
       }

       bool checkCanForeach(Type type)
       {
           PropertyInfo countProperty = type.GetProperty("Count");
           PropertyInfo itemProperty = type.GetProperty("Item");
           if (countProperty != null && itemProperty!=null)
           {
               getCountMethod = countProperty.GetGetMethod();
               itemMethod = itemProperty.GetGetMethod();
               return true;
           }
           return false;
       }

       private void AnalyListExp()
       {
           ListExp = AnalyExpRaw();
           if (ListExp == null)
           {
               ErrorF(ForeachToken.Position, "循环每一个语句没有表达式");
           }
           else
           {
               ListExp = ListExp.Analy();
           }
       }

       private Exp AnalyExpRaw()
       {
           ExpRaw rawExp = (ExpRaw)ListExp;
           ContextExp context = new ContextExp(this.ProcContext, this);
           rawExp.SetContext(context);
           List<LexToken> tokens = rawExp.Seg();
           ExpParser parser = new ExpParser();
           Exp exp = parser.Parse(tokens, this.FileContext);
           exp.SetContext(rawExp.ExpContext);
           return exp;
       }

       protected void CreateEachSymbols()
       {
           var procContext = this.ProcContext;
           //var symbols = procContext.Symbols;
           int foreachIndex = procContext.CreateRepeatIndex();
           var indexName = "@foreach" + foreachIndex + "_index";
           var countName = "@foreach" + foreachIndex + "_count";
           var listName = "@foreach" + foreachIndex + "_list";
           var itemName = this.ItemToken.GetText();// "@foreach" + foreachIndex + "_item";

           indexSymbol = new SymbolLocalVar(indexName, ZLangBasicTypes.ZINT);
           indexSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(indexName);
           this.ProcContext.AddDefSymbol(indexSymbol);

           countSymbol = new SymbolLocalVar(countName, ZLangBasicTypes.ZINT);
           countSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(countName);
           this.ProcContext.AddDefSymbol(countSymbol);

           listSymbol = new SymbolLocalVar(listName,this.ListExp.RetType);
           listSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(listName);
           this.ProcContext.AddDefSymbol(listSymbol);

           var listType = this.ListExp.RetType.SharpType;
           Type[] genericTypes = GenericUtil.GetInstanceGenriceType(listType, typeof(列表<>));
           if (genericTypes.Length == 0)
           {
               genericTypes = GenericUtil.GetInstanceGenriceType(listType, typeof(IList<>));
           }

           Type ElementType = genericTypes[0];
           itemSymbol = new SymbolLocalVar(itemName, (ZType)(ZTypeManager.GetBySharpType(ElementType)));
           itemSymbol.LoacalVarIndex = procContext.CreateLocalVarIndex(itemName);
           this.ProcContext.AddDefSymbol(itemSymbol);
       }

       //int START_INDEX = 0;
       public override void Emit()
       {
           var True_Label = IL.DefineLabel();
           var False_Label = IL.DefineLabel();

           ListExp.Emit();
           EmitHelper.StormVar(IL, listSymbol.VarBuilder);

           generateCount();
           genInitIndex();

           EmitCondition();
           IL.Emit(OpCodes.Brfalse, False_Label);

           //定义一个标签，表示从下面开始进入循环体
           IL.MarkLabel(True_Label);
           emitItem();
           Body.Emit();
           EmitHelper.Inc(IL, indexSymbol.VarBuilder);
           EmitCondition();
           IL.Emit(OpCodes.Brtrue, True_Label);
           IL.MarkLabel(False_Label);
       }

       void emitItem( )
       {
           EmitHelper.LoadVar(IL, listSymbol.VarBuilder);
           EmitHelper.LoadVar(IL, indexSymbol.VarBuilder);
           EmitHelper.CallDynamic(IL, itemMethod);
           EmitHelper.StormVar(IL, itemSymbol.VarBuilder);
       }

       void genInitIndex( )
       {
           EmitHelper.LoadInt(IL, startIndex);
           EmitHelper.StormVar(IL, indexSymbol.VarBuilder);
       }

       void generateCount( )
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
           StringBuilder buf = new StringBuilder();
           buf.Append(getStmtPrefix());
           buf.AppendFormat("循环每一个( {0},{1} )", this.ListExp.ToString(),
               ItemToken.ToCode());
           //buf.AppendFormat("循环每一个( {0},{1},{2} )", this.ListExp.ToString(),
           //    ItemToken.ToCode(), IndexToken != null ? ("," + IndexToken.GetText()) : "");
           buf.AppendLine();
           buf.Append(Body.ToString());
           return buf.ToString();
       }
    }
}
