using System;
using System.Collections.Generic;

namespace ZCompileNLP.Segment.FinalSeg
{
    public interface IFinalSeg
    {
        IEnumerable<string> Cut(string sentence);
    }
}