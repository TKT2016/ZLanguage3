using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZCompileDesc.Collections;

namespace ZCompileDesc.Words
{
    public class WordDictionary : Dictionary<string, WordInfo>, IWordDictionary
    {
        public string Key { get; protected set; }

        public WordDictionary(string name)
        {
            this.Key = name;
        }

        public void Add(WordInfo word)
        {
            string key = word.Text;
            if(this.ContainsKey(key))
            {
                WordInfo word2 = this[key];
                WordInfo newWord = WordInfo.Merge(word, word2);
                this[key] = newWord;
            }
            else
            {
                this.Add(key, word);
            }
        }

        public bool ContainsWord(string text)
        {
            return this.ContainsKey(text);
        }

        public WordInfo SearchWord(string text)
        {
            if(this.ContainsKey(text))
            {
                return this[text];
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return string.Format("{0}[{1}]",Key,this.Keys.Count);
        }
    }
}
