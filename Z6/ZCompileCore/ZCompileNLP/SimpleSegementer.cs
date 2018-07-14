using ZCompileNLP.Segment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileNLP
{
    public class SimpleSegementer : ISegementer
    {
        JiebaSegmenter segmenter;

        public SimpleSegementer()
        {
            segmenter = new JiebaSegmenter();
            /* 添加Z语言关键字 */
            segmenter.AddWord("每一个");
            segmenter.AddWord("否则如果");
            segmenter.AddWord("重复");
            segmenter.AddWord("新的");
        }

        public void AddWord(string word)
        {
            segmenter.AddWord(word);
        }

        public void DeleteWord(string word)
        {
            segmenter.DeleteWord(word);
        }

        public string[] Cut(string src)
        {
            IEnumerable<string> tokens = segmenter.Cut(src);
            return tokens.ToArray();

            //if (src.Length == 2)
            //{

            //}
            //else
            //{
            //    IEnumerable<string> tokens = segmenter.Cut(src);
            //    return tokens.ToArray();
            //}
        }
    }
}
