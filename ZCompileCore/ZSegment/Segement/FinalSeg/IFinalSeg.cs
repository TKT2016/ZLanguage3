using System;
using System.Collections.Generic;

namespace ZNLP.Segment.FinalSeg
{
    public interface IFinalSeg
    {
        IEnumerable<string> Cut(string sentence);
    }
}