using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileCore.Lex
{
    public class CodePosition
    {
        public int Line { get; private set; }
        public int Col { get; private set; }
        public static readonly CodePosition ZeroCodePostion = new CodePosition(0, 0);

        public CodePosition()
        {

        }

        public CodePosition(int line,int col)
        {
            Line = line;
            Col = col;
        }

        public override string ToString()
        {
            return string.Format("({0},{1}",Line,Col);
        }
    }
}
