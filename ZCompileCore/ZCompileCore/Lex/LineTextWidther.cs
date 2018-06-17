using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.Lex
{
    internal class LineTextWidther
    {
        public bool IsDebug { get; set; }

        private List<char> Chars = new List<char>();
        private List<int> ColWidth = new List<int>();
        private int TotalWidth = 0;
        private int CurrentWidth = 0;
        private int Line = 1;
        private char preChar = '\0';

        public void Append(char ch)
        {
            //int CurrentWidth = 0;
            if(ch=='\n')
            {
                Line++;
                Clear();
                preChar = ch;
                return;
            }
            else if ( ch == '\r')
            {
                if (preChar != '\n')
                {
                    Line++;
                    Clear();
                }
                preChar = ch;
                return;
            }

            if (ch == '\t')
            {
                //int lineWidthTotal = GetLinePreCharTotal();
                CurrentWidth = (8 - TotalWidth % 8);
            }
            else
            {
                //charWidth =// Encoding.GetEncoding("utf-8").GetByteCount(ch.ToString());
                CurrentWidth = Encoding.GetEncoding("GBK").GetByteCount(ch.ToString());
            }
            
            Chars.Add(ch);
            ColWidth.Add(CurrentWidth);
            TotalWidth += CurrentWidth;
            preChar = ch;
            //if (IsDebug)
            //{
            //    Console.WriteLine("LineTextWidther[" + Line + "]: [" + ch + "] " + CurrentWidth + " " + TotalWidth + " " + Col);
            //}
        }

        public int Col
        {
            get
            {
                return TotalWidth - CurrentWidth + 1;
            }
        }

        private void Clear()
        {
            //Line = line;
            //preChar = '\0';
            Chars.Clear();
            ColWidth.Clear();
            CurrentWidth = 0;
            TotalWidth = 0;
        }
    }
}
