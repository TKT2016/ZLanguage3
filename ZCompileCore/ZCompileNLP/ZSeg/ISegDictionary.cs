using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNLP.Segment;

namespace ZNLP.ZSeg
{
    public interface ISegDictionary
    {
        Dictionary<int, List<int>> GetDag(string sentence);
        Dictionary<int, Pair<int>> Calc(string sentence, IDictionary<int, List<int>> dag);
        bool ContainsText(string word);
        bool ContainsWordTrie(string word);
    }
}
