using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZNLP
{
    public static class FormWorderManager
    {
        private static readonly string MainDictFile = ConfigManager.YugeFile;
        static Dictionary<string, int> Dict = new Dictionary<string, int>();

        static FormWorderManager()
        {
            LoadDict();
        }

        public static int Get(string word)
        {
            if(Dict.ContainsKey(word))
            {
                return Dict[word];
            }
            return -1;
        }

        private static void LoadDict()
        {
            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                using (var sr = new StreamReader(MainDictFile, Encoding.UTF8))
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
                        Dict.Add(word, freq);
                    }
                }

                stopWatch.Stop();
                //Debug.WriteLine("main dict load finished, time elapsed {0} ms", stopWatch.ElapsedMilliseconds);
            }
            catch (IOException e)
            {
                Debug.Fail(string.Format("{0} load failure, reason: {1}", MainDictFile, e.Message));
            }
            catch (FormatException fe)
            {
                Debug.Fail(fe.Message);
            }
        }
    }
}
