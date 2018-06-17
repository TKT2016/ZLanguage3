using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ZCompileNLP
{
    public static class ManifestResourceReader
    {
        public static Stream GetStream(string resourceName)
        {
            Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            if (sm == null) throw new NullReferenceException("嵌入的资源'"+resourceName+"'不存在");
            return sm;
        }

        public static string[] ReadText(string resourceName, Encoding encoding)
        {
            //Stream sm =  GetStream(resourceName);//Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            //byte[] bs = new byte[sm.Length];
            //sm.Read(bs, 0, (int)sm.Length);
            //sm.Close();
            //UTF8Encoding con = new UTF8Encoding();
            string str = ReadAllText(resourceName, encoding);// encoding.GetString(bs);
            string[] lines = str.Split(new string[]{ Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            return lines;
            //MessageBox.Show(str);
        }

        public static string ReadAllText(string resourceName, Encoding encoding)
        {
            Stream sm = GetStream(resourceName);
            byte[] bs = new byte[sm.Length];
            sm.Read(bs, 0, (int)sm.Length);
            sm.Close();
            //UTF8Encoding con = new UTF8Encoding();
            string str = encoding.GetString(bs);
            return str;
        }
    }
}
