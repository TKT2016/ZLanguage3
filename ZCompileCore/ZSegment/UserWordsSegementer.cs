using ZNLP.Segment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNLP
{
    public class UserWordsSegementer : ISegementer
    {
        SimpleSegementer segmenter;
        List<string> UserWords = new List<string> ();

        public UserWordsSegementer()
        {
            segmenter = new SimpleSegementer();
        }

        public void AddWord(string word)
        {
            segmenter.AddWord(word);
            UserWords.Add(word);
        }

        public void AddWords(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                AddWord(word);
            }
        }

        public void DeleteWords(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                segmenter.DeleteWord(word);
                UserWords.Remove(word);
            }
        }

        public string[] Cut(string src)
        {
            if (UserWords.IndexOf(src) != -1) return new string[]{ src};
            IEnumerable<string> tokens = segmenter.Cut(src);
            return tokens.ToArray();
        }

        public UserWordsSegementer Clone()
        {
            UserWordsSegementer uws2 = new UserWordsSegementer();
            foreach(var item in this.UserWords)
            {
                uws2.AddWord(item);
            }
            return uws2;
        }

    }
}
