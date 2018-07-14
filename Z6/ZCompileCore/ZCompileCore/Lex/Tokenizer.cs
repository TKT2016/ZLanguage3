using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ZCompileCore.Reports;
using ZCompileCore.Tools;
using ZCompileCore.Contexts;

namespace ZCompileCore.Lex
{
    public class Tokenizer
    {
        private int line;
        private SourceReader reader;
        private const char END = '\0';
        private const int EOF = -1;
        private ContextFile fileContext;
        private LineTextWidther Widther = new LineTextWidther();

        private List<LineTokenCollection> lineTokens = new List<LineTokenCollection>();
        private LineTokenCollection curToks;

        public Tokenizer()
        {

        }

        public List<LineTokenCollection> Scan(SourceReader sr, ContextFile fileContext, int startLine)
        {
            Reset(sr,fileContext);
            line = startLine;
            curToks = new LineTokenCollection();
            //report("开始");
            //InitLineFirst();

            while (ch != END)
            {
                //report("ch="+ch+" "+(int)ch);
                //if (line == 33  )// ch == '控' && line == 18)
                //{
                //    //report("col:" + col);
                //    Widther.IsDebug = true;
                //}
                //else if (  line == 34)// ch == '控' && line == 18)
                //{
                //    Widther.IsDebug = true;
                //}
                //else
                //{
                //    Widther.IsDebug = false;
                //}
                
                char nextChar = GetNext();
                if (ch==' ' || ch=='\t')
                {
                    //report("SkipSpace");
                    SkipWhiteSpace();
                }
                else if (ch == '/' && nextChar == '/')
                {
                    SkipSingleLineComment();
                }
                else if (ch == '/' && nextChar == '*')
                {
                    SkipMutilLineComment();
                }
                else if (ch == '/')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line,col,TokenKindSymbol.DIV);// { Col = col, Line = line, };
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == '"'||ch=='“'||ch=='”')
                {
                    string str = scanString();
                    LexTokenLiteral tok = new LexTokenLiteral(line, col - 1, TokenKindLiteral.LiteralString, str);// { Col = col - 1, Line = line, Text = str, Kind = TokenKindSymbol.LiteralString };
                    curToks.Add(tok);
                }
                else if (ch == '\r' && nextChar == '\n')
                {
                    Next(); Next();
                    AddLineToken();//lineTokens.Add(curToks);
                    curToks = new LineTokenCollection();
                    ScanNewLine();
                }
                else if (ch == '\n' || ch == '\r')
                {
                    Next();
                    AddLineToken(); //lineTokens.Add(curToks);
                    curToks = new LineTokenCollection();
                    ScanNewLine();
                }
                else if ( "0123456789".IndexOf(ch)!=-1)
                {
                    string str = scanNumber();
                    var temp = col;
                    if (StringHelper.IsInt(str))
                    {
                        LexTokenLiteral tok = new LexTokenLiteral(line, temp, TokenKindLiteral.LiteralInt, str);
                        curToks.Add(tok);
                    }
                    else if (StringHelper.IsFloat(str))
                    {
                        LexTokenLiteral tok = new LexTokenLiteral(line, temp, TokenKindLiteral.LiteralFloat, str);
                        curToks.Add(tok);
                    }
                    else
                    {
                        lexError(str+"不是正确的数字");
                    }
                }
                else if (ch == '+')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.ADD);
                    curToks.Add(tok);
                    Next(); 
                }
                else if (ch == '-')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.SUB);
                    curToks.Add(tok);
                    Next();
                }
                else if ((ch == '=' ) && (nextChar == '=' ))
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.EQ);
                    curToks.Add(tok);
                    Next(); Next();
                }
                else if ((ch == '=' ) && (nextChar == '>'))
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.AssignTo);
                    curToks.Add(tok);
                    Next(); Next();
                }
                else if ((ch == '=' ))
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.Assign);
                    curToks.Add(tok);
                    Next();
                }
                else if ((ch == '*'))
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.MUL);
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == ','||ch=='，')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.Comma);
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == ';' || ch == '；')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.Semi);
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == '(' || ch == '（')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.LBS);
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == ')' || ch == '）')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.RBS);
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == '>' && GetNext() == '=')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.GE);
                    curToks.Add(tok);
                    Next(); Next();
                }
                else if (ch == '>')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.GT);
                    curToks.Add(tok);
                    Next();
                }
                else if (ch == '<' && nextChar == '=')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.LE);
                    curToks.Add(tok);
                    Next(); Next();
                }
                else if (ch == '<')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.LT);
                    curToks.Add(tok);
                    Next();
                }
                else if ((nextChar == '!' || nextChar == '！')&& (nextChar == '=' || nextChar == '＝'))
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.NE);
                    curToks.Add(tok);
                    Next(); Next();
                }
                else if (ch == ':' || ch == '：')
                {
                    LexTokenSymbol tok = new LexTokenSymbol(line, col, TokenKindSymbol.Colon);// { Col = col, Line = line, Kind = TokenKindSymbol.Colon };
                    curToks.Add(tok);
                    Next();
                }
                else if ((ch >= 'A' && ch <= 'Z') /*|| (ch == '_') */|| (ch >= 'a' && ch <= 'z') || ChineseHelper.IsChineseLetter(ch))
                {
                    var tempCol = col;
                    var tempLine = line;
                    LexToken t1 = scanIdentToken();
                    if (t1.Text == "说明")
                    {
                        if (ch == ':' || ch == '：')
                        {
                            SkipSingleLineComment();
                            continue;;
                        }
                    }
                    addIdentOrKey(t1);
                }
                else if (char.IsControl(ch))
                {
                    while (char.IsControl(ch)&&ch!=END)
                    {
                        Next();
                        if ((int)ch == 13)
                        {
                            ScanNewLine();
                        }
                    }
                }
                else
                {
                    lexError("无法识别"+(int)ch+": '" + ch+"' ");
                    Next();
                }
            }
            if (curToks!=null && curToks.Count>0)
            {
                AddLineToken();
            }
            return lineTokens;
        }

        private void AddLineToken()
        {
            lineTokens.Add(curToks);
            curToks = new LineTokenCollection();
        }

        private void ScanNewLine()
        {
            //ResetCol();
            //InitLineFirst();
            line++;
        }

        private void addIdentOrKey(LexToken token)
        {
            //if (processKeyIdent(token, "如果",TokenKindSymbol.IF)) return;
            //if (processKeyIdent(token, "否则如果", TokenKindSymbol.ELSEIF)) return;
            //if (processKeyIdent(token, "否则", TokenKindSymbol.ELSE)) return;
            //if (processKeyIdent(token, "循环当", TokenKind.While)) return;
            //if (processKeyIdent(token, "重复", TokenKindSymbol.Repeat)) return;
            //if (processKeyIdent(token, "当", TokenKindSymbol.Dang)) return;
            //if (processKeyIdent(token, "次", TokenKind.Times)) return;
            //if (processKeyIdent(token, "处理", TokenKindSymbol.Catch)) return;
            //if (processKeyIdent(token, "循环每一个", TokenKindSymbol.Foreach)) return;
            curToks.Add(token);
        }
         
        private LexTokenText scanIdentToken()
        {
            var tempCol = col;
            string idtext = scanIdentText();
            LexTokenText token = new LexTokenText(line, tempCol, idtext);
            token.CheckKind();
            return token;
        }

        private string scanIdentText()
        {
            StringBuilder buf = new StringBuilder();
            while (is_identifier_part_character(ch))
            {
                buf.Append(ch);
                Next();
            }

            //if (ch == '的' || ch == '第')
            //{
            //    string str = ch.ToString();
            //    Next();
            //    return str;
            //}
            //else
            //{
            //    while (is_identifier_part_character(ch) && ch != '的' && ch != '第')
            //    {
            //        buf.Append(ch);
            //        Next();
            //    }
            //}
            return buf.ToString();
        }

        private void lexError(string msg)
        {
            this.fileContext.Errorf(line, col, msg);
        }

        private void SkipWhiteSpace()
        {
            while (ch == ' ' || ch == '\t')
            {
                Next();
            }
        }

        private string scanNumber()
        {
            StringBuilder buf = new StringBuilder();
            while ("0123456789.".IndexOf(ch) != -1)
            {
                buf.Append(ch);
                Next();
            }
            return buf.ToString();
        }

        private void SkipSingleLineComment()
        {
            while (ch != '\n')
            {
                Next();
            }
        }

        private void SkipMutilLineComment()
        {
            while (ch!=END)
            {
                char nextChar = GetNext();
                if (ch == '*' && (nextChar == '/' ))
                {
                    Next();
                    Next();
                    break;
                }
                else if (ch == '\r' && nextChar == '\n')
                {
                    //report("扫描换行符");
                    //SkipLine();
                    //Next(); Next();
                    //col = 1;
                    //line++;
                    //Token tok = ScanNewLine(1);
                    //tokenList.Add(tok);
                    //LexToken tok = ScanNewLine(2);
                    //curToks.Add(tok);

                    Next(); Next();
                    ScanNewLine();
                }
                else if (ch == '\n' || ch == '\r')
                {
                    //report("扫描换行符");
                    //SkipLine();
                    //LexToken tok = ScanNewLine(1);
                    //curToks.Add(tok);
                    Next();
                    ScanNewLine();
                }
                else
                {
                    Next();
                }
            }
        }      

        static bool is_identifier_start_character(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || Char.IsLetter(c);
        }

        static bool is_identifier_part_character(char c)
        {
            return (c >= 'a' && c <= 'z') 
                || (c >= 'A' && c <= 'Z') 
                //|| c == '_' 
                || (c >= '0' && c <= '9') 
                || ChineseHelper.IsChineseLetter(c)
                ;
        }

        char scanEscapeChar()
        {
            //report("scanChar " + ch);
            Next();
            char temp;

            //if (ch != '\\' && ch !=END )
            //{
            //    temp = ch;
            //}
            //else
            //{
            //    Next();
            switch (ch)
            {
                case 'a':
                    temp = '\a'; break;
                case 'b':
                    temp = '\b'; break;
                case 'n':
                    temp = '\n'; break;
                case 't':
                    temp = '\t'; break;
                case 'v':
                    temp = '\v'; break;
                case 'r':
                    temp = '\r'; break;
                case '\\':
                    temp = '\\'; break;
                case 'f':
                    temp = '\f'; break;
                case '0':
                    temp = '\0'; break;
                case '"':
                    temp = '"'; break;
                case '\'':
                    temp = '\''; break;
                default:
                    lexError("错误的转义符`\\" + ch + "'");
                    return ch;
            }
            Next();
            //}
            //Next();
            /*if (ch != '\'')
            {
                lexError("字符的长度大于1");
                while (ch != '\'' && ch != END && ch != '\n' && (int)ch != 13)
                {
                    Next();
                }
            }
            if (ch == '\'')
            {
                Next();
            }*/
            return temp;
        }   
         
        string scanString()
        {
            Next();
            StringBuilder buf = new StringBuilder();
            while (ch !=END )
            {
                /* 因转义符号复杂，所以取消转义符号 */
                if (ch == '\\')
                {
                    //var c = scanEscapeChar();
                    buf.Append(ch);
                    Next();
                }
                else if (ch == '"' || ch == '“' || ch == '”')
                {
                    Next();
                    return buf.ToString();
                }
                else if (ch == '\n')
                {
                    line++;
                    buf.Append("\n");
                }
                else
                {
                    buf.Append(ch);
                    Next();
                }
            }
            lexError("文本没有对应的结束双引号");
            return buf.ToString();
        }

        #region rest ch next getnext
        public void Reset(SourceReader sr, ContextFile fileContext)
        {
            reader = sr;
            this.fileContext = fileContext;

            line = 1;
            //ResetCol();
            lineTokens.Clear();
        }

        //private void ResetCol()
        //{
        //    //col = 0;
        //    //linCharCount = 0;
        //    //linCharWide = 0;
        //    //linCharWidePre = 0;
        //    //lineWidthTotal = 0;
        //    //lineCharWidths.Clear();
        //    Widther.Clear(line);
        //}

        public char ch
        {
            get
            {
                if (reader.Peek() == -1) return END;
                return reader.PeekChar();
            }
        }

        //public int chWidth
        //{
        //    get
        //    {
        //        int charWidth = 0;
        //        if (ch == '\t')
        //        {
        //            int lineWidthTotal = GetLinePreCharTotal();
        //            charWidth = (8 - lineWidthTotal % 8);
        //            Console.WriteLine(line+" : "+ charWidth + " " + lineWidthTotal+" ");
        //        }
        //        else
        //        {
        //            //charWidth =// Encoding.GetEncoding("utf-8").GetByteCount(ch.ToString());
        //            charWidth = Encoding.GetEncoding("GBK").GetByteCount(ch.ToString());
        //        }
        //        return charWidth;
        //    }
        //}

        //private List<int> lineCharWidths = new List<int>();

        //private int GetLinePreCharTotal()
        //{
        //    int sum = 0;
        //    for (int i = 0; i < lineCharWidths.Count - 1; i++)
        //    {
        //        sum += lineCharWidths[i];
        //    }
        //    return sum;
        //}

        private int col
        {
            get
            {
                return Widther.Col;
            }
        }

        public void Next()
        {
            char ch2= reader.ReadChar();
            //lineCharWidths.Add(chWidth);
            Widther.Append(ch2);
        }

        //public void InitLineFirst()
        //{
        //    //col = 1;
        //    //lineWidthTotal = chWidth;
        //    //linCharWidePre = 0;
        //    //lineCharWidths.Clear();
        //    //lineCharWidths.Add(chWidth);
        //    Widther.Clear(1);
        //}

        public char GetNext()
        {
            return reader.GetNextChar();
        }

        #endregion

        private void report(string msg)
        {
            Console.WriteLine("[" + line + "," + col + "]:" + (ch != '\n' ? ch.ToString() : "(换行)") + ":" + msg);
        }
    }
}

