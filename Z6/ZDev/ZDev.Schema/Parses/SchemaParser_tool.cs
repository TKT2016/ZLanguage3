using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZDev.Schema.Parses
{
    public partial class SchemaParser
    {
        bool matchSemiOrNewLine()
        {
            if (CurrentKind == TokenKind.EOF) return true;
            if (isNewLine()) return true;
            TokenKind semi = TokenKind.Semi;
            if (CurrentToken.Kind != semi)
            {
                //error(CurrentToken, CurrentToken.ToCode() + "不正确,应该是" + Token.GetTextByKind(semi));
                return false;
            }
            else
            {
                MoveNext();
                return true;
            }
        }

        protected bool isNewLine()
        {
            Token preToken = PreToken;
            Token curToken = CurrentToken;
            if (preToken == null)
            {
                return true;
            }
            else if (preToken.Line < curToken.Line)
            {
                return true;
            }
            else if (preToken.Kind == TokenKind.Semi)
            {
                return true;
            }
            return false;
        }

        /*
        Exp parseExp()
       {
           Token opToken;
           Exp resultExpr = parseCompareExpr();
           while (CurrentToken.Kind == TokenKind.AND || CurrentToken.Kind == TokenKind.OR)
           {
               opToken = CurrentToken;
               MoveNext();
               Exp rightExpr = parseCompareExpr();
               resultExpr = new BinExp() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
           }
           return resultExpr;
       }*/
        /*
        protected Exp parseCompareExpr()
        {
            Token opToken;
            Exp resultExpr = parseAddSub();
            while (CurrentToken.Kind == TokenKind.GT || CurrentToken.Kind == TokenKind.LT || CurrentToken.Kind == TokenKind.GE
                || CurrentToken.Kind == TokenKind.LE || CurrentToken.Kind == TokenKind.NE || CurrentToken.Kind == TokenKind.EQ)
            {
                opToken = CurrentToken;
                MoveNext();
                Exp rightExpr = parseAddSub();
                resultExpr = new BinExp() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }*/
        /*
        public Exp parseAddSub()
        {
            Token opToken;
            Exp resultExpr = parseMulDiv();
            while (CurrentToken.Kind == TokenKind.ADD || CurrentToken.Kind == TokenKind.SUB)
            {
                opToken = CurrentToken;
                MoveNext();
                Exp rightExpr = parseAddSub();
                resultExpr = new BinExp() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }*/
        /*
        Exp parseMulDiv()
        {
            Token opToken;
            Exp resultExpr = parseCall();
            while (CurrentToken.Kind == TokenKind.MUL || CurrentToken.Kind == TokenKind.DIV)
            {
                opToken = CurrentToken;
                MoveNext();
                Exp rightExpr = parseMulDiv();
                resultExpr = new BinExp() { LeftExp = resultExpr, OpToken = opToken, RightExp = rightExpr };
            }
            return resultExpr;
        }*/
        /*
        Exp parseCall()
        {
            Exp subjExpr = parseTerm(); //parseDe();
            if (PreToken != null && CurrentToken.Line == PreToken.Line)
            {
                if (CurrentKind == TokenKind.Ident || CurrentKind == TokenKind.LBS || CurrentToken.IsLiteral())
                {
                    FCallExp callexpr = new FCallExp();
                    callexpr.Elements.Add(subjExpr);
                    while (CurrentKind == TokenKind.Ident || CurrentKind == TokenKind.LBS || CurrentToken.IsLiteral())
                    {
                        subjExpr = parseTerm();//parseDe();
                        callexpr.Elements.Add(subjExpr);
                    }
                    return callexpr;
                }
            }
            return subjExpr;
        }*/
        /*
        BracketExp parseBracket()
         {
             //match(TokenKind.LBS);
             BracketExp bracket = new BracketExp();
             bracket.LeftBracketToken = CurrentToken; MoveNext();
             if (!isBracketEnd(CurrentKind))
             {
                 //while(CurrentKind!=TokenKind.EOF && CurrentKind!= TokenKind.Semi && CurrentKind!= TokenKind.RBS)
                 while (true)
                 {
                     Exp expr = parseExp();
                     if (expr != null)
                     {
                         bracket.InneExps.Add(expr);
                     }

                     if (isBracketEnd(CurrentKind))
                     {
                         break;
                     }
                     if (CurrentKind != TokenKind.Comma)
                     {
                         //error("多余的表达式元素");
                         MoveNext();
                         while (!(isBracketEnd(CurrentKind)) && CurrentKind != TokenKind.Comma)
                         {
                             MoveNext();
                         }
                     }
                     if (CurrentKind == TokenKind.Comma)
                     {
                         MoveNext();
                     }
                 }
             }
            if(CurrentKind== TokenKind.RBS)
            {
                bracket.RightBracketToken = CurrentToken;
                MoveNext();
            }
            else
            {
                error("括号不匹配");
            }
             return bracket;
         }*/

        static bool isBracketEnd(TokenKind kind)
        {
            return (kind == TokenKind.EOF || kind == TokenKind.Semi || kind == TokenKind.RBS);
        }
        /*
        Exp parseTerm()
        {
            if (CurrentToken.Kind == TokenKind.LBS)
            {
                return parseBracket();
            }
            else if (CurrentToken.Kind == TokenKind.RBS)
            {
                //error("多余的右括号");
                MoveNext();
                return null;
            }
            else if (CurrentToken.IsLiteral())
            {
                return parseLiteral();
            }
            else if (CurrentToken.Kind == TokenKind.Ident && NextToken.Kind == TokenKind.Colon)
            {
                return pareNV();
            }
            else if (CurrentToken.Kind == TokenKind.Ident)
            {
                return parseIdent();
            }
            else
            {
                //error(CurrentToken, "无法识别的表达式");
                //MoveNext();
                return null;
            }
        }
        */
        /*
        Exp parseIdent()
        {
            FTextExp idexp = new FTextExp();
            idexp.IdentToken = CurrentToken;
            MoveNext();
            return idexp;
        }
        */
        /*
      Exp parseLiteral()
      {
          LiteralExp literalex = new LiteralExp();
          literalex.LiteralToken = CurrentToken;
          MoveNext();
          return literalex;
      }*/
        /*
        Exp pareNV()
        {
            NameValueExp nv = new NameValueExp();
            nv.NameToken = CurrentToken;
            MoveNext();
            match(TokenKind.Colon);
            Exp exp = parseExp();
            nv.ValueExp = exp;
            return nv;
        }*/
        /*
        List<Stmt> ParseFuncBody()
        {
            int funcBodyDeep = 2;
            List<Stmt> stmtlist = new List<Stmt>();
            while (CurrentToken.Kind != TokenKind.EOF)
            {
                Stmt temp = parseStmt(funcBodyDeep);
                if (temp != null)
                {
                    stmtlist.Add(temp);
                }
            }
            return stmtlist;
        }*/
        /*
        Stmt parseStmt(int deep)
        {
            //report("parseStmt()"+ CurrentToken.ToString());
            Stmt stmt = null;
            if (CurrentToken.Kind == TokenKind.EOF)
            {
                return null;
            }
            else if (CurrentToken.Kind == TokenKind.IF)
            {
                stmt = parseIf(deep);
                return stmt;
            }
            else if (CurrentToken.Kind == TokenKind.While)
            {
                stmt = ParseWhile(deep);
                return stmt;
            }
            else if (CurrentToken.Kind == TokenKind.Foreach)
            {
                stmt = ParseForeach(deep);
                return stmt;
            }
            else if (CurrentToken.Kind == TokenKind.Catch)
            {
                stmt = ParseCatch(deep);
                return stmt;
            }
            else if (CurrentToken.Kind == TokenKind.Semi)
            {
                MoveNext();
                return null;
            }
            else
            {
                stmt = ParseAssignOrExprStmt();
            }
            if(stmt!=null)
            {
                stmt.Deep = deep;
            }
            return stmt;
        }
        */
        void skipBlock(CodePostion parentPostion, int deep)
        {
            //report("parseBlock()");
            //BlockStmt blockStmt = new BlockStmt();
            //List<Stmt> arr = new List<Stmt>();
            while (CurrentToken.Kind != TokenKind.EOF && CurrentToken.Col > parentPostion.Col)
            {
                MoveNext();
                //Stmt temp = parseStmt(deep+1);
                //if (temp != null)
                //{
                //    blockStmt.Add(temp);
                //}
            }
            //return blockStmt;
        }
        /*
        Stmt parseIf(int deep)
        {
            //report("parseIf()",3);
            //checkToken(TokenKind.IF);
            //if (CurrentToken.Line == 59) { 
            //    Console.WriteLine("Parser parseIf 59"); 
            //}
            IfStmt ifStmt = new IfStmt();
            IfStmt.IfTrueStmt ifPart = parseTruePart(deep);
            ifStmt.Parts.Add(ifPart);
            while (CurrentToken.Kind == TokenKind.ELSEIF)
            {
                IfStmt.IfTrueStmt elseifPart = parseTruePart(deep);
                ifStmt.Parts.Add(elseifPart);
            }
            if (CurrentToken.Kind== TokenKind.ELSE)
            {
                CodePostion pos = CurrentToken.Postion;
                MoveNext();
                ifStmt.ElsePart = skipBlock(pos, deep);
            }
            return ifStmt;
        }*/
        /*
        IfStmt.IfTrueStmt parseTruePart(int deep)
        {
            IfStmt.IfTrueStmt eistmt = new IfStmt.IfTrueStmt();
            eistmt.KeyToken = CurrentToken;
            MoveNext();//跳过否则如果
            eistmt.Condition = parseCondition();
            eistmt.Deep = deep;
            eistmt.Body = skipBlock(eistmt.Postion,deep+1);
            return eistmt;
        }
        */
        /*
        Exp parseCondition()
        {
            match(TokenKind.LBS);
            Exp exp = parseExp();
            match(TokenKind.RBS);
            return exp;
        }*/
        /*
        public Stmt ParseWhile(int deep)
        {
            //report("ParseWhile()");
            checkToken(TokenKind.While);
            WhileStmt whileStmt = new WhileStmt();
            whileStmt.WhileToken = CurrentToken;
            MoveNext();
            whileStmt.Condition = parseCondition();
            whileStmt.Body = skipBlock(whileStmt.Postion,deep);
            return whileStmt;
        }*/
        /*
        public Stmt ParseForeach(int deep)
        {
            //report("ParseForeach()");
            checkToken(TokenKind.Foreach);
            ForeachStmt foreachStmt = new ForeachStmt();
            foreachStmt.ForeachToken = CurrentToken;
            MoveNext();
            match(TokenKind.LBS);
            foreachStmt.ListExp = parseExp();
            match(TokenKind.Comma);
            foreachStmt.ElementToken = CurrentToken;
            MoveNext();
            if(CurrentKind==TokenKind.Comma)
            {
                MoveNext();
                foreachStmt.IndexToken = CurrentToken;
                MoveNext();
            }
            match(TokenKind.RBS);
            foreachStmt.Body = skipBlock(foreachStmt.Postion, deep);          
            return foreachStmt;
        }
        */
        /*
      public Stmt ParseCatch(int deep)
      {
          //report("ParseCatch()");
          checkToken(TokenKind.Catch);
          CatchStmt catchStmt = new CatchStmt();
          catchStmt.CatchToken = CurrentToken;
          MoveNext();
          match(TokenKind.LBS);
          catchStmt.ExceptionTypeToken = CurrentToken;
          MoveNext();
          match(TokenKind.Colon);
          catchStmt.ExceptionNameToken = CurrentToken;
          MoveNext();
          match(TokenKind.RBS);
          catchStmt.CatchBody = skipBlock(catchStmt.Postion, deep);
          return catchStmt;
      }
      */
        /*
      Stmt ParseAssignOrExprStmt( )
      {
          //if (CurrentText == "发射子弹" || NextToken.GetText() == "发射子弹")
          //{
          //    Console.WriteLine("ParseAssignOrExprStmt 发射子弹");
          //}
          Exp startExpr = parseExp();
          if (CurrentToken.Kind == TokenKind.Assign )
          {
              return parseAssign(startExpr);
          }
          else if (CurrentToken.Kind == TokenKind.AssignTo)
          {
              return parseAssignTo(startExpr);
          }
          else if (CurrentKind == TokenKind.Semi || CurrentKind== TokenKind.EOF)
          {
              return parseCall(startExpr);
          }       
          else
          {
              error("ParseAssignOrExprStmt 无法识别" + CurrentToken.ToCode());
              return null;
          }
          //return null;
      }*/
        /*
        Stmt parseCall(Exp start)
        {
            CallStmt stmtcall = new CallStmt();
            stmtcall.CallExpr = start;
            match(TokenKind.Semi);
            return stmtcall;
        }*/
        /*
        Stmt parseAssign(Exp start)
        {
            AssignStmt stmtassign = new AssignStmt();
            stmtassign.ToExp = start;
            //if(start is FTextExp)
            //{
            //    FTextExp idexp = start as FTextExp;
            //    //spliter.Add(idexp.IdToken);
            //}
            MoveNext();
            stmtassign.ValueExp = parseExp();
            match(TokenKind.Semi);
            return stmtassign;
        }*/
        /*
        Stmt parseAssignTo(Exp start)
        {
            AssignStmt stmtassign = new AssignStmt();
            stmtassign.IsAssignTo = true;
            stmtassign.ValueExp = start;
            MoveNext();
            stmtassign.ToExp = parseExp();
            match(TokenKind.Semi);
            return stmtassign;
        }
        */

        protected Token PreToken
        {
            get
            {
                if (index == 0) return null;
                return this.srcTokens[index - 1];
            }
        }

        protected Token CurrentToken
        {
            get { return this.srcTokens[index]; }
        }

        protected TokenKind CurrentKind
        {
            get { return CurrentToken.Kind; }
        }

        protected string CurrentText
        {
            get { return CurrentToken.GetText(); }
        }

        protected Token NextToken
        {
            get { return this.srcTokens[index + 1]; }
        }

        protected void MoveNext()
        {
            index++;
        }

        void skipToSemi()
        {
            while (CurrentToken.Kind != TokenKind.Semi && CurrentToken.Kind != TokenKind.EOF)
            {
                MoveNext();
            }
            if (CurrentToken.Kind == TokenKind.Semi)
            {
                MoveNext();
            }
        }

        protected bool match(TokenKind tokKind)
        {
            if (CurrentToken.Kind != tokKind)
            {
                //error(CurrentToken, CurrentToken.ToCode() + "不正确,应该是" + Token.GetTextByKind(tokKind) );
                return false;
            }
            else
            {
                MoveNext();
                return true;
            }
        }

        protected bool checkToken(TokenKind tokKind)
        {
            if (CurrentToken.Kind != tokKind)
            {
                //error(CurrentToken, CurrentToken.ToCode() + "不正确,应该是" + Token.GetTextByKind(tokKind));
                return false;
            }
            else
            {
                return true;
            }
        }

        List<Token> inputLine()
        {
            List<Token> tokens = new List<Token>();
            int curline = CurrentToken.Postion.Line;
            while (CurrentToken.Kind != TokenKind.EOF && curline == CurrentToken.Postion.Line)
            {
                tokens.Add(CurrentToken);
                MoveNext();
            }
            return tokens;
        }
        /*
        protected void error(Token tok, string str)
        {
            Messager.Error(tok.Line, tok.Col, " " + CurrentToken.ToString() + " - " + str);
        }

        protected void error(string str)
        {
            error(CurrentToken, str);
        }*/
        /*
        protected void report(string str, int color = 1)
        {
            ConsoleColor c = Console.ForegroundColor;
            if (color == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            else if (color == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (color == 3)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if (color == 4)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            Console.WriteLine("DEBUG:" + index + " [" + CurrentToken.Line + "," + CurrentToken.Col + "]:" + CurrentToken.ToCode() + " --- " + str);
            Console.ForegroundColor = c;
        }*/

    }
}
