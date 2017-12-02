using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ZCompileKit.Infoes;

namespace ZCompileCore.Reports
{
    public abstract class CompileMessageKey
    {
        //public abstract string ToString();
    }

    public class CompileMessageSrcKey : CompileMessageKey
    {
        public string SrcFileName { get; set; }

        public CompileMessageSrcKey(string fileName)
        {
            SrcFileName = fileName;
        }

        public override string ToString()
        {
            return SrcFileName;
        }
    }
}
