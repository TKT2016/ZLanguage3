using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNLP.Segment.Common;
using ZNLP.Segment;

namespace ZNLP.ZSeg
{
    public class SegIntDictionary
    {
        public Dictionary<string, int> Trie { get; private set; }
        public double Total { get; set; }
        public List<string> UserWords { get; set; }

        public SegIntDictionary()
        {
            Trie = new Dictionary<string, int>();
        }

        public SegIntDictionary(IDictionary<string, int> trie, double total, List<string> userWords)
        {
            Trie = Trie;
            Total = total;
            UserWords = userWords;
        }

        public SegIntDictionary Clone()
        {
            SegIntDictionary clon = new SegIntDictionary();
            foreach (var key in this.Trie.Keys)
            {
                clon.Trie.Add(key, this.Trie[key]);
            }
            clon.Total = this.Total;
            if (this.UserWords!=null)
            {
                clon.UserWords = new List<string>();
                clon.UserWords.AddRange(this.UserWords);
            }
            return clon;
        }

        public void AddWord(string word, int freq = 0, string tag = null)
        {
            if(UserWords==null)
            {
                UserWords = new List<string>();
            }
            if (ContainsWordTrie(word))
            {
                Total -= Trie[word];
            }

            Trie[word] = freq;
            Total += freq;
            for (var i = 0; i < word.Length; i++)
            {
                var wfrag = word.Substring(0, i + 1);
                if (!Trie.ContainsKey(wfrag))
                {
                    Trie[wfrag] = 0;
                }
            }
            UserWords.Add(word);
        }

        public bool ContainsWordTrie(string word)
        {
            return Trie.ContainsKey(word) && Trie[word] > 0;
        }

        public int GetFreqOrDefault(string key)
        {
            if (ContainsWordTrie(key))
                return Trie[key];
            else
                return 1;
        }

        public bool ContainsText(string text)
        {
            if (this.UserWords == null) return false;
            return this.UserWords.IndexOf(text) != -1;
        }

        public Dictionary<int, List<int>> GetDag(string sentence)
        {
            var dag = new Dictionary<int, List<int>>();
            var trie = this.Trie;

            var N = sentence.Length;
            for (var k = 0; k < sentence.Length; k++)
            {
                var templist = new List<int>();
                var i = k;
                var frag = sentence.Substring(k, 1);
                while (i < N && trie.ContainsKey(frag))
                {
                    if (trie[frag] > 0)
                    {
                        templist.Add(i);
                    }

                    i++;
                    if (i < N)
                    {
                        frag = sentence.Sub(k, i + 1);
                    }
                }
                if (templist.Count == 0)
                {
                    templist.Add(k);
                }
                dag[k] = templist;
            }

            return dag;
        }

        public Dictionary<int, Pair<int>> Calc(string sentence, IDictionary<int, List<int>> dag)
        {
            var n = sentence.Length;
            var route = new Dictionary<int, Pair<int>>();
            route[n] = new Pair<int>(0, 0.0);

            var logtotal = Math.Log(this.Total);
            for (var i = n - 1; i > -1; i--)
            {
                var candidate = new Pair<int>(-1, double.MinValue);
                foreach (int x in dag[i])
                {
                    var freq = Math.Log(this.GetFreqOrDefault(sentence.Sub(i, x + 1))) - logtotal + route[x + 1].Freq;
                    if (candidate.Freq < freq)
                    {
                        candidate.Freq = freq;
                        candidate.Key = x;
                    }
                }
                route[i] = candidate;
            }
            return route;
        }

       
    }
}
