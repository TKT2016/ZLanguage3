﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.ASTExps;
using ZCompileCore.Lex;
using ZCompileKit.Collections;
using ZNLP;
using System.Diagnostics;

namespace ZCompileCore.Parsers
{
    public class ChainParser
    {
        ArrayTape<object> Tape;
        ContextExp Context;
        Stack<ChainItemFeaturer> chains = new Stack<ChainItemFeaturer>();

        private void InitFields(IEnumerable<object> elements, ContextExp context)
        {
            Tape = new ArrayTape<object>(elements.ToArray());
            Context = context;

            //ItemParser = new ChainItemParser(context);
            chains.Clear();
            isParsedCurrCompPart = false;
        }

        private ChainItemFeaturer[] parseItems()
        {
            while (!Tape.IsEnd)
            {
                var obj = ParseItem();
                chains.Push(obj);
            }
            var objs = chains.ToList();
            objs.Reverse();
            return objs.ToArray();
        }

        public Exp Parse(IEnumerable<object> elements, ContextExp context)
        {
            InitFields(elements,context);

            ChainItemFeaturer[] objs = parseItems();

            int objSize = objs.Length;
            if (objSize  == 0)
            {
                return null;
            }
            else if (objSize == 1)
            {
                return Parse1(objs[0]);
            }
            else if (objSize == 2 && (objs[0].Data is ExpTypeBase) && IsArg(objs[1]))
            {
                return ParseToExpNew((ExpTypeBase)objs[0].Data, (Exp)objs[1].Data);
            }
            else
            {
                ExpCall callExp = new ExpCall(objs.Select(p => (Exp)p.Data));
                callExp.SetContext(this.Context);
                return callExp.Analy();
            }
        }

        private Exp Parse1(ChainItemFeaturer cif)
        {
            var data = cif.Data;
            if (data is ExpProcNamePart)
            {
                ExpProcNamePart dataExp = (ExpProcNamePart)data;
                LexToken tok = dataExp.PartNameToken;
                string text = tok.GetText();
                var ProcContext = this.Context.ProcContext;
                if (ProcContext.IsThisMethodSingle(text))
                {
                    ExpCallSingleThis pptExp = new ExpCallSingleThis(tok);
                    pptExp.SetContext(this.Context);
                    return pptExp.Analy();
                }
                else if (ProcContext.IsSuperMethodSingle(text))
                {
                    ExpCallSingleSuper pptExp = new ExpCallSingleSuper(tok);
                    pptExp.SetContext(this.Context);
                    return pptExp.Analy();
                }
                else if (ProcContext.IsUseMethodSingle(text))
                {
                    ExpCallSingleUse pptExp = new ExpCallSingleUse(tok);
                    pptExp.SetContext(this.Context);
                    return pptExp.Analy();
                }
                else
                {
                    return new ExpErrorToken(tok, dataExp.ExpContext);
                }
            }
            else if(cif.IsExp)
            {
                return (Exp)data;
            }
            else
            {
                return null;
            }
        }

        private Exp ParseToExpNew(ExpTypeBase expType, Exp exp2)
        {
            ExpBracket bracketBracket = WarpExp(exp2);
            ExpNew newexp = new ExpNew(expType, bracketBracket);
            newexp.SetContext(exp2.ExpContext);
            Exp exp3 = newexp.Analy();
            return exp3;
        }

        private bool IsArg(ChainItemFeaturer obj)
        {
            if (!(obj.Data is Exp)) return false;
            if ((obj.Data is ExpError)) return false;
            if ((obj.Data is ExpProcNamePart)) return false;
            return true;
        }

        private ChainItemFeaturer ParseItem()
        {
            ChainItemFeaturer cf = CurrentItem;// new ChainItemFeaturer(this.Context, Tape.Current);
            //if (CurrentItem.Data.ToString().IndexOf("W")!=-1)
            //{
            //    Console.WriteLine("W");
            //}
            object obj = null;
            //WordCompilePart curcp = ItemParser.ParseCompilePart(Tape.Current);
            if (cf.IsDe)//(CurrCompPart == WordCompilePart.de)
            {
                obj=ParseDe();
                //chains.Push(obj);
            }
            else if (cf.IsDi)//(CurrCompPart == WordCompilePart.di)
            {
                obj = ParseDi();
                //chains.Push(obj);
            }
            else if (cf.IsExp)//(CurrCompPart == WordCompilePart.exp)
            {
                obj = ParseItemExp();
                //chains.Push(obj);
            }
            else if(cf.IsLocalVar || cf.IsLiteral || cf.IsThisProperty || cf.IsSuperProperty
                || cf.IsUsedEnumItem|| cf.IsUsedProperty||cf.IsParameter|| cf.IsUsedField|| cf.IsThisField)
            {
                obj = ParseExpect_Var();
            }
            else if (cf.IsThisClassName || cf.IsImportTypeName)
            {
                Exp exp = ParseTypes();
                obj = exp;
                if (exp is ExpTypeUnsure)
                {
                    ExpTypeUnsure expType = (ExpTypeUnsure)exp;
                    if (!CurrentItem.IsNone && !CurrentItem.IsDe && !CurrentItem.IsDi  )
                    {
                        var nextObj = ParseItem();
                        if ((nextObj.Data is ExpBracket)
                            || (nextObj.Data is ExpLiteral)
                            || (nextObj.Data is ExpVarBase)
                            )
                        {
                            Exp newexp = ParseToExpNew(expType,(Exp)nextObj.Data);
                            obj = newexp;
                        }
                        else 
                        {
                            var obj2 = NewFeaturer(exp);
                            chains.Push(obj2);
                            obj = nextObj;
                        }
                    }
                }
            }
            else if (CurrentItem.IsText)
            {
                Exp exp = ParseStr();
                obj = exp;
            }
            else
            {
                throw new CCException();
            }
            return NewFeaturer(obj);
        }

        private ChainItemFeaturer NewFeaturer(object obj)
        {
            if (obj == null) return new ChainItemFeaturer();
            else if (obj is ChainItemFeaturer) return (ChainItemFeaturer)obj;
            else return new ChainItemFeaturer(this.Context,obj);
        }

        private ExpBracket WarpExp(Exp exp)
        {
            if (exp is ExpBracket) return exp as ExpBracket;
            ExpBracket exp2 = new ExpBracket(exp);
            exp.IsAnalyed = true;
            return exp2;
        }

        private Exp ParseStr()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpProcNamePart exp = new ExpProcNamePart(tok);
            exp.SetContext(this.Context);
            return exp;
        }

        private Exp ParseTypes()
        {
            List<LexToken> tokens = new List<LexToken> ();
                while(Tape.IsEnd ==false && (CurrentItem.IsThisClassName || CurrentItem.IsImportTypeName) )
                {
                    LexToken tok = (LexToken)Tape.Current;
                    tokens.Add(tok);
                    MoveNext();
                }
                ExpTypeUnsure exptype = new ExpTypeUnsure(tokens);
                exptype.SetContext(this.Context);
                return exptype.Analy();
        }

        private Exp ParseDe( )
        {
            ExpDe deexp = new ExpDe();
            
            deexp.KeyToken = (LexToken)Tape.Current;
            deexp.LeftExp = PopChainsExp();
            
            MoveNext();
            if(Tape.Current is LexToken)
            {
                LexToken tok =(LexToken)( Tape.Current);
                if (tok.Kind == TokenKind.Ident || tok.Kind == TokenKind.Each)
                {
                    deexp.RightToken = tok;
                    MoveNext();
                }
            }
            deexp.SetContext(this.Context);
            return deexp;
        }

        private Exp ParseDi( )
        {
            ExpDi diexp = new ExpDi();
            diexp.KeyToken = (LexToken)Tape.Current;
            diexp.SubjectExp = PopChainsExp();
            MoveNext();
            diexp.ArgExp = ParseExpect_Exp_Var();
            diexp.SetContext(this.Context);
            return diexp;
        }

        private Exp ParseExpect_Exp_Var()
        {
            Exp exp = null;
            if (CurrentItem.IsExp)// == WordCompilePart.exp)
            {
                exp = ParseItemExp();
            }
            else
            {
                exp = ParseExpect_Var();
            }
            return exp;
        }

        private Exp ParseExpect_Var()
        {
            if (CurrentItem.IsLocalVar)// == WordCompilePart.localvar)
            {
                return ParseLocalVar();
            }
            else if (CurrentItem.IsLiteral)// == WordCompilePart.literal)
            {
                return ParseLiteral();
            }
            else if (CurrentItem.IsParameter)// == WordCompilePart.arg)
            {
                return ParseArg();
            }
            else if (CurrentItem.IsThisProperty)// == WordCompilePart.property_this)
            {
                return ParsePropertyThis();
            }
            else if (CurrentItem.IsSuperProperty)// == WordCompilePart.property_base)
            {
                return ParsePropertySuper();
            }
            else if (CurrentItem.IsSuperField)
            {
                return ParseFieldSuper();
            }
            else if (CurrentItem.IsUsedEnumItem)// == WordCompilePart.enumitem_use)
            {
                return ParseEnumItemUse();
            }
            else if (CurrentItem.IsUsedProperty)// == WordCompilePart.property_use)
            {
                return ParsePropertyUse();
            }
            else if (CurrentItem.IsThisField)
            {
                return ParseFieldThis();
            }
            else if (CurrentItem.IsUsedField)
            {
                return ParseFieldUse();
            }
            return null;
        }

        private Exp ParseFieldThis()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpDefField exp2 = new ExpDefField(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParseFieldUse()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpUseField exp2 = new ExpUseField(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParseEnumItemUse()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpUseEnumItem exp2 = new ExpUseEnumItem(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParsePropertyUse()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpUseProperty exp2 = new ExpUseProperty(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParsePropertySuper()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpSuperProperty exp2 = new ExpSuperProperty(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParseFieldSuper()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpSuperField exp2 = new ExpSuperField(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParsePropertyThis()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpDefProperty exp2 = new ExpDefProperty(tok);
            exp2.SetContext(this.Context);
            return exp2.Analy();
        }

        private Exp ParseArg()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpArg argExp = new ExpArg(tok);
            argExp.SetContext(this.Context);
            return argExp.Analy();
        }

        private Exp ParseLiteral()
        {
            ExpLiteral expb = (ExpLiteral)Tape.Current;
            MoveNext();
            expb.SetContext(this.Context);
            return expb.Analy();
        }

        private Exp ParseItemExp()
        {
            if (Tape.Current is ExpBracket)
            {
                ExpBracket expb = (ExpBracket)Tape.Current;
                MoveNext();
                expb.SetContext(this.Context);
                return expb.Analy();
            }
            else if (Tape.Current is ExpLiteral)
            {
                ExpLiteral expb = (ExpLiteral)Tape.Current;
                MoveNext();
                expb.SetContext(this.Context);
                return expb.Analy();
            }
            throw new CCException();
        }

        private Exp ParseLocalVar()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpLocalVar expLocal = new ExpLocalVar(tok);
            expLocal.SetContext(this.Context);
            return expLocal.Analy();
        }

        bool isParsedCurrCompPart = false;
        private void MoveNext()
        {
            Tape.MoveNext();
            isParsedCurrCompPart = false;
        }

        ChainItemFeaturer _CurItem;
        private ChainItemFeaturer CurrentItem
        {
            get
            {
                if (!isParsedCurrCompPart)
                {
                    if (Tape.Current == null)
                    {
                        _CurItem = new ChainItemFeaturer();
                    }
                    else
                    {
                        _CurItem = new ChainItemFeaturer(this.Context, Tape.Current);
                    }
                   isParsedCurrCompPart = true;
                }
                return _CurItem;
            }
        }

        //private WordCompilePart CurrCompPart
        //{
        //    get
        //    {
        //        if (!isParsedCurrCompPart)
        //        { 
        //            if (Tape.Current == null) return WordCompilePart.none;
        //            _CurrCompPart = ItemParser.ParseCompilePart(Tape.Current);
        //            isParsedCurrCompPart = true;
        //        }
        //        return _CurrCompPart;
        //    }
        //}

        private ChainItemFeaturer PopChains()
        {
            if (this.chains.Count > 0)
            {
                return this.chains.Pop();
            }
            return null;
        }

        private ChainItemFeaturer PeekChains()
        {
            if (this.chains.Count > 0)
            {
                return this.chains.Peek();
            }
            return null;
        }

        private Exp PopChainsExp()
        {
            var obj = PeekChains();
            if (obj == null) return null;
            if (!(obj.IsExp)) return null;
            PopChains();
            return (Exp)obj.Data;
        }

    }
}
