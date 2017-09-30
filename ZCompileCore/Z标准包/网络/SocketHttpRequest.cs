using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Z标准包.网络
{
    public static class SocketHttpRequest
    {
        /// <summary>
        /// 使用socket得到网页html
        /// </summary>
        /// <param name="strUrl">网页url</param>
        /// <returns>网页html代码</returns>
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
        /// 得到网页原始字节数组
        /// </summary>
        /// <param name="strHost">主机头</param>
        /// <param name="getBytes">Get字符串的字节数组形式</param>
        /// <param name="iTotalCount">接受的字节数</param>
        /// <returns>原始网页字节数组</returns>
        private static byte[] GetHtmlOriginByte(string strHost, byte[] getBytes, out int iTotalCount)
        {
            int iReponseByteSize = 400 * 1024;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(strHost, 80);
            socket.Send(getBytes);
            byte[] buffer = new byte[256];
            byte[] responseBytes = new byte[iReponseByteSize];
            int iNumber = socket.Receive(buffer, buffer.Length, SocketFlags.None);
            iTotalCount = iNumber;
            buffer.CopyTo(responseBytes, 0);
            while (iNumber > 0)
            {
                iNumber = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                if (iTotalCount + iNumber >= responseBytes.Length)
                {
                    //重新生成个更大的数组
                    byte[] temp = new byte[responseBytes.Length * 2];
                    //原数据copy到新数组中
                    responseBytes.CopyTo(temp, 0);
                    buffer.CopyTo(temp, iTotalCount - 1);
                    responseBytes = temp; //引用变更
                }
                else
                {
                    buffer.CopyTo(responseBytes, iTotalCount - 1);
                }
                iTotalCount += iNumber; //索引位置增加
            }
            return responseBytes;
        }
        /// <summary>
        /// 解压网页
        /// </summary>
        /// <param name="responseBytes">网页字节数组含http头</param>
        /// <param name="iTotalCount">数组长度</param>
        /// <param name="strHeader">Http头字符串</param>
        /// <param name="iStart">网页正文开始位置</param>
        private static void DecompressWebPage(ref byte[] responseBytes, ref int iTotalCount, string strHeader, int iStart)
        {
            byte[] szSwapBuffer = new byte[iTotalCount];
            Array.Copy(responseBytes, iStart, szSwapBuffer, 0, iTotalCount);
            Regex regZip = new Regex(@"Content-Encoding:\s+gzip[^\n]*\r\n", RegexOptions.IgnoreCase);

            if (regZip.IsMatch(strHeader))
            {
                responseBytes = Decompress(szSwapBuffer);
                iTotalCount = responseBytes.Length; //解压后长度变了
            }
            else
            {
                responseBytes = szSwapBuffer;
            }
        }
        /// <summary>
        /// 根据网页meta标记里面的字符编码解析字符串
        /// </summary>
        /// <param name="responseBytes">网页内容字节数组(除http头以外的内容)</param>
        /// <param name="iTotalCount">网页内容字节数组长度</param>
        /// <param name="strResponse">网页内容字符串, 可能已经根据其它转码要求转换过的字符串</param>
        /// <returns>转好的字符串</returns>
        private static string DecodeWebStringByHtmlPageInfo(byte[] responseBytes, int iTotalCount, string strResponse)
        {
            Regex regGB2312 = new Regex(@"<meta[^>]+Content-Type[^>]+gb2312[^>]*>", RegexOptions.IgnoreCase);
            Regex regGBK = new Regex(@"<meta[^>]+Content-Type[^>]+gbk[^>]*>", RegexOptions.IgnoreCase);
            Regex regBig5 = new Regex(@"<meta[^>]+Content-Type[^>]+Big5[^>]*>", RegexOptions.IgnoreCase);
            if (regGB2312.IsMatch(strResponse) || regGBK.IsMatch(strResponse))
                strResponse = Encoding.GetEncoding("GBK").GetString(responseBytes, 0, iTotalCount);
            if (regBig5.IsMatch(strResponse))
                strResponse = Encoding.GetEncoding("Big5").GetString(responseBytes, 0, iTotalCount);
            return strResponse;
        }
        /// <summary>
        /// 根据Http头标记里面的字符编码解析字符串
        /// </summary>
        /// <param name="responseBytes">网页内容字节数组(除http头以外的内容)</param>
        /// <param name="iTotalCount">网页内容字节数组长度</param>
        /// <param name="strHeader">http头的字符串</param>
        /// <returns>转好的字符串</returns>
        private static string DecodeWebStringByHttpHeader(byte[] responseBytes, int iTotalCount, string strHeader)
        {
            string strResponse = "";
            if (strHeader.Contains("charset=GBK") || strHeader.Contains("charset=gb2312"))
            {
                strResponse = Encoding.GetEncoding("GBK").GetString(responseBytes, 0, iTotalCount);
            }
            else
                strResponse = Encoding.UTF8.GetString(responseBytes, 0, iTotalCount);
            return strResponse;
        }
        /// <summary>
        /// 是否已跳转
        /// </summary>
        /// <param name="strResponse">http响应字符串</param>
        /// <returns>bool值 是否跳转</returns>
        private static bool HasRelocated(ref string strResponse)
        {
            bool hasRelocated = false;
            int iIndex = strResponse.IndexOf("\r\n\r\n");
            string strHeader = strResponse.Substring(0, iIndex);//头部
            if (strResponse.StartsWith("HTTP/1.1 302") || strResponse.StartsWith("HTTP/1.1 301"))//跳转
            {
                int iStart = strHeader.IndexOf("\r\nLocation: ");
                string strRelocateUrl = strHeader.Substring(iStart + "\r\nLocation: ".Length);
                int iEnd = strRelocateUrl.IndexOf("\r\n");
                strRelocateUrl = strRelocateUrl.Substring(0, iEnd);
                strResponse = GetHtml(strRelocateUrl); //递归的获取
                hasRelocated = true;
            }
            return hasRelocated;
        }
        /// <summary>
        /// 查找html开始部分(去除了html头)\r\n\r\n后面第一个byte的位置
        /// </summary>
        /// <param name="szHtmlBuffer">接受到的byte数组</param>
        /// <returns>第一个byte位置</returns>
        private static int FindBodyStartPosition(byte[] szHtmlBuffer)
        {
            for (int i = 0; i < szHtmlBuffer.Length - 3; ++i)
            {
                if (szHtmlBuffer[i] == 13 && szHtmlBuffer[i + 1] == 10 && szHtmlBuffer[i + 2] == 13 && szHtmlBuffer[i + 3] == 10)
                    return i + 4;
            }
            return -1;
        }
        /// <summary>
        /// 得到请求资源的相对路径
        /// </summary>
        /// <param name="strUrl">Url字符串</param>
        /// <returns>url的相对路径</returns>
        private static string GetRelativeUrl(string strUrl)
        {
            int iIndex = strUrl.IndexOf(@"//");
            if (iIndex <= 0)
                return "/";
            string strTemp = strUrl.Substring(iIndex + 2);
            iIndex = strTemp.IndexOf(@"/");
            if (iIndex > 0)
                return strTemp.Substring(iIndex);
            else
                return "/";
        }

        /// <summary>
        /// 根据Url得到host
        /// </summary>
        /// <param name="strUrl">url字符串</param>
        /// <returns>host字符串</returns>
        private static string GetHost(string strUrl)
        {
            int iIndex = strUrl.IndexOf(@"//");
            if (iIndex <= 0)
                return "";
            string strTemp = strUrl.Substring(iIndex + 2);
            iIndex = strTemp.IndexOf(@"/");
            if (iIndex > 0)
                return strTemp.Substring(0, iIndex);
            else
                return strTemp;
        }

        /// <summary>
        /// 解压gzip网页
        /// </summary>
        /// <param name="szSource">压缩过的字符串字节数组</param>
        /// <returns>解压后的字节数组</returns>
        private static byte[] Decompress(byte[] szSource)
        {
            MemoryStream msSource = new MemoryStream(szSource);
            //DeflateStream  也可以这儿
            GZipStream stream = new GZipStream(msSource, CompressionMode.Decompress);
            byte[] szTotal = new byte[40 * 1024];
            long lTotal = 0;
            byte[] buffer = new byte[8];
            int iCount = 0;
            do
            {
                iCount = stream.Read(buffer, 0, 8);
                if (szTotal.Length <= lTotal + iCount) //放大数组
                {
                    byte[] temp = new byte[szTotal.Length * 10];
                    szTotal.CopyTo(temp, 0);
                    szTotal = temp;
                }
                buffer.CopyTo(szTotal, lTotal);
                lTotal += iCount;
            } while (iCount != 0);
            byte[] szDest = new byte[lTotal];
            Array.Copy(szTotal, 0, szDest, 0, lTotal);
            return szDest;
        }
    }
}


