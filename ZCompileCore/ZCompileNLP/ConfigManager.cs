using System;
using System.Configuration;
using System.IO;

namespace ZCompileNLP
{
    public class ConfigManager
    {
        static ConfigManager()
        {
            var domainDir = AppDomain.CurrentDomain.BaseDirectory;
            var configFileDir = Path.Combine(domainDir, "Resources");
            ConfigManager.ConfigFileBaseDir = configFileDir;
        }

        public static string ConfigFileBaseDir
        {
            get;
            set;
        }

        public static string YugeFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "yuge.txt"); }
        }

        public static string MainDictFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "dict.txt"); }
        }

        public static string ProbTransFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "prob_trans.json"); }
        }

        public static string ProbEmitFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "prob_emit.json"); }
        }

        public static string PosProbStartFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "pos_prob_start.json"); }
        }

        public static string PosProbTransFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "pos_prob_trans.json"); }
        }

        public static string PosProbEmitFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "pos_prob_emit.json"); }
        }

        public static string CharStateTabFile
        {
            get { return Path.Combine(ConfigFileBaseDir, "char_state_tab.json"); }
        }
    }
}