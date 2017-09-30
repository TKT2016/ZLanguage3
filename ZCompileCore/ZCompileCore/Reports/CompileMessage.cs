using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZCompileKit.Infoes;

namespace ZCompileCore.Reports
{
    public class CompileMessage
    {
        public ZCompileFileInfo SourceFileInfo { get; set; }

        public int Line { get; set; }

        public int Col { get; set; }

        public string Text { get; set; }

        //public bool IsWarning { get; set; }

        //public CompileMessageEnum MessageEnum { get; set; }

        public CompileMessage(ZCompileFileInfo srcfile, int line, int col, string text)
        {
            SourceFileInfo = srcfile;
            Line = line;
            Col = col;
            Text = text;
        }
    }
}
