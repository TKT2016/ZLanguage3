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
    public static class DictItemBuff
    {
        private static readonly string MainDict = ConfigManager.MainDictFile;
        public static SegIntDictionary MainSegDict { get; private set; }
        private static Dictionary<List<string>, SegIntDictionary> Buff = new Dictionary<List<string>, SegIntDictionary>();

        static DictItemBuff()
        {
            LoadDict();
        }

        public static void ReBuild(List<string> key)
        {
            if(Contains(key))
            {
                Buff[key].ReBuild();
            }
        }

        public static bool Contains(List<string> key)
        {
            return (Buff.ContainsKey(key));
        }

        public static SegIntDictionary Get(List<string> key)
        {
            if(Buff.ContainsKey(key))
            {
                return Buff[key];
            }
            return null;
        }

        public static void Add(List<string> key, SegIntDictionary item)
        {
            Buff.Add(key,item);
        }

        public static void Delete(List<string> key)
        {
            if (Buff.ContainsKey(key))
            {
                Buff.Remove(key);
            }
        }

        private static void LoadDict()
        {
            MainSegDict = new SegIntDictionary();
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                using (var sr = new StreamReader(MainDict, Encoding.UTF8))
                {
                    string line = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var tokens = line.Split(' ');
                        if (tokens.Length < 2)
                        {
                            Debug.Fail(string.Format("Invalid line: {0}", line));
                            continue;
                        }

                        var word = tokens[0];
                        var freq = int.Parse(tokens[1]);

                        MainSegDict.Trie[word] = freq;
                        MainSegDict.Total += freq;

                        foreach (var ch in Enumerable.Range(0, word.Length))
                        {
                            var wfrag = word.Sub(0, ch + 1);
                            if (!MainSegDict.Trie.ContainsKey(wfrag))
                            {
                                MainSegDict.Trie[wfrag] = 0;
                            }
                        }
                    }
                }

                stopWatch.Stop();
                Debug.WriteLine("main dict load finished, time elapsed {0} ms", stopWatch.ElapsedMilliseconds);
            }
            catch (IOException e)
            {
                Debug.Fail(string.Format("{0} load failure, reason: {1}", MainDict, e.Message));
            }
            catch (FormatException fe)
            {
                Debug.Fail(fe.Message);
            }
        }
    }
}
