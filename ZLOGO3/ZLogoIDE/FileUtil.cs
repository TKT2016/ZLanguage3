using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ZLogoIDE
{
    public static class FileUtil
    {
        public static void WriteText(string fileName, string content)
        {
            using (StreamWriter toWrite = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                toWrite.Write(content);
                toWrite.Close();
            }
        }

        public static void AppendText(string fileName, string content)
        {
            using (StreamWriter toWrite = new StreamWriter(fileName, true, Encoding.UTF8))
            {
                toWrite.Write(content);
                toWrite.Close();
            }
        }

        public static string ReadText(string fileName)
        {
            FileStream fs = File.OpenRead(fileName);
            using (StreamReader reader = new StreamReader(fs))
            {
                string content = reader.ReadToEnd();
                reader.Close();
                return content;
            }
        }

        public static readonly String NewLine = "\r\n";

        //如果文件夹不存在则创建  
        public static bool CreateFolder(String path)
        {
            //File file = new File(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            else
            {
                return false;
            }
        }

        //如果文件夹不存在则创建
        public static bool CreateFile(String path)
        {
            //File file = new File(path);
            if (!File.Exists(path))
            {
                File.Create(path).Close();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
