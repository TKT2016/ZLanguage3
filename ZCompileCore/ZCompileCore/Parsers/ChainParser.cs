using System;
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
        ChainItemParser ItemParser;
        Stack<object> chains = new Stack<object>();

        WordCompilePart _CurrCompPart ; 

        public Exp Parse(IEnumerable<object> elements, ContextExp context)
        {
            Tape = new ArrayTape<object>(elements.ToArray());
            Context = context;
            
            ItemParser = new ChainItemParser(context);
            chains.Clear();
            isParsedCurrCompPart = false;
            while(!Tape.IsEnd)
            {
                object obj = ParseItem();
                chains.Push(obj);
            }
            List<object> objs = chains.ToList();
            objs.Reverse();
           
            if(objs.Count==0)
            {
                return null;
            }
            else if (objs.Count == 1)
            {
                if(objs[0] is ExpProcNamePart)
                {
                    LexToken tok = (objs[0] as ExpProcNamePart).PartNameToken;
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
                        return new ExpErrorToken(tok, (objs[0] as ExpProcNamePart).ExpContext);
                    }
                }
                else
                {
                    return (Exp)objs[0];
                }
            }
            else if (objs.Count == 2 && (objs[0] is ExpTypeBase) && IsArg(objs[1]))
            {
                return ParseToExpNew((ExpTypeBase)objs[0], (Exp)objs[1]);
            }
            else
            {
                ExpCall callExp = new ExpCall(objs.Select(p => (Exp)p));
                callExp.SetContext(this.Context);
                return callExp.Analy();
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

        private bool IsArg(object obj)
        {
            if (!(obj is Exp)) return false;
            if ((obj is ExpError)) return false;
            if ((obj is ExpProcNamePart)) return false;
            return true;
        }

        private object ParseItem()
        {
            object obj = null;
            //if (Tape.Current.ToString().IndexOf("速度") != -1)
            //{
            //    Console.WriteLine("速度");
            //}
            //WordCompilePart curcp = ItemParser.ParseCompilePart(Tape.Current);
            if (CurrCompPart == WordCompilePart.de)
            {
                obj=ParseDe();
                //chains.Push(obj);
            }
            else if (CurrCompPart == WordCompilePart.di)
            {
                obj = ParseDi();
                //chains.Push(obj);
            }
            else if (CurrCompPart == WordCompilePart.exp)
            {
                obj = ParseItemExp();
                //chains.Push(obj);
            }
            else if (CurrCompPart == WordCompilePart.localvar
                || CurrCompPart == WordCompilePart.literal
                || CurrCompPart == WordCompilePart.property_this
                || CurrCompPart == WordCompilePart.property_base
                || CurrCompPart == WordCompilePart.enumitem_use
                || CurrCompPart == WordCompilePart.property_use
                 || CurrCompPart == WordCompilePart.arg
                )
            {
                obj = ParseExpect_Var();
                //chains.Push(obj);
            }
            else if (CurrCompPart == WordCompilePart.tname_this
                || CurrCompPart == WordCompilePart.tname_import)
            {
                Exp exp = ParseTypes();
                obj = exp;
                if (exp is ExpTypeUnsure)
                {
                    ExpTypeUnsure expType = (ExpTypeUnsure)exp;
                    if (CurrCompPart != WordCompilePart.de && CurrCompPart != WordCompilePart.di
                        && CurrCompPart!= WordCompilePart.none)
                    {
                        object nextObj = ParseItem();
                        if ((nextObj is ExpBracket)
                            || (nextObj is ExpLiteral)
                            || (nextObj is ExpVarBase)
                            )
                        {
                            Exp newexp = ParseToExpNew(expType,(Exp)nextObj);
                            obj = newexp;
                        }
                        else 
                        {
                            chains.Push(exp);
                            obj = nextObj;
                        }
                    }
                }
            }
            else if (CurrCompPart == WordCompilePart.str)
            {
                Exp exp = ParseStr();
                obj = exp;
            }
            else
            {
                throw new CCException();
            }
            return obj;
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
                while(Tape.IsEnd ==false && (CurrCompPart == WordCompilePart.tname_this
                || CurrCompPart == WordCompilePart.tname_import))
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
            if (CurrCompPart == WordCompilePart.exp)
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
            if (CurrCompPart == WordCompilePart.localvar)
            {
                return ParseLocalVar();
            }
            else if (CurrCompPart == WordCompilePart.literal)
            {
                return ParseLiteral();
            }
            else if (CurrCompPart == WordCompilePart.arg)
            {
                return ParseArg();
            }
            else if (CurrCompPart == WordCompilePart.property_this)
            {
                return ParsePropertyThis();
            }
            else if (CurrCompPart == WordCompilePart.property_base)
            {
                return ParsePropertyBase();
            }
            else if (CurrCompPart == WordCompilePart.enumitem_use)
            {
                return ParseEnumItemUse();
            }
            else if (CurrCompPart == WordCompilePart.property_use)
            {
                return ParsePropertyUse();
            }
            return null;
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

        private Exp ParsePropertyBase()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpSuperProperty exp2 = new ExpSuperProperty(tok);
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

        private WordCompilePart CurrCompPart
        {
            get
            {
                if (!isParsedCurrCompPart)
                { 
                    if (Tape.Current == null) return WordCompilePart.none;
                    //if (Tape.Current.ToString().IndexOf("敌人子弹") != -1)
                    //{
                    //    Console.WriteLine("敌人子弹");
                    //}
                    _CurrCompPart = ItemParser.ParseCompilePart(Tape.Current);
                    isParsedCurrCompPart = true;
                }
                return _CurrCompPart;
            }
        }

        private object PopChains()
        {
            if (this.chains.Count > 0)
            {
                return this.chains.Pop();
            }
            return null;
        }

        private object PeekChains()
        {
            if (this.chains.Count > 0)
            {
                return this.chains.Peek();
            }
            return null;
        }

        private Exp PopChainsExp()
        {
            object obj = PeekChains();
            if (obj == null) return null;
            if (!(obj is Exp)) return null;
            PopChains();
            return (Exp)obj;
        }

    }
}
