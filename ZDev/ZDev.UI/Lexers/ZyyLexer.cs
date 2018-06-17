using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ZDev.Lexers
{
    public class ZyyLexer
    {
        public const int StyleDefault = 0;
        public const int StyleKeyword = 1;
        public const int StyleIdentifier = 2;
        public const int StyleNumber = 3;
        public const int StyleString = 4;
        public const int StyleCommentLine = 5;
        public const int StyleCommentMutil = 6;

        private const int STATE_UNKNOWN = 0;
        private const int STATE_IDENTIFIER = 1;
        private const int STATE_NUMBER = 2;
        private const int STATE_STRING = 3;
        private const int STATE_CommentLine = 4;
        private const int STATE_CommentMutil = 5;

        private HashSet<string> keywords;

        int state = STATE_UNKNOWN;

        public const string KeywordsText = "导入 类型 属性 整数 浮点数 判断符 过程 如果 否则 否则如果 循环当 每一个 文本 使用 处理异常 名称";
        public const string CodeKeywordsText = "属于 属性 过程 如果 否则 否则如果 循环当 循环每一个 使用 处理异常";
        public const string DataKeywordsText = "整数 浮点数 判断符 文本";

        public ZyyLexer()
        {
            var list = Regex.Split(KeywordsText ?? string.Empty, @"\s+").Where(l => !string.IsNullOrEmpty(l));
            this.keywords = new HashSet<string>(list);
        }

        public void Style(Scintilla scintilla, int startPos, int endPos)
        {
            // Back up to the line start
            var line = scintilla.LineFromPosition(startPos);
            startPos = scintilla.Lines[line].Position;

            var length = 0;
            
            // Start styling
            scintilla.StartStyling(startPos);
            while (startPos < endPos)
            {
                var c = (char)scintilla.GetCharAt(startPos);
                var nextchar ='\0';
                if(startPos<endPos)
                {
                    nextchar = (char)scintilla.GetCharAt(startPos+1);
                }
            REPROCESS:
                switch (state)
                {
                    case STATE_UNKNOWN:
                        if (c == '"')
                        {
                            // Start of "string"
                            scintilla.SetStyling(1, StyleString);
                            state = STATE_STRING;
                        }
                        else if (Char.IsDigit(c))
                        {
                            state = STATE_NUMBER;
                            goto REPROCESS;
                        }
                        else if (Char.IsLetter(c))
                        {
                            state = STATE_IDENTIFIER;
                            goto REPROCESS;
                        }
                        else if (c=='/'&&nextchar=='/')
                        {
                            state = STATE_CommentLine;
                            goto REPROCESS;
                        }
                        else if (c == '/' && nextchar == '*')
                        {
                            state = STATE_CommentMutil;
                            goto REPROCESS;
                        }
                        else
                        {
                            // Everything else
                            scintilla.SetStyling(1, StyleDefault);
                        }
                        break;
                    case STATE_CommentLine:
                        if(c=='\r' && nextchar=='\n')
                        {
                            scintilla.SetStyling(2, StyleCommentLine);
                            startPos++;
                            state = STATE_UNKNOWN;
                        }
                        else if (c == '\r' || c == '\n')
                        {
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            scintilla.SetStyling(1, StyleCommentLine);
                        }
                        break;
                    case STATE_CommentMutil:
                        if (c == '*' && nextchar == '/')
                        {
                            scintilla.SetStyling(2, StyleCommentLine);
                            startPos++;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            scintilla.SetStyling(1, StyleCommentLine);
                        }
                        break;
                    case STATE_STRING:
                        if (c == '"')
                        {
                            length++;
                            scintilla.SetStyling(length, StyleString);
                            length = 0;
                            state = STATE_UNKNOWN;
                        }
                        else
                        {
                            length++;
                        }
                        break;

                    case STATE_NUMBER:
                        if (Char.IsDigit(c) || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == 'x')
                        {
                            length++;
                        }
                        else
                        {
                            scintilla.SetStyling(length, StyleNumber);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;

                    case STATE_IDENTIFIER:
                        if (Char.IsLetterOrDigit(c))
                        {
                            length++;
                        }
                        else
                        {
                            var style = StyleIdentifier;
                            var identifier = scintilla.GetTextRange(startPos - length, length);
                            if (keywords.Contains(identifier))
                                style = StyleKeyword;

                            scintilla.SetStyling(length, style);
                            length = 0;
                            state = STATE_UNKNOWN;
                            goto REPROCESS;
                        }
                        break;
                }

                startPos++;
            }
        }

    }
}