using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDev.Schema.Models;
using ZDev.Schema.Tools;

namespace ZDev.Schema.Parses
{
    public class SchemaScanner
    {
        int i = 0;
        int line=1;
        int col=1;
        string code;
        List<Token> tokenList = new List<Token>();

        void show(object obj)
        {
            Console.WriteLine(obj);
        }
        public List<Token> Scan(string code)
        {
            i = 0;
            line = 1;
            col = 1;
            this.code = code;
            tokenList = new List<Token>();

            while (ch != END)
            {
                char nextChar = GetNext();
                //show(ch + " " + nextChar);
                if (ch == ' ' || ch == '\t')
                {
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
                    Token tok = new Token("/", TokenKind.DIV,line,col,i);// { Col = col, Line = line, Index = i, Kind = TokenKind.DIV };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '"' || ch == '“' || ch == '”')
                {
                    var tempCol = col;
                    string str = scanString();
                    Token tok = new Token(str, TokenKind.LiteralString, line, tempCol, i);// { Col = col - 1, Line = line, Index = i, Text = str, Kind = TokenKind.LiteralString };
                    tokenList.Add(tok);
                }
                else if (ch == '\r' && nextChar == '\n')
                {
                    Next(); Next();
                    col = 1;
                    line++;
                }
                else if (ch == '\n')
                {
                    SkipLine();
                }
                else if (ch == '\r')
                {
                    SkipLine();
                }
                else if ("0123456789".IndexOf(ch) != -1)
                {
                    string str = scanNumber();
                    var tempCol = col;
                    if (TextUtil.IsInt(str))
                    {
                        Token tok = new Token(str, TokenKind.LiteralInt, line, tempCol, i);// { Col = temp, Line = line, Index = i, Text = str, Kind = TokenKind.LiteralInt };
                        tokenList.Add(tok);
                    }
                    else if (TextUtil.IsFloat(str))
                    {
                        Token tok = new Token(str, TokenKind.LiteralFloat, line, tempCol, i);// { Col = temp, Line = line, Index = i, Text = str, Kind = TokenKind.LiteralFloat };
                        tokenList.Add(tok);
                    }
                    else
                    {
                        //lexError(str + "不是正确的数字");
                    }
                }
                else if (ch == '+' || ch == '＋')
                {
                    Token tok = new Token("+", TokenKind.ADD, line, col, i);// { Col = col, Line = line, Index = i, Kind = TokenKind.ADD };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '-' || ch == '－')
                {
                    Token tok = new Token("-", TokenKind.SUB, line, col, i);// { Col = col, Line = line, Index = i, Kind = TokenKind.SUB };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((ch == '=' || ch == '＝') && (nextChar == '=' || nextChar == '＝'))
                {
                    Token tok = new Token("==", TokenKind.EQ, line, col, i);// { Col = col, Line = line, Index = i, Kind = TokenKind.EQ };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if ((ch == '=' || ch == '＝') && (nextChar == '>'))
                {
                    Token tok = new Token("==", TokenKind.EQ, line, col, i);// { Col = col, Line = line, Index = i, Kind = TokenKind.AssignTo };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if ((ch == '=' || ch == '＝'))
                {
                    Token tok = new Token("=", TokenKind.Assign, line, col, i);// { Col = col, Line = line, Index = i, Kind = TokenKind.Assign };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((ch == '*'))
                {
                    Token tok = new Token("*", TokenKind.MUL, line, col, i);// { Col = col, Line = line, Kind = TokenKind.MUL };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == ',' || ch == '，')
                {
                    Token tok = new Token(",", TokenKind.Comma, line, col, i);// { Col = col, Line = line, Kind = TokenKind.Comma };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == ';' || ch == '；')
                {
                    Token tok = new Token(";", TokenKind.Semi, line, col, i);// { Col = col, Line = line, Kind = TokenKind.Semi };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '(' || ch == '（')
                {
                    Token tok = new Token("(", TokenKind.LBS, line, col, i);// { Col = col, Line = line, Kind = TokenKind.LBS };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == ')' || ch == '）')
                {
                    Token tok = new Token(")", TokenKind.RBS, line, col, i);// { Col = col, Line = line, Kind = TokenKind.RBS };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '>' && GetNext() == '=')
                {
                    Token tok = new Token(">=", TokenKind.GE, line, col, i);// { Col = col, Line = line, Kind = TokenKind.GE };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if (ch == '>')
                {
                    Token tok = new Token(">", TokenKind.GT, line, col, i);// { Col = col, Line = line, Kind = TokenKind.GT };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '<' && nextChar == '=')
                {
                    Token tok = new Token("<=", TokenKind.LE, line, col, i);// { Col = col, Line = line, Kind = TokenKind.LE };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if (ch == '<')
                {
                    Token tok = new Token("<", TokenKind.LT, line, col, i);// { Col = col, Line = line, Kind = TokenKind.LT };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((nextChar == '!' || nextChar == '！') && (nextChar == '=' || nextChar == '＝'))
                {
                    Token tok = new Token("!=", TokenKind.NE, line, col, i);// { Col = col, Line = line, Kind = TokenKind.NE };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if (ch == ':' || ch == '：')
                {
                    Token tok = new Token(":", TokenKind.Colon, line, col, i);// { Col = col, Line = line, Kind = TokenKind.Colon };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((ch >= 'A' && ch <= 'Z') /*|| (ch == '_') */|| (ch >= 'a' && ch <= 'z') || ChineseHelper.IsChineseLetter(ch))
                {
                    var tempCol = col;
                    var tempLine = line;
                    Token t1 = scanKeyIdent();
                    tokenList.Add(t1);
                }
                else if (char.IsControl(ch))
                {
                    while (char.IsControl(ch) && ch != END)
                    {
                        Next();
                        if ((int)ch == 13)
                        {
                            line++;
                            col = 1;
                        }
                    }
                }
                else
                {
                    //lexError("无法识别" + (int)ch + ": '" + ch + "' ");
                    Next();
                }
            }
            return tokenList;
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

        Token scanKeyIdent()
        {
            var tempCol = col;
            string idtext = scanIdent();
            TokenKind kind = Token.GetKindByText(idtext);
            Token token = new Token("", kind, line, tempCol, i);// { Col = tempCol, Line = line, Kind = kind };
            if (kind == TokenKind.Ident)
            {
                token.Text = idtext;
            }
            return token;
        }


        private string scanIdent()
        {
            StringBuilder buf = new StringBuilder();
            while (is_identifier_part_character(ch) /*&& ch != '的'*/)
            {
                buf.Append(ch);
                Next();
            }
            return buf.ToString();
        }

        static bool is_identifier_part_character(char c)
        {
            return (c >= 'a' && c <= 'z')
                || (c >= 'A' && c <= 'Z')
                || (c >= '0' && c <= '9')
                || ChineseHelper.IsChineseLetter(c)
                ;
        }

        string scanString()
        {
            Next();
            StringBuilder buf = new StringBuilder();
            while (ch != END)
            {
                if (ch == '\\')
                {
                    var c = scanEscapeChar();
                    buf.Append(c);
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
            //lexError("文本没有对应的结束双引号");
            return buf.ToString();
        }


        char scanEscapeChar()
        {
            Next();
            char temp;
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
                    //lexError("错误的转义符`\\" + ch + "'");
                    return ch;
            }
            Next();
            return temp;
        }   
         

        #region 跳过注释

        private void SkipWhiteSpace()
        {
            while (ch == ' ' || ch == '\t')
            {
                Next();
            }
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
            while (ch != END)
            {
                char nextChar = GetNext();
                if (ch == '*' && (nextChar == '/'))
                {
                    Next();
                    Next();
                    break;
                }
                else if (ch == '\r' && nextChar == '\n')
                {
                    Next(); Next();
                    col = 1;
                    line++;
                }
                else if (ch == '\n')
                {
                    //report("扫描换行符");
                    SkipLine();
                }
                else if (ch == '\r')
                {
                    //report("扫描换行符");
                    SkipLine();
                }
                else
                {
                    Next();
                }
            }
        }

        private void SkipLine()
        {
            Next();
            col = 1;
            line++;
        }
        #endregion

        #region 辅助方法

        private const char END = '\0';
        private const int EOF = -1; 

        char ch
        {
            get
            {
                if (i < 0 || i > code.Length - 1) return END;
                return code[i];
            }
        }

        public char GetNext()
        {
            int k = i + 1;
            if (k < 0 || k > code.Length - 1) return END;
            return code[k];
        }

        public char Next()
        {
            col++;
            if (ch == '\t') col += 3;
            i++;
            return ch;
        }
        #endregion
    }
}
