using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;

namespace ZCompileDesc.Utils
{
    public static  class IWordDictionaryHelper
    {
        public static bool ArrayContainsWord(string text, params IWordDictionary[] dictList)
        {
            foreach (var item in dictList)
            {
                if (item.ContainsWord(text))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool EnumerableContainsWord(string text, IEnumerable<IWordDictionary> dictList)
        {
            foreach (var item in dictList)
            {
                if (item.ContainsWord(text))
                {
                    return true;
                }
            }
            return false;
        }

        public static WordInfo EnumerableSearchWord(string text, IEnumerable<IWordDictionary> dictList)
        {
            List<WordInfo> words = new List<WordInfo>();
            foreach (IWordDictionary dict in dictList)
            {
                WordInfo word = dict.SearchWord(text);
                if (word != null)
                {
                    words.Add(word);
                }
            }
            if (words.Count > 0)
            {
                WordInfo newWord = WordInfo.Merge(words.ToArray());
                return newWord;
            }
            else
            {
                return null;
            }
        }

        public static WordInfo ArraySearchWord(string text, params IWordDictionary[] dictList)
        {
            List<WordInfo> words = new List<WordInfo>();
            foreach (IWordDictionary dict in dictList)
            {
                WordInfo word = dict.SearchWord(text);
                if (word != null)
                {
                    words.Add(word);
                }
            }
            if (words.Count > 0)
            {
                WordInfo newWord = WordInfo.Merge(words.ToArray());
                return newWord;
            }
            else
            {
                return null;
            }
        }
    }
}
