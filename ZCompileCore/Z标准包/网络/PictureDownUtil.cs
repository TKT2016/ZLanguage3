using System;
using System.IO;
using System.Net;


namespace Z标准包.网络
{
    public class PictureDownUtil
    {
        public static String GetFileNameByUrl(String url)
        {
            String[] arr = url.Split('/');
            if (arr.Length > 0)
            {
                return arr[arr.Length - 1];
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 从图片地址下载图片到本地磁盘
        /// </summary>
        /// <param name="ToLocalPath">图片本地磁盘地址</param>
        /// <param name="Url">图片网址</param>
        /// <returns></returns>
        public static bool SavePhotoFromUrl(string Url, string FileName)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            response = request.GetResponse();
            stream = response.GetResponseStream();
            if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                Value = SaveBinaryFile(response, FileName);
            }
            return Value;
        }

        /// <summary>
        /// 将二进制文件保存到磁盘
        /// </summary>
        private static bool SaveBinaryFile(WebResponse response, string FileName)
        {
            bool Value = true;
            byte[] buffer = new byte[1024];

            try
            {
                if (File.Exists(FileName))
                    File.Delete(FileName);
                Stream outStream = System.IO.File.Create(FileName);
                Stream inStream = response.GetResponseStream();

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();
            }
            catch
            {
                Value = false;
            }
            return Value;
        }

    }
}

