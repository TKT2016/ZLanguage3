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
    public class ComplexDictionary : ISegDictionary
    {
        List<string>[] WordsList;
        List<SegIntDictionary> DictItemes;

        public ComplexDictionary(List<string>[] wordsList)
        {
            WordsList = wordsList;
            LoadWords();
        }

        private void LoadWords()
        {
            DictItemes = new List<SegIntDictionary>();
            DictItemes.Add(DictItemBuff.MainSegDict);
            foreach(var list in WordsList)
            {
                if(DictItemBuff.Contains(list))
                {
                    DictItemes.Add(DictItemBuff.Get(list));
                }
                else
                {
                    SegIntDictionary ditem = new SegIntDictionary(list);
                    DictItemes.Add(ditem);
                    DictItemBuff.Add(list, ditem);
                }
            }
        }

        public Dictionary<int, List<int>> GetDag(string sentence)
        {
            Dictionary<int, List<int>> result = new Dictionary<int,List<int>> ();
            foreach(var item in DictItemes)
            {
                Dictionary<int, List<int>> temp = item.GetDag(sentence);
                foreach(var key in temp.Keys)
                {
                    if (result.ContainsKey(key))
                    {
                        result[key].AddRange(temp[key]);
                    }
                    else
                    {
                        result.Add(key, temp[key]);
                    }
                }
            }
            for (int i = 0; i < result.Keys.Count;i++ )
            {
                    var key = result.Keys.ToArray()[i];
                    result[key] = result[key].Distinct().ToList();
             }
            return result;
        }

        public Dictionary<int, Pair<int>> Calc(string sentence, IDictionary<int, List<int>> dag)
        {
            Dictionary<int, Pair<int>> result = new Dictionary<int, Pair<int>>(); 
            foreach (var item in DictItemes)
            {
                Dictionary<int, Pair<int>> temp = item.Calc(sentence, dag);
                foreach (var key in temp.Keys)
                {
                    if (result.ContainsKey(key))
                    {
                        //result[key].AddRange(temp[key]);
                    }
                    else
                    {
                        result.Add(key, temp[key]);
                    }
                }
            }
            //foreach (var key in result.Keys)
            //{
            //    result[key] = result[key].Distinct().ToList();
            //}
            return result;
        }

        public bool ContainsWordTrie(string word)
        {
            if (word.IndexOf("»æ") != -1)
            {
                Debug.WriteLine("»æ");
            }
            foreach(var item in DictItemes)
            {
                if(item.ContainsWordTrie(word))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsText(string word)
        {
            foreach (var item in DictItemes)
            {
                if (item.ContainsText(word))
                {
                    return true;
                }
            }
            return false;
        }
    }
}