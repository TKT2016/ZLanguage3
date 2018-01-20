using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using ZNLP.Segment;
using ZNLP.Segment.Common;

namespace ZNLP.ZSeg
{
    public class SingleDictionary : ISegDictionary
    {
        List<string>[] WordsList;
        DictItemSingle DictInt;

        public SingleDictionary(List<string>[] wordsList)
        {
            WordsList = wordsList;
            LoadWords();
        }

        private void LoadWords()
        {
            List<string> words = new List<string>( );
            words.AddRange(DictItemBuff.MainSegDict.Words);
            foreach(var item in WordsList)
            {
                words.AddRange(item);
            }
            DictInt = new DictItemSingle(words);
            
        }

        public Dictionary<int, List<int>> GetDag(string sentence)
        {
            return DictInt.GetDag(sentence);
           
        }

        public Dictionary<int, Pair<int>> Calc(string sentence, IDictionary<int, List<int>> dag)
        {
            return DictInt.Calc( sentence, dag);
            
        }

        public bool ContainsWordTrie(string word)
        {
            return DictInt.ContainsWordTrie(word);
        }

        public bool ContainsText(string word)
        {
            return DictInt.ContainsText(word);
        }
    }
}