using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.AST.Exps;
using ZCompileCore.CommonCollections;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;

namespace ZCompileCore.Parsers.Exps
{
    public class ChainItemParser
    {
        ArrayTape<object> Tape;
        ContextExp expContext;
        Stack<object> chains = new Stack<object>();
        ChainItemFeaturer featurer;

        public object[] ParseItems(IEnumerable<object> elements, ContextExp context)
        {
            Tape = new ArrayTape<object>(elements.ToArray());
            expContext = context;
            chains.Clear();
            featurer = new ChainItemFeaturer(context);

            while (Tape.HasCurrent)
            {
                bool b= ParseItem();
            }

            return chains.Reverse().ToArray();
        }

        private bool ParseItem()
        {
            if(!Tape.HasCurrent)
            {
                return false;
            }

            var data = Tape.Current;
            if (featurer.IsDe(data))
            {
                var obj = ParseDe();
                chains.Push(obj);
                return true;
            }
            else if (featurer.IsNewfault(data))// (cf.IsNewfault)
            {
                var obj = ParseNewfault();
                chains.Push(obj);
                return true;
                //MoveNext();
                //return false;
            }
            else if (featurer.IsDi(data))//(cf.IsDi)
            {
               var obj = ParseDi();
               chains.Push(obj);
               return true;
            }
            else if (featurer.IsExp(data))//if (cf.IsExp)
            {
               var obj = ParseItemExp();
               chains.Push(obj);
               return true;
            }
            else if (featurer.IsLocalVar(data)//cf.IsLocalVar 
                || featurer.IsLiteral(data)//cf.IsLiteral 
                || featurer.IsThisProperty(data)
                || featurer.IsSuperProperty(data)//cf.IsSuperProperty
                || featurer.IsUsedEnumItem(data)//cf.IsUsedEnumItem 
                || featurer.IsUsedProperty(data)// cf.IsUsedProperty 
                || featurer.IsParameter(data)//cf.IsParameter 
                || featurer.IsUsedField(data)
                || featurer.IsThisField(data)
                )
            {
               Exp exp1 = ParseExpect_Var();
               ExpBracket bracketBracket = WarpExp(exp1);
               chains.Push(bracketBracket);
               return true;
            }
            
            else if (featurer.IsThisClassName(data) || featurer.IsImportTypeName(data))
            {
                Exp exp = ParseTypes();
                chains.Push(exp);  

                if (exp is ExpTypeBase)
                {
                    if(Tape.HasCurrent)
                    {
                        var data2 = Tape.Current;
                        if(!(featurer.IsDi(data2) || featurer.IsDe(data2)))
                        {
                            var b2 = ParseItem();
                            if(b2)
                            {
                                var nextObj = PeekChains();
                                if ((nextObj is ExpBracket)
                                    || (nextObj is ExpLiteral)
                                    || (nextObj is ExpVarBase)
                                )
                                {
                                    var argExp = (Exp)PopChains();
                                    var typeExp = (ExpTypeBase)PopChains();
                                    Exp newexp = ParseToExpNew(typeExp, argExp);
                                    //obj = newexp;
                                    chains.Push(newexp);
                                    return true;
                                }
                            }                           
                        }
                    }
                    return true;
                }
            }
            else if (featurer.IsIdent(data))
            {
                LexTokenText lexToken = (LexTokenText)data;
                if (lexToken.Text == "是" || lexToken.Text == "否")
                {
                    LexTokenLiteral literalToken = new LexTokenLiteral(lexToken.Line, lexToken.Col,
                        lexToken.Text == "是" ? TokenKindLiteral.True : TokenKindLiteral.False, lexToken.Text);
                    ExpLiteral literalExp = new ExpLiteral(this.expContext, literalToken);
                    Exp exp2 = literalExp.Analy();
                    chains.Push(exp2);
                    MoveNext();
                }
                else
                {
                    Exp exp = ParseProcNamePart();
                    chains.Push(exp);
                }
                return true;
            }
            else
            {
                throw new CCException();
            }
            return true;
        }

        private Exp ParseToExpNew(ExpTypeBase expType, Exp exp2)
        {
            ExpBracket bracketBracket = WarpExp(exp2);
            if(bracketBracket.IsExpBracketTagNew())
            {
                bracketBracket = bracketBracket.AnalyToTagNew();
            }
            ExpNew newexp = new ExpNew(this.expContext,expType, bracketBracket);
            //newexp.SetContextExp(this.expContext);
            Exp exp3 = newexp.Analy();
            return exp3;
        }

        private ExpBracket WarpExp(Exp exp)
        {
            if (exp is ExpBracket) return exp as ExpBracket;
            ExpBracketWrapOne exp2 = new ExpBracketWrapOne(this.expContext, exp, true);
            return exp2;
        }

        private Exp ParseProcNamePart()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpProcNamePart exp = new ExpProcNamePart(this.expContext,tok);
            //ExpBracket bracketexp = WarpExp(exp);
            //exp.SetContextExp(this.expContext);
            return exp;
        }

        private Exp ParseTypes()
        {
            List<LexToken> tokens = new List<LexToken>();
            while (Tape.HasCurrent)
            {
                var data = Tape.Current;
                if (featurer.IsThisClassName(data) || featurer.IsImportTypeName(data))
                {
                    LexToken tok = (LexToken)Tape.Current;
                    tokens.Add(tok);
                }
                else
                {
                    break;
                }
                MoveNext();
            }
            ExpTypeUnsure exptype = new ExpTypeUnsure(this.expContext,tokens);
            //exptype.SetContextExp(this.expContext);
            return exptype.Analy();
        }

        private Exp ParseDe()
        {
            ExpDe deexp = new ExpDe(this.expContext);

            deexp.KeyToken = (LexTokenText)Tape.Current;
            deexp.SubjectExp = PopChainsExp();

            MoveNext();
            if (Tape.Current is LexTokenText)
            {
                LexTokenText tok = (LexTokenText)(Tape.Current);
                if (tok.Kind == TokenKindKeyword.Ident || tok.Kind == TokenKindKeyword.Each)
                {
                    deexp.RightToken = tok;
                    MoveNext();
                }
            }
            //deexp.SetContextExp(this.expContext);
            return deexp;
        }

        private Exp ParseNewfault()
        {
            ExpTagNew deexp = new ExpTagNew(this.expContext, (LexTokenText)Tape.Current);
            //deexp.KeyToken = (LexTokenText)Tape.Current;
            MoveNext();
            return deexp;
        }

        private Exp ParseDi()
        {
            ExpDi diexp = new ExpDi(this.expContext);
            diexp.KeyToken = (LexTokenText)Tape.Current;
            diexp.SubjectExp = PopChainsExp();
            MoveNext();
            diexp.ArgExp = ParseExpect_Exp_Var();
            if(Tape.Current is LexToken)
            {
                LexToken tok1 = (LexToken)Tape.Current;
                if(tok1.IsKind( TokenKindKeyword.Ge ))
                {
                    diexp.GeToken = (LexTokenText)tok1;
                    MoveNext();
                }
                else
                {
                    this.expContext.FileContext.Errorf(diexp.KeyToken.Position, "‘的’后面缺少‘个’匹配");
                }
            }
            else
            {
                this.expContext.FileContext.Errorf(diexp.KeyToken.Position, "‘的’后面缺少‘个’匹配");
            }
            var data = Tape.Current;
            if (featurer.IsThisClassName(data) || featurer.IsImportTypeName(data))
            {
                Exp exp = ParseTypes();
                diexp.ElementTypeExp = exp;
            }
            else
            {
                this.expContext.FileContext.Errorf(diexp.KeyToken.Position, "‘的’后面缺少元素类型名称");
            }

            return diexp;
        }

        private Exp ParseExpect_Exp_Var()
        {
            Exp exp = null;
            if ( featurer.IsExp(Tape.Current))
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
            var data = Tape.Current;
             
            if ( featurer.IsLocalVar(data))// CurrentItem.IsLocalVar)
            {
                return ParseLocalVar();
            }
            else if (featurer.IsLiteral(data))//(CurrentItem.IsLiteral)
            {
                return ParseLiteral();
            }
            else if (featurer.IsParameter(data))//(CurrentItem.IsParameter)
            {
                return ParseArg();
            }
            else if (featurer.IsThisProperty(data))//(CurrentItem.IsThisProperty)
            {
                return ParsePropertyThis();
            }
            else if (featurer.IsSuperProperty(data))//(CurrentItem.IsSuperProperty)
            {
                return ParsePropertySuper();
            }
            else if (featurer.IsSuperField(data))//(CurrentItem.IsSuperField)
            {
                return ParseFieldSuper();
            }
            else if (featurer.IsUsedEnumItem(data))//(CurrentItem.IsUsedEnumItem)
            {
                return ParseEnumItemUse();
            }
            else if (featurer.IsUsedProperty(data))//(CurrentItem.IsUsedProperty)
            {
                return ParsePropertyUse();
            }
            else if (featurer.IsThisField(data))//(CurrentItem.IsThisField)
            {
                return ParseFieldThis();
            }
            else if (featurer.IsUsedField(data))//(CurrentItem.IsUsedField)
            {
                return ParseFieldUse();
            }
            return null;
        }

        private Exp ParseFieldThis()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpFieldDef exp2 = new ExpFieldDef(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParseFieldUse()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpUseField exp2 = new ExpUseField(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParseEnumItemUse()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpUseEnumItem exp2 = new ExpUseEnumItem(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParsePropertyUse()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpUseProperty exp2 = new ExpUseProperty(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParsePropertySuper()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpPropertySuper exp2 = new ExpPropertySuper(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParseFieldSuper()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpFieldSuper exp2 = new ExpFieldSuper(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParsePropertyThis()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpPropertyDef exp2 = new ExpPropertyDef(this.expContext,tok);
            //exp2.SetContextExp(this.expContext);
            return exp2.Analy();
        }

        private Exp ParseArg()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpArg argExp = new ExpArg(this.expContext,tok);
            //argExp.SetContextExp(this.expContext);
            return argExp.Analy();
        }

        private Exp ParseLiteral()
        {
            ExpLiteral expb = (ExpLiteral)Tape.Current;
            MoveNext();
            //expb.SetContextExp(this.expContext);
            return expb.Analy();
        }

        private Exp ParseItemExp()
        {
            if (Tape.Current is ExpBracket)
            {
                ExpBracket expb = (ExpBracket)Tape.Current;
                MoveNext();
                //expb.SetContextExp(this.expContext);
                return expb.Analy();
            }
            else if (Tape.Current is ExpLiteral)
            {
                ExpLiteral expb = (ExpLiteral)Tape.Current;
                MoveNext();
                //expb.SetContextExp(this.expContext);
                return expb.Analy();
            }
            throw new CCException();
        }

        private Exp ParseLocalVar()
        {
            LexToken tok = (LexToken)Tape.Current;
            MoveNext();
            ExpLocalVar expLocal = new ExpLocalVar(this.expContext , tok);
            //expLocal.SetContextExp(this.expContext);
            return expLocal.Analy();
        }

        //private bool isParsedCurrCompPart = false;
        //bool isParsedCurrCompPart = false;
        private void MoveNext()
        {
            Tape.MoveNext();
            //isParsedCurrCompPart = false;
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
            var obj = PeekChains();
            if (obj == null) return null;
            if (!(obj is Exp)) return null;
            PopChains();
            return (Exp)obj;
        }
    }
}
