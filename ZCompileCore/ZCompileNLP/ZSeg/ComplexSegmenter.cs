using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ZNLP.Segment;
using ZNLP.Segment.Common;
using ZNLP.Segment.FinalSeg;

namespace ZNLP.ZSeg
{
    public class ComplexSegmenter
    {
        private static readonly IFinalSeg FinalSeg = Viterbi.Instance;
        private static readonly ISet<string> LoadedPath = new HashSet<string>();

        private static readonly object locker = new object();

        internal IDictionary<string, string> UserWordTagTab { get; set; }
        public ISegDictionary IWDict { get; private set; }
        #region Regular Expressions

        internal static readonly Regex RegexChineseDefault = new Regex(@"([\u4E00-\u9FD5a-zA-Z0-9+#&\._]+)", RegexOptions.Compiled);

        internal static readonly Regex RegexSkipDefault = new Regex(@"(\r\n|\s)", RegexOptions.Compiled);

        internal static readonly Regex RegexChineseCutAll = new Regex(@"([\u4E00-\u9FD5]+)", RegexOptions.Compiled);
        internal static readonly Regex RegexSkipCutAll = new Regex(@"[^a-zA-Z0-9+#\n]", RegexOptions.Compiled);

        internal static readonly Regex RegexEnglishChars = new Regex(@"[a-zA-Z0-9]", RegexOptions.Compiled);

        internal static readonly Regex RegexUserDict = new Regex("^(?<word>.+?)(?<freq> [0-9]+)?(?<tag> [a-z]+)?$", RegexOptions.Compiled);

        #endregion

        public ComplexSegmenter(ISegDictionary complexDictionary)
        {
            UserWordTagTab = new Dictionary<string, string>();
            this.IWDict = complexDictionary;
        }

        public string[] Cut(string text)
        {
            
            var reHan = RegexChineseDefault;
            var reSkip = RegexSkipDefault;
            Func<string, IEnumerable<string>> cutMethod = CutDag;
            return CutIt(text, cutMethod, reHan, reSkip, false).ToArray();
        }

        #region Internal Cut Methods

        internal IEnumerable<string> CutDag(string sentence)
        {
            if (sentence.IndexOf("»æÍ¼Æ÷") != -1)
            {
                Debug.WriteLine("»æÍ¼Æ÷");
            }
            var dag = this.IWDict.GetDag(sentence); //GetDag(sentence);
            var route = this.IWDict.Calc(sentence, dag); //Calc(sentence, dag);

            var tokens = new List<string>();

            var x = 0;
            var n = sentence.Length;
            var buf = string.Empty;
            while (x < n)
            {
                var y = route[x].Key + 1;
                var w = sentence.Substring(x, y - x);
                if (y - x == 1)
                {
                    buf += w;
                }
                else
                {
                    if (buf.Length > 0)
                    {
                        AddBufferToWordList(tokens, buf);
                        buf = string.Empty;
                    }
                    tokens.Add(w);
                }
                x = y;
            }

            if (buf.Length > 0)
            {
                AddBufferToWordList(tokens, buf);
            }

            return tokens;
        }

        internal IEnumerable<string> CutIt(string text, Func<string, IEnumerable<string>> cutMethod,
                                           Regex reHan, Regex reSkip, bool cutAll)
        {
            var result = new List<string>();
            var blocks = reHan.Split(text);
            foreach (var blk in blocks)
            {
                if (string.IsNullOrWhiteSpace(blk))
                {
                    continue;
                }

                if (reHan.IsMatch(blk))
                {
                    foreach (var word in cutMethod(blk))
                    {
                        result.Add(word);
                    }
                }
                else
                {
                    var tmp = reSkip.Split(blk);
                    foreach (var x in tmp)
                    {
                        if (reSkip.IsMatch(x))
                        {
                            result.Add(x);
                        }
                        else if (!cutAll)
                        {
                            foreach (var ch in x)
                            {
                                result.Add(ch.ToString());
                            }
                        }
                        else
                        {
                            result.Add(x);
                        }
                    }
                }
            }

            return result;
        }

        #endregion

        #region Private Helpers

        private void AddBufferToWordList(List<string> words, string buf)
        {
            if (buf.Length == 1)
            {
                words.Add(buf);
            }
            else
            {
                if (!IWDict.ContainsWordTrie(buf))
                {
                    var tokens = FinalSeg.Cut(buf);
                    words.AddRange(tokens);
                }
                else
                {
                    words.AddRange(buf.Select(ch => ch.ToString()));
                }
            }
        }

        #endregion
    }
}