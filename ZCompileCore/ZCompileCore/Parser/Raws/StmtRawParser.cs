using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCompileCore.AST;
using ZCompileCore.AST.Exps;
using ZCompileCore.Contexts;
using ZCompileCore.Lex;
using ZCompileCore.Parsers;
using ZCompileCore.CommonCollections;
using ZCompileCore.ASTRaws;

namespace ZCompileCore.Parsers.Raws
{
    public class StmtRawParser
    {
        private ArrayTape<LineTokenCollection> tape;
        private ContextProc context;
        private List<StmtRaw> stmtList = new List<StmtRaw>();
        private TokenTape currLineTokenTape;

        public StmtRawParser(IEnumerable<LineTokenCollection> lineTokens, ContextProc context)
        {
            stmtList.Clear();
            this.context = context;
            tape = new ArrayTape<LineTokenCollection>(lineTokens);
        }

        public object ParseCurrent()
        {
            if (!tape.HasCurrent) return null;
            //Debugr.WriteLine(tape.Current.ToString());
            //if (tape.Current.ToString().IndexOf("之和")!=-1)
            //{
            //    Debugr.WriteLine(tape.Current.ToString());
            //}
            BuildCurrentLineTape();
            LexToken firstToken = currLineTokenTape.First;
            if(firstToken.IsKind( TokenKindKeyword.IF))
            {
                StmtRaw stmt = ParseIf();
                return stmt;
            }
            else if (firstToken.IsKind(TokenKindKeyword.Dang))
            {
                StmtRaw stmt = ParseWhile();
                return stmt;
            }
            else if (firstToken.IsKind(TokenKindKeyword.Repeat))
            {
                StmtRaw stmt = ParseRepeat();
                return stmt;
            }
            else if (firstToken.IsKind(TokenKindKeyword.Loop))
            {
                StmtRaw stmt = ParseForeach();
                return stmt;
            }
            else if (firstToken.IsKind(TokenKindKeyword.Catch))
            {
                StmtRaw stmt = ParseCatch();
                return stmt;
            }
            else
            {
                List<StmtRaw> stmts = ParseStmtCall();
                return stmts;
            }
        }

        private StmtRaw ParseWhile()
        {
            StmtWhileRaw whileStmt = new StmtWhileRaw();
            whileStmt.DangToken = (LexTokenText)(currLineTokenTape.Current);
            currLineTokenTape.MoveNext();
            whileStmt.ConditionExp = ParseIfRawExpLine();
            TapeMoveNext();
            //BuildCurrentLine();
            whileStmt.WhileBody = ParseStmtBlockRaw(whileStmt.DangToken.Position.Col);
            //tape.Match(TokenKind.Repeat);
            return whileStmt;
        }

        private StmtRaw ParseCatch() //例:处理异常EX
        {
            StmtCatchRaw catchStmt = new StmtCatchRaw();
            catchStmt.CatchToken = (LexTokenText)currLineTokenTape.Current;
            currLineTokenTape.MoveNext();
            catchStmt.ExceptionTypeVarToken = (LexTokenText)currLineTokenTape.Current;
            currLineTokenTape.MoveNext();
            TapeMoveNext();
            catchStmt.CatchBody = ParseStmtBlockRaw(catchStmt.CatchToken.Position.Col);
            return catchStmt;
        }

        private StmtRaw ParseForeach()   //例:循环每一个(子弹管理器的子弹群,ZD)
        {
            StmtForeachRaw foreachStmt = new StmtForeachRaw();
            foreachStmt.LoopToken = (LexTokenText)currLineTokenTape.Current; 
            currLineTokenTape.MoveNext();
            foreachStmt.EachToken = (LexTokenText)currLineTokenTape.Current;
            currLineTokenTape.MoveNext();
            currLineTokenTape.Match(TokenKindSymbol.LBS);
            foreachStmt.ListExp = ParseRawExpLineComma();
            currLineTokenTape.Match(TokenKindSymbol.Comma);
            foreachStmt.ItemToken = (LexTokenText)currLineTokenTape.Current; 
            currLineTokenTape.MoveNext();
            currLineTokenTape.Match(TokenKindSymbol.RBS);
            TapeMoveNext();
            foreachStmt.Body = ParseStmtBlockRaw(foreachStmt.LoopToken.Position.Col);
            return foreachStmt;
        }

        private StmtRaw ParseRepeat()
        {
            StmtRepeatRaw repeatStmt = new StmtRepeatRaw();
            repeatStmt.RepeatToken = (LexTokenText)currLineTokenTape.Current;
            currLineTokenTape.MoveNext();
            repeatStmt.TimesExp = ParseRawExpLine();
            //currLineTokenTape.Match(TokenKindKeyword.Times);
            TapeMoveNext();
            repeatStmt.RepeatBody = ParseStmtBlockRaw(repeatStmt.RepeatToken.Position.Col);
            return repeatStmt;
        }

        private List<StmtRaw> ParseStmtCall()
        {
            List<StmtRaw> list = new List<StmtRaw>();
            while(currLineTokenTape.HasCurrent)
            {
                StmtRaw stmt = ParseStmtCallSingle();
                if(stmt!=null)
                {
                    list.Add(stmt);
                }
            }
            TapeMoveNextNotBuildLine();
            return list;
        }

        private StmtRaw ParseStmtCallSingle()
        {
            ExpRaw startExpr = ParseRawExpLine();
            StmtCallRaw stmtcall = new StmtCallRaw();
            stmtcall.CallExp = startExpr;
            while (currLineTokenTape.HasCurrent)
            {
                if( currLineTokenTape.Current.IsKind(TokenKindSymbol.Comma))
                {
                    currLineTokenTape.Match(TokenKindSymbol.Comma);
                    break;
                }
                else if (currLineTokenTape.Current.IsKind(TokenKindSymbol.Semi))
                {
                    currLineTokenTape.Match(TokenKindSymbol.Semi);
                    break;
                }
            }
            return stmtcall;
        }

        private StmtBlockRaw ParseStmtBlockRaw(int col)
        {
            StmtBlockRaw stmtBlock = new StmtBlockRaw();
            while (tape.HasCurrent && tape.Current.StartCol > col)
            {
                stmtBlock.LineTokens.Add(tape.Current);
                TapeMoveNext();
            }
            return stmtBlock;
        }


        #region parse raw exp

        private ExpRaw ParseRawExpLine()
        {
            ExpRaw rexp = new ExpRaw();
            //int line = tape.Current.Line;
            int bracketCount = -1;
            while (currLineTokenTape.HasCurrent)
            {
                if (currLineTokenTape.Current.IsKind(TokenKindSymbol.LBS))
                {
                    rexp.RawTokens.Add(currLineTokenTape.Current);
                    if (bracketCount == -1)
                    {
                        bracketCount = 0;
                    }
                    bracketCount++;
                    currLineTokenTape.MoveNext();
                }
                else if (currLineTokenTape.Current.IsKind(TokenKindSymbol.RBS))
                {
                    if (bracketCount <= 0)
                    {
                        currLineTokenTape.error("多余的右括号");
                    }
                    else
                    {
                        bracketCount--;
                        rexp.RawTokens.Add(currLineTokenTape.Current);
                    }
                    currLineTokenTape.MoveNext();
                }
                else if (currLineTokenTape.Current.IsKind(TokenKindSymbol.Comma))
                {
                    if (bracketCount == -1)
                    {
                        //rexp.RawTokens.Add(currLineTokenTape.Current);
                        //currLineTokenTape.MoveNext();
                        break;
                    }
                    else if (bracketCount == 0)
                    {
                        //currLineTokenTape.MoveNext();
                        break;
                    }
                    else if (bracketCount > 0)
                    {
                        rexp.RawTokens.Add(currLineTokenTape.Current);
                        currLineTokenTape.MoveNext();
                    }
                }
                else
                {
                    rexp.RawTokens.Add(currLineTokenTape.Current);
                    currLineTokenTape.MoveNext();
                }
                //if (tape.CurrentKind == TokenKind.Semi)
                //{
                //    break;
                //}
            }
            return rexp;
        }

        private ExpRaw ParseRawExpLineComma()
        {
            ExpRaw rexp = new ExpRaw();
            //int line = tape.Current.Line;
            int bracketCount = -1;
            while (tape.Current != null)//while (tape.Current.Kind != TokenKindSymbol.EOF && tape.Current.Kind != TokenKindSymbol.NewLine)
            {
                //TokenKindSymbol kind = tape.Current.Kind;
                if (currLineTokenTape.Current.IsKind( TokenKindSymbol.Comma))
                {
                    break;
                }
                else if (currLineTokenTape.Current.IsKind(TokenKindSymbol.LBS))
                {
                    rexp.RawTokens.Add(currLineTokenTape.Current);
                    if (bracketCount == -1)
                    {
                        bracketCount = 0;
                    }
                    bracketCount++;
                    currLineTokenTape.MoveNext();
                }
                else if (currLineTokenTape.Current.IsKind( TokenKindSymbol.RBS))
                {
                    if (bracketCount <= 0)
                    {
                        currLineTokenTape.error("多余的右括号");
                    }
                    else
                    {
                        bracketCount--;
                        rexp.RawTokens.Add(currLineTokenTape.Current);
                    }
                    currLineTokenTape.MoveNext();
                }

                else
                {
                    rexp.RawTokens.Add(currLineTokenTape.Current);
                    currLineTokenTape.MoveNext();
                }
            }
            return rexp;
        }
        #endregion

        #region 分析"如果"

        private StmtRaw ParseIf()
        {
            StmtIfRaw ifStmt = new StmtIfRaw();
            ifStmt.IfToken = (LexTokenText)currLineTokenTape.Current;

            StmtIfRaw.IfElseStmt ifPart = ParseTruePart();
            ifStmt.ElseIfParts.Add(ifPart);

            while (currLineTokenTape.Current.IsKind(TokenKindKeyword.ELSEIF))
            {
                StmtIfRaw.IfElseStmt elseifPart = ParseTruePart();
                ifStmt.ElseIfParts.Add(elseifPart);
            }
            if (currLineTokenTape.Current.IsKind(TokenKindKeyword.ELSE))
            {
                ifStmt.ElsePart = new StmtIfRaw.ElseStmt();
                ifStmt.ElsePart.KeyToken = (LexTokenText)currLineTokenTape.Current;
                var pos = tape.Current.StartCol;
                TapeMoveNext();
                ifStmt.ElsePart.Body = ParseStmtBlockRaw(pos);
            }
            return ifStmt;
        }

        private StmtIfRaw.IfElseStmt ParseTruePart()
        {
            StmtIfRaw.IfElseStmt eistmt = new StmtIfRaw.IfElseStmt();
            eistmt.KeyToken = (LexTokenText)currLineTokenTape.Current;
            currLineTokenTape.MoveNext();//跳过否则如果
            eistmt.ElseIfExp = ParseIfRawExpLine();
            TapeMoveNext();
            eistmt.Body = ParseStmtBlockRaw(eistmt.KeyToken.Position.Col);
            return eistmt;
        }

        private ExpRaw ParseIfRawExpLine()
        {
            ExpRaw rexp = new ExpRaw();
            int bracketCount = -1;
            while (currLineTokenTape.HasCurrent)
            {
                if (currLineTokenTape.Current.IsKind( TokenKindSymbol.LBS))
                {
                    rexp.RawTokens.Add(currLineTokenTape.Current);
                    if (bracketCount == -1)
                    {
                        bracketCount = 0;
                    }
                    bracketCount++;
                    currLineTokenTape.MoveNext();
                }
                else if (currLineTokenTape.Current.IsKind(TokenKindSymbol.RBS))
                {
                    if (bracketCount <= 0)
                    {
                        currLineTokenTape.error("多余的右括号");
                    }
                    else
                    {
                        bracketCount--;
                        rexp.RawTokens.Add(currLineTokenTape.Current);
                    }
                    currLineTokenTape.MoveNext();
                }
                else
                {
                    rexp.RawTokens.Add(currLineTokenTape.Current);
                    currLineTokenTape.MoveNext();
                }
            }
            return rexp;
        }
        #endregion

        public List<LexToken> SegToken(List<LexToken> RawTokens)
        {
            TokenSegmenter segmenter = new TokenSegmenter(this.context.ProcSegmenter);
            List<LexToken> tokens = new List<LexToken>();
            foreach (var tok in RawTokens)
            {
                if (tok.IsKind(TokenKindKeyword.Ident)) 
                {
                    //if (tok.Text.IndexOf("之") != -1)
                    //{
                    //    Debugr.WriteLine("切割:"+tok.Text);
                    //}
                    LexToken[] newTokens = segmenter.Split(tok);
                    tokens.AddRange(newTokens);
                }
                else
                {
                    tokens.Add(tok);
                }
            }
            return tokens;
        }

        private void TapeMoveNextNotBuildLine()
        {
            tape.MoveNext();
        }

        private void TapeMoveNext()
        {
            tape.MoveNext();
            BuildCurrentLineTape();
        }

        private bool BuildCurrentLineTape()
        {
            if (tape.HasCurrent)
            {
                var currLineTokens = SegToken(tape.Current.ToList());
                currLineTokenTape = new TokenTape(currLineTokens, this.context.ClassContext.FileContext);
                return true;
            }
            return false;
        }

        public bool HasCurrent
        {
            get
            {
                return this.tape.HasCurrent;
            }
        }
    }
}
