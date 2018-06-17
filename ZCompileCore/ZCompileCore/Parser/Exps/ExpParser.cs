using System;
using System.Collections.Generic;
using ZCompileCore.AST;
using ZCompileCore.Contexts;
using ZCompileCore.AST.Exps;
using ZCompileCore.Lex;
using ZCompileCore.Reports;
using ZCompileCore.Tools;
using ZCompileCore.CommonCollections;

namespace ZCompileCore.Parsers.Exps
{
    public class ExpParser
    {
        private TokenTape tape;
        private ContextExp expContext;
        public Exp Parse(List<LexToken> tokens, ContextExp expContext)
        {
            this.expContext = expContext;
            tape = new TokenTape(tokens, expContext.FileContext);
            Exp exp = ParseAssign();
            //exp.SetContextExp(expContext,false);
            return exp;
        }

        private Exp ParseAssign()
        {
            Exp leftExp = ParseNameValueExp();

            if (tape.HasCurrent && tape.Current.IsKind(TokenKindSymbol.Assign, TokenKindSymbol.AssignTo))
            {
                ExpAssign assignExp = new ExpAssign(expContext);
                assignExp.IsAssignTo = (tape.Current.IsKind(TokenKindSymbol.AssignTo));
                tape.MoveNext();
                Exp rightExp = ParseBinaryLogicExp();

                if (!assignExp.IsAssignTo)
                {
                    assignExp.ToExp = leftExp;
                    assignExp.ValueExp = rightExp;
                }
                else
                {
                    assignExp.ToExp = rightExp;
                    assignExp.ValueExp = leftExp ;
                }
                return assignExp;
            }
            else
            {
                return leftExp;
            }
        }

        private Exp ParseNameValueExp()
        {
            Exp leftExp = ParseBinaryLogicExp();
            
            if (tape.HasCurrent && tape.Current.IsKind(TokenKindSymbol.Colon))
            {
                tape.MoveNext();
                Exp rightExp = ParseBinaryLogicExp();
                if (leftExp is ExpChain)
                {
                    ExpChain chainExp = leftExp as ExpChain;
                    if(chainExp.SubCount==1)
                    {
                        object varobj = chainExp.RawElements[0];
                        if(varobj is LexTokenText)
                        {
                            LexTokenText textToken = (varobj as LexTokenText);
                            ExpNameValue expNameValue = new ExpNameValue(this.expContext, textToken, rightExp);
                            return expNameValue;
                        }
                        else
                        {
                            tape.error("参数名称错误");
                            return rightExp;
                        }
                    }
                    else
                    {
                        tape.error("参数名称的长度不是1");
                        return rightExp;
                    }
                }
                else if (leftExp is ExpVarBase)
                {
                    ExpVarBase leftVarExp = (leftExp as ExpVarBase);
                    LexToken varToken = leftVarExp.VarToken;
                    ExpNameValue expNameValue = new ExpNameValue(this.expContext, varToken, rightExp);
                    return expNameValue;
                }
                else
                {
                    tape.error("调用过程时指定的参数名称只能是标识符");
                    return rightExp;
                }
            }
            else
            {
                return leftExp;
            }
        }

        private Exp ParseBinaryLogicExp()
        {
            LexTokenSymbol opToken;
            Exp resultExpr = parseCompareExpr();
            while (tape.HasCurrent && tape.Current.IsKind(TokenKindSymbol.AND, TokenKindSymbol.OR))
            {
                opToken = (LexTokenSymbol)(tape.Current);
                tape.MoveNext();
                Exp rightExpr = parseCompareExpr();
                resultExpr = new ExpBinary(expContext) { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            //resultExpr.SetContextExp(this.expContext);
            return resultExpr;
        }

        protected Exp parseCompareExpr()
        {
            LexTokenSymbol opToken;
            Exp resultExpr = parseAddSub();
            while (tape.HasCurrent && IsCompareOp(tape.Current))
            {
                opToken = (LexTokenSymbol)(tape.Current);
                tape.MoveNext();
                Exp rightExpr = parseAddSub();
                resultExpr = new ExpBinary(expContext) { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        public bool IsCompareOp(LexToken tok)
        {
            return tok.IsKind(TokenKindSymbol.GT, TokenKindSymbol.LT, TokenKindSymbol.GE,
                TokenKindSymbol.LE, TokenKindSymbol.NE, TokenKindSymbol.EQ);
        }

        public Exp parseAddSub()
        {
            LexTokenSymbol opToken;
            Exp resultExpr = parseMulDiv();
            while (tape.HasCurrent && tape.Current.IsKind (  TokenKindSymbol.ADD, TokenKindSymbol.SUB))
            {
                opToken = (LexTokenSymbol)(tape.Current);
                tape.MoveNext();
                Exp rightExpr = parseAddSub();
                resultExpr = new ExpBinary(expContext) { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        Exp parseMulDiv()
        {
            //report("parseMulDiv");
            LexTokenSymbol opToken;
            Exp resultExpr = ParseChain();
            while (tape.HasCurrent && tape.Current.IsKind(TokenKindSymbol.MUL, TokenKindSymbol.DIV))
            {
                opToken = (LexTokenSymbol)(tape.Current);
                tape.MoveNext();
                Exp rightExpr = parseMulDiv();
                resultExpr = new ExpBinary(expContext) { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }

        private Exp ParseChain()
        {
            ExpChain expChain = new ExpChain(expContext);
            while (tape.HasCurrent)
            {
                if (tape.Current.IsKind(TokenKindSymbol.LBS))
                {
                    ExpBracket subExp = parseBracket();
                    expChain.Add(subExp);
                }
                else if  (tape.Current.IsKind(TokenKindSymbol.RBS))
                {
                    break;
                    //tape.error("多余的右括号");
                    //tape.MoveNext();
                }
                else if (tape.Current is LexTokenLiteral)
                {
                    ExpLiteral subExp = parseLiteral();
                    expChain.Add(subExp);
                }
                else if (tape.Current.IsKind(
                    TokenKindKeyword.Ident,
                    TokenKindKeyword.Ident,
                    TokenKindKeyword.DE,
                    TokenKindKeyword.DI,
                    TokenKindKeyword.Each,
                    TokenKindKeyword.NewDefault
                    )
                   )
                {
                    LexToken tok = tape.Current;
                    expChain.Add(tok);
                    tape.MoveNext();
                }
                else
                {
                    break;
                }
            }
            if(expChain.SubCount==0)
            {
                return null;
            }
            return expChain;
        }

        private ExpLiteral parseLiteral()
        {
            var literalToken = (LexTokenLiteral)(tape.Current);
            ExpLiteral literalex = new ExpLiteral(expContext, literalToken);
            //literalex.SetContextExp(this.expContext);
            tape.MoveNext();
            return literalex;
        }

        private ExpBracket parseBracket( )
        {
            //report("parseBracket");
            ExpBracket bracketExp = new ExpBracket(this.expContext);
            
            bracketExp.LeftBracketToken = tape.Current;
            tape.MoveNext();
            Exp innerExp = null;
            innerExp = ParseAddInnerExp(bracketExp);
            if (tape.HasCurrent && !isBracketEnd(tape.Current))
            {
                while (tape.HasCurrent && !isBracketEnd(tape.Current))
                {
                    //if(tape.Current.Line==17)
                    //{
                    //    Console.WriteLine("17 line");
                    //}
                   
                    //if (isBracketEnd(tape.Current) )
                    //{
                    //    break;
                    //}
                    //else 
                    if (tape.Current.IsKind(TokenKindSymbol.Comma))
                    {
                        tape.MoveNext();
                        innerExp = ParseAddInnerExp(bracketExp);
                    }
                    //if (tape.Current.IsKind(TokenKindSymbol.Comma))
                    //{
                    //    tape.error("多余的表达式元素");
                    //    tape.MoveNext();
                    //    while (!isBracketEnd(tape.Current) && !tape.Current.IsKind( TokenKindSymbol.Comma))
                    //    {
                    //        tape.MoveNext();
                    //    }
                    //}
                    //else if (tape.Current.IsKind(TokenKindSymbol.Comma))
                    //{
                    //    tape.MoveNext();
                    //}
                }
            }
            if (tape.HasCurrent)
            {
                if (tape.Current.IsKind(TokenKindSymbol.RBS))
                {
                    bracketExp.RightBracketToken = tape.Current;
                    tape.MoveNext();
                }
                else
                {
                    tape.error("右括号不匹配");
                }
            }
            else
            {
                tape.error("缺少右括号");
            }
            //bracketExp.SetContextExp(this.expContext);
            return bracketExp;
        }

        private Exp ParseAddInnerExp(ExpBracket bracketExp)
        {
            //if(tape.Current.Text=="新的")
            //{
            //    Console.WriteLine("新的");
            //}
            Exp expr = ParseAssign();
            if (expr != null)
            {
                bracketExp.AddInnerExp(expr);
            }
            else
            {
                tape.error("缺少表达式");
            }
            return expr;
        }

        private bool isBracketEnd(LexToken tok)
        {
            return tok.IsKind( TokenKindSymbol.EOF , TokenKindSymbol.Semi , TokenKindSymbol.RBS) ;
        }
    }
}
