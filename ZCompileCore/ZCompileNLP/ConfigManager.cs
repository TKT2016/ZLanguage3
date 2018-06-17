using System;
using System.Configuration;
using System.IO;

namespace ZCompileNLP
{
    public class ConfigManager
    {
        static ConfigManager()
        {
            //var domainDir = AppDomain.CurrentDomain.BaseDirectory;
            //var configFileDir = Path.Combine(domainDir, "NlpRes");
            //ConfigManager.ConfigFileBaseDir = configFileDir;

            ConfigManager.ConfigFileBaseDir = "ZCompileNLP.NlpRes.";
        }

        public static bool LoadMainDictFile { get; set; }

        public static string ConfigFileBaseDir
        {
            get;
            set;
        }

        //public static string YugeFile
        //{
        //    get { return ConfigFileBaseDir +"yuge.txt";}// Path.Combine(ConfigFileBaseDir, "yuge.txt"); }
        //}

        public static string MainDictFile
        {
            get { return ConfigFileBaseDir +"dict.txt";}// Path.Combine(ConfigFileBaseDir, "dict.txt"); }
        }

        public static string ProbTransFile
        {
            get { return ConfigFileBaseDir + "prob_trans.json"; }// Path.Combine(ConfigFileBaseDir, "prob_trans.json"); }
        }

        public static string ProbEmitFile
        {
            get { return ConfigFileBaseDir + "prob_emit.json"; }// Path.Combine(ConfigFileBaseDir, "prob_emit.json"); }
        }

        public static string PosProbStartFile
        {
            get { return ConfigFileBaseDir + "pos_prob_start.json"; }// Path.Combine(ConfigFileBaseDir, "pos_prob_start.json"); }
        }

        public static string PosProbTransFile
        {
            get { return ConfigFileBaseDir + "pos_prob_trans.json"; }// Path.Combine(ConfigFileBaseDir, "pos_prob_trans.json"); }
        }

        public static string PosProbEmitFile
        {
            get { return ConfigFileBaseDir + "pos_prob_emit.json"; }// Path.Combine(ConfigFileBaseDir, "pos_prob_emit.json"); }
        }

        public static string CharStateTabFile
        {
            get { return ConfigFileBaseDir + "char_state_tab.json"; }// Path.Combine(ConfigFileBaseDir, "char_state_tab.json"); }
        }
    }
}