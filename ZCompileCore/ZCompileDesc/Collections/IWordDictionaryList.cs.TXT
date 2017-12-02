using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;
using ZCompileDesc.Words;

namespace ZCompileDesc.Collections
{
    public class IWordDictionaryList : List<IWordDictionary>, IWordDictionary
    {
        public WordInfo SearchWord(string text)
        {
            List<WordInfo> words = new List<WordInfo>();
            foreach (IWordDictionary dict in this)
            {
                WordInfo word = dict.SearchWord(text);
                if(word!=null)
                {
                    words.Add(word);
                }
            }
            if (words != null)
            {
                WordInfo newWord = WordInfo.Merge(words.ToArray());
                return newWord;
            }
            else
            {
                return null;
            }
        }

        public bool ContainsWord(string text)
        {
            foreach (WordDictionary dict in this)
            {
                WordInfo word = dict.SearchWord(text);
                if (word != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
