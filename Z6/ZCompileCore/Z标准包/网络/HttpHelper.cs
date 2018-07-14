using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Z标准包.网络
{
    public static class HttpHelper
    {
        /// <summary>
        /// 根据网址获取网页html
        /// </summary>
        public static string GetHtml(string strUrl)
        {
            Uri uri = new Uri(strUrl);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(uri);
            myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
            Stream receviceStream = result.GetResponseStream();
            StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8"));
            string strHTML = readerOfStream.ReadToEnd();
            readerOfStream.Close();
            receviceStream.Close();
            result.Close();
            return strHTML;
        }

        /// <summary>
        /// 以Post 形式提交数据到 uri
        /// </summary>
        public static string Post(Uri uri,Dictionary<string,object> values)
        {
            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            MemoryStream stream = new MemoryStream();
            byte[] line = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            //提交文本字段
            if (values != null)
            {
                string format = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\";\r\n\r\n{1}";
                foreach (string key in values.Keys)
                {
                    string s = string.Format(format, key, values[key]);
                    byte[] data = Encoding.UTF8.GetBytes(s);
                    stream.Write(data, 0, data.Length);
                }
                stream.Write(line, 0, line.Length);
            }

            request.ContentLength = stream.Length;
            Stream requestStream = request.GetRequestStream();
            stream.Position = 0L;
            stream.CopyTo(requestStream);
            stream.Close();
            requestStream.Close();

            using (var response = request.GetResponse())
            using (var responseStream = response.GetResponseStream())
            using (var mstream = new MemoryStream())
            {
                responseStream.CopyTo(mstream);
                var byteArray = mstream.ToArray();
                string str = System.Text.Encoding.UTF8.GetString(byteArray);
                return str;
            }
        }

    }
}
