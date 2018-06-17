using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using ZCompileKit.Infoes;

namespace ZCompileCore.Reports
{
    public class CompileMessage
    {
        public CompileMessageKey Key { get; set; }

        //public string SrcFileName { get; set; }
        public int Line { get; set; }

        public int Col { get; set; }

        public string Content { get; set; }

        public CompileMessage(CompileMessageSrcKey key, int line, int col, string text)
        {
            Key = key;
            Line = line;
            Col = col;
            Content = text;
        }

        //public CompileMessage(ZCompileFileInfo srcfile, int line, int col, string text)
        //{
        //    SourceFileInfo = srcfile;
        //    Line = line;
        //    Col = col;
        //    Content = text;
        //}
    }
}
