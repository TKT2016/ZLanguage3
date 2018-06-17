using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDev.Schema.Parses
{
    public class CodePostion
    {
        public int Line { get; private set; }
        public int Col { get; private set; }
        public int Index { get; private set; }

        public CodePostion()
        {

        }

        public CodePostion(int line,int col,int index)
        {
            Line = line;
            Col = col;
            Index = index;
        }

        public override string ToString()
        {
            return string.Format("({0},{1} {2})",Line,Col,Index);
        }
    }
}
