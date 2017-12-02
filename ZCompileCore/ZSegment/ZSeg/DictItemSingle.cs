using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZNLP.Segment;
using ZNLP.Segment.Common;

namespace ZNLP.ZSeg
{
    public class DictItemSingle
    {
        public SegIntDictionary SegDictionary { get;private set; }

        public DictItemSingle(List<string> words)
        {
            SegDictionary = DictItemBuff.MainSegDict.Clone();
            foreach (var item in words)
            {
                SegDictionary.AddWord(item);
            }
        }

        //public void ReBuild()
        //{
        //    LoadWords();
        //}

        public Dictionary<int, List<int>> GetDag(string sentence)
        {
            var dag = new Dictionary<int, List<int>>();
            var trie = SegDictionary.Trie;

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
                    // TODO:
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
            return this.SegDictionary.Calc(sentence, dag);
        }

        //public void LoadWords()
        //{
        //    foreach(var item in UserWords)
        //    {
        //        AddWord(item);
        //    }
        //}

        //public void AddWord(string word, int freq=0, string tag = null)
        //{
        //    if (ContainsWordTrie(word))
        //    {
        //        Total -= Trie[word];
        //    }

        //    Trie[word] = freq;
        //    Total += freq;
        //    for (var i = 0; i < word.Length; i++)
        //    {
        //        var wfrag = word.Substring(0, i + 1);
        //        if (!Trie.ContainsKey(wfrag))
        //        {
        //            Trie[wfrag] = 0;
        //        }
        //    }
        //}

        //public int GetFreqOrDefault(string key)
        //{
        //    if (ContainsWordTrie(key))
        //        return Trie[key];
        //    else
        //        return 1;
        //}

        //public bool ContainsWordTrie(string word)
        //{
        //    return Trie.ContainsKey(word) && Trie[word] > 0;
        //}

        //public bool ContainsText(string text)
        //{
        //    if (this.UserWords == null) return false;
        //    return this.UserWords.IndexOf(text) != -1;
        //}
    }
}
