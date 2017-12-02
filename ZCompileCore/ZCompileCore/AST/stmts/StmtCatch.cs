using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.Symbols;
using ZCompileCore.Tools;
using ZCompileDesc.Collections;
using ZCompileDesc.Descriptions;
using ZCompileDesc.ZTypes;
using ZCompileKit.Tools;

namespace ZCompileCore.AST
{
    public class StmtCatch:Stmt
    {
       public LexToken CatchToken { get; set; }
       public LexToken ExceptionTypeVarToken { get; set; }
       public StmtBlock CatchBody { get; set; }

       string exTypeName;
       string exName;
       ZType exType;
       SymbolLocalVar exSymbol;

       public override void Analy( )
       {
           //var retText = ExceptionTypeVarToken.GetText();
           TypeArgParser parser = new TypeArgParser(this.ClassContext);
           TypeArgParser.ParseResult result = parser.Parse(ExceptionTypeVarToken);
           if (result.ResultCount == 1)
           {
               //var tyedimNames = GetTypeWords();
               //var segMan = this.FileContext.SegManager;
               //NameTypeParser parser = new NameTypeParser(segMan.TypeNameDict, segMan.ArgSegementer);
               //NameTypeParser.ParseResult result = parser.ParseVar(ExceptionTypeVarToken);

               exTypeName = result.ArgZTypes[0].ZName;
               exType = result.ArgZTypes[0];
               exName = result.ArgName;
           }
           //var symbols = this.ProcContext.Symbols;
           //exTypeName = ExceptionTypeToken.GetText();
           //exType = ZTypeCache.GetByZName(exTypeName)[0];
           //if (exType == null)
           //{
           //    errorf(ExceptionTypeToken.Postion, "类型'{0}'不存在", exTypeName);
           //}
           //exName = ExceptionVarToken.GetText();
           //var exSymbol2 = symbols.Get(exName);
           if (this.ProcContext.ContainsVarName(exName)==false)// exSymbol2 == null)
           {
               exSymbol = new SymbolLocalVar(exName, exType);
               exSymbol.LoacalVarIndex =this.ProcContext.CreateLocalVarIndex(exName);
               this.ProcContext.AddDefSymbol(exSymbol);
           }
           else
           {
               if (this.ProcContext.IsDefLocal(exName))//if (exSymbol2 is SymbolLocalVar)
               {
                   exSymbol = this.ProcContext.GetDefLocal(exName);// exSymbol2 as SymbolLocalVar;
                   if (exSymbol.SymbolZType != exType)
                   {
                       ErrorF(ExceptionTypeVarToken.Position, "变量'{0}'的类型与异常的类型不一致", exName);
                   }
               }
               else
               {
                   ErrorF(ExceptionTypeVarToken.Position, "变量名称'{0}'已经使用过", exName);
               }
           }
           //symbols.Add(exSymbol);
           
           CatchBody.ProcContext = this.ProcContext;
           CatchBody.Analy();
       }

       public override void Emit( )
       {
           //MarkSequencePoint(context);
           IL.BeginCatchBlock(exType.SharpType);
           EmitHelper.StormVar(IL, exSymbol.VarBuilder);
           CatchBody.Emit();
           IL.EndExceptionBlock();
       }

       //private IWordDictionary GetTypeWords()
       //{
       //    WordDictionary dict = this.ProcContext.ClassContext.FileContext.ImportContext.TypeNameDict;
       //    //WordDictionaryList collect = new WordDictionaryList();
       //    //collect.Add(dict);
       //    return dict;
       //}

       #region 覆盖 
       public override string ToString()
       {
           StringBuilder buf = new StringBuilder();
           buf.Append(getStmtPrefix());
           //buf.AppendFormat("{0}({1}:{2})", CatchToken.GetText(), ExceptionTypeToken.GetText(), ExceptionVarToken.GetText());
           buf.AppendFormat("{0}({1})", CatchToken.GetText(), ExceptionTypeVarToken.GetText());
           buf.AppendLine();
           buf.Append(CatchBody.ToString());
           return buf.ToString();
       }

       public CodePosition Postion
       {
           get
           {
               return CatchToken.Position;
           }
       }
       #endregion

    }
}
