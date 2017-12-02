using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using ZCompileCore.Reports;
using ZCompileCore.Tools;
using ZCompileCore.Contexts;
using ZCompileKit.Tools;

namespace ZCompileCore.Lex
{
    public class Tokenizer
    {
        int line;
        int col;
        SourceReader reader;
        private const char END = '\0';
        private const int EOF = -1;
        ContextFile fileContext;
        List<LexToken> tokenList = new List<LexToken>();

        public Tokenizer()
        {

        }

        public void Reset(SourceReader sr, ContextFile fileContext)
        {
            reader = sr;
            this.fileContext = fileContext;

            line = 1;
            col =1;
        }

        //private void report(string msg)
        //{
        //    Console.WriteLine("[" + line + "," + col + "]:" + (ch != '\n' ? ch.ToString() : "(换行)") + ":" + msg);
        //}

        char ch
        {
            get
            {
                if (reader.Peek() == -1) return END;
                return reader.PeekChar();
            }
        }

        public char Next()
        {
            col++;
            if (ch == '\t') col+=3;
            return reader.ReadChar();
        }

        public char GetNext()
        {
            return reader.GetNextChar();
        }


        public List<LexToken> Scan(SourceReader sr, ContextFile fileContext)
        {
            Reset(sr,fileContext);

            //report("开始");
            tokenList.Clear();
            while (ch != END)
            {
                //report("ch="+ch+" "+(int)ch);
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
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.DIV };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '"'||ch=='“'||ch=='”')
                {
                    string str = scanString();
                    LexToken tok = new LexToken() { Col = col - 1, Line = line, Text = str, Kind = TokenKind.LiteralString };
                    tokenList.Add(tok);
                }
                else if (ch == '\r' && nextChar == '\n')
                {
                    LexToken tok = ScanNewLine(2);// new Token() { Col = col, Line = line, Text = "\r\n", Kind = TokenKind.NewLine };
                    //report("扫描换行符");
                    //Next(); Next();
                    //col = 1;
                    //line++;
                    tokenList.Add(tok);
                }
                else if (ch == '\n' || ch == '\r')
                {
                    LexToken tok = ScanNewLine(1);
                    tokenList.Add(tok);
                    //SkipLine();
                    //Token tok = new Token() { Col = col, Line = line, Text = "\r\n", Kind = TokenKind.NewLine };
                    //Next(); 
                    //col = 1;
                    //line++;
                    //tokenList.Add(tok);
                }
                else if ( "0123456789".IndexOf(ch)!=-1)
                {
                    string str = scanNumber();
                    var temp = col;
                    if (StringHelper.IsInt(str))
                    {
                        LexToken tok = new LexToken() { Col = temp, Line = line, Text = str, Kind = TokenKind.LiteralInt };
                        tokenList.Add(tok);
                    }
                    else if (StringHelper.IsFloat(str))
                    {
                        LexToken tok = new LexToken() { Col = temp, Line = line, Text = str, Kind = TokenKind.LiteralFloat };
                        tokenList.Add(tok);
                    }
                    else
                    {
                        lexError(str+"不是正确的数字");
                    }
                }
                else if (ch == '+' || ch == '＋')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.ADD };
                    tokenList.Add(tok);
                    Next(); 
                }
                else if (ch == '-' || ch == '－')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.SUB };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((ch == '=' || ch == '＝') && (nextChar == '=' || nextChar == '＝'))
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.EQ };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if ((ch == '=' || ch == '＝') && (nextChar == '>'))
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.AssignTo };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if ((ch == '=' || ch == '＝'))
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.Assign };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((ch == '*'))
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.MUL };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == ','||ch=='，')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.Comma };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == ';' || ch == '；')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.Semi };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '(' || ch == '（')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.LBS };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == ')' || ch == '）')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.RBS };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '>' && GetNext() == '=')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.GE };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if (ch == '>')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.GT };
                    tokenList.Add(tok);
                    Next();
                }
                else if (ch == '<' && nextChar == '=')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.LE };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                else if (ch == '<')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.LT };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((nextChar == '!' || nextChar == '！')&& (nextChar == '=' || nextChar == '＝'))
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.NE };
                    tokenList.Add(tok);
                    Next(); Next();
                }
                /*else if (ch == ':' && nextChar == ':')
                {
                    Token tok = new Token() { Col = col, Line = line, Kind = TokenKind.Colond };
                    tokenList.Add(tok);
                    Next(); Next();
                }*/
                else if (ch == ':' || ch == '：')
                {
                    LexToken tok = new LexToken() { Col = col, Line = line, Kind = TokenKind.Colon };
                    tokenList.Add(tok);
                    Next();
                }
                else if ((ch >= 'A' && ch <= 'Z') /*|| (ch == '_') */|| (ch >= 'a' && ch <= 'z') || ChineseHelper.IsChineseLetter(ch))
                {
                    var tempCol = col;
                    var tempLine = line;
                    LexToken t1 = scanKeyIdent();
                    //if (t1.GetText().StartsWith("否则如果") || t1.GetText().StartsWith("否则") || t1.GetText().StartsWith("如果"))
                    //{
                    //    Console.WriteLine("否则如果");
                    //}
                    //tokenList.Add(t1);
                    if (t1.GetText() == "说明")
                    {
//                      char nchar = GetNext();
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
                            line++;
                            col = 1;
                        }
                    }
                }
                else
                {
                    lexError("无法识别"+(int)ch+": '" + ch+"' ");
                    Next();
                }
            }
            return tokenList;
        }

        private LexToken ScanNewLine(int i)
        {
            LexToken tok = new LexToken() { Col = col, Line = line, Text = "\r\n", Kind = TokenKind.NewLine };
            if (i >= 1) Next();
            if (i >= 2) Next();
            col = 1;
            line++;
            return tok;
        }

        void addIdentOrKey(LexToken token)
        {
            if(!isNewLine())
            {
                tokenList.Add(token);
                return;
            }
            if (processKeyIdent(token, "如果",TokenKind.IF)) return;
            if (processKeyIdent(token, "否则如果", TokenKind.ELSEIF)) return;
            if (processKeyIdent(token, "否则", TokenKind.ELSE)) return;
            //if (processKeyIdent(token, "循环当", TokenKind.While)) return;
            if (processKeyIdent(token, "重复", TokenKind.Repeat)) return;
            if (processKeyIdent(token, "当", TokenKind.Dang)) return;
            if (processKeyIdent(token, "次", TokenKind.Times)) return;
            if (processKeyIdent(token, "处理", TokenKind.Catch)) return;
            if (processKeyIdent(token, "循环每一个", TokenKind.Foreach)) return;
            tokenList.Add(token);
        }

        bool processKeyIdent(LexToken token,string keyContent,TokenKind keyKind)
        {
            string srcContent = token.GetText();
            if (!srcContent.StartsWith(keyContent)) return false;
            LexToken keyToken = new LexToken(keyContent, keyKind, token.Line, token.Col);
            tokenList.Add(keyToken);
            if (srcContent.Length > keyContent.Length)
            {
                int keyLength = keyContent.Length;
                LexToken identToken = new LexToken(srcContent.Substring(keyLength), TokenKind.Ident, token.Line, token.Col + keyLength);
                tokenList.Add(identToken);
            }
            return true;
        }

        LexToken getPreToken()
        {
            if (tokenList.Count == 0) return null;
            int lastIndex = tokenList.Count - 1;
            return tokenList[lastIndex];
        }

        bool isNewLine()
        {
            LexToken preToken = getPreToken();
            if (preToken == null)
            {
                return true;
            }
            else if (preToken.Line < this.line)
            {
                return true;
            }
            else if (preToken.Kind == TokenKind.Semi)
            {
                return true;
            }
            return false;
        }

        private LexToken scanKeyIdent()
        {
            var tempCol = col;
            string idtext = scanIdent();
            TokenKind kind = LexToken.GetKindByText(idtext);
            LexToken token = new LexToken() { Col = tempCol, Line = line, Kind = kind };
            if (kind == TokenKind.Ident)
            {
                token.Text = idtext;
            }
            return token;
        }

        private string scanIdent()
        {
            StringBuilder buf = new StringBuilder();
            if (ch == '的' || ch == '第')
            {
                string str = ch.ToString();
                Next();
                return str;
            }
            else
            {
                while (is_identifier_part_character(ch) && ch != '的' && ch != '第')
                {
                    buf.Append(ch);
                    Next();
                }
            }
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
                    LexToken tok = ScanNewLine(2);
                    tokenList.Add(tok);
                }
                else if (ch == '\n')
                {
                    //report("扫描换行符");
                    //SkipLine();
                    LexToken tok = ScanNewLine(1);
                    tokenList.Add(tok);
                }
                else if (ch == '\r')
                {
                    //report("扫描换行符");
                    //SkipLine();
                    LexToken tok = ScanNewLine(1);
                    tokenList.Add(tok);
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
    }
}

