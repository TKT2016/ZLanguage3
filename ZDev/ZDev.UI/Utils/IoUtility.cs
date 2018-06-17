using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZDev.Util
{
    public static class IoUtility
    {
        public static void SaveFile( string fileName,string content)
        {
            using (System.IO.StreamWriter toWrite = new System.IO.StreamWriter(fileName,false,Encoding.UTF8))
            {
                toWrite.Write(content);
                toWrite.Close();
            }
        }

        public static string ReadFile(string fileName)
        {
            FileStream fs = File.OpenRead(fileName);
            using (StreamReader reader = new StreamReader(fs))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }

        public static void CheckCreate(string folder)
        {
            if (Directory.Exists(folder) == false)
            {
                Directory.CreateDirectory(folder);
            }
        }
    }
}
