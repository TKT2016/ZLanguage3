using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using ZCompileCore.Tools;

namespace ZCompileCore.Lex
{
    public abstract class SourceReader
    {
        public const char END = '\0';
        public abstract int Peek();
        public abstract int Read();
        public abstract void Close();
        public abstract char GetNextChar();

        public virtual char PeekChar()
        {
            return (char)(Peek());
        }

        public virtual char ReadChar()
        {
            return (char)(Read());
        }

        

        //public char GetNextChar()
        //{
        //    if (_sourceText != null && pointer < _sourceText.Length-1)
        //    {
        //        char ch = _sourceText[pointer+1];
        //        return ch;
        //    }
        //    return END;
        //}
    }
}
