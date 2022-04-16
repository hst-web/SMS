using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace ZT.Utillity
{
    public static class HttpTool
    {
        #region HttpRequest
        /// <summary>
        /// HttpRequest
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="contentType"></param>
        /// <param name="charSet"></param>
        /// <param name="isPost"></param>
        /// <returns></returns>
        public static string HttpRequest(string url, string parameters = "", string contentType = "application/json", string charSet = "utf-8", bool isPost = true, int timeOut = 5000)
        {
            GC.Collect();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;
            Stream repStream = null;
            StreamReader resqStreamReader = null;
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (s, certs, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowWriteStreamBuffering = false;
                request.ProtocolVersion = HttpVersion.Version11;
                request.Headers.Add("Accept-Encoding", "gzip, deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.KeepAlive = true;
                request.Accept = "text/html, application/xhtml+xml, */*";
                request.Timeout = timeOut;
                request.ReadWriteTimeout = timeOut;
                request.Method = "GET";
                if (isPost)
                {
                    request.Method = "POST";
                    request.ContentType = contentType;
                    var postData = Encoding.GetEncoding(charSet).GetBytes(parameters);
                    request.ContentLength = postData.Length;
                    reqStream = request.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    reqStream.Flush();
                    reqStream.Close();
                }

                var sb = new StringBuilder(string.Empty);
                response = (HttpWebResponse)request.GetResponse();

                repStream = response.GetResponseStream();
                if (repStream != null)
                {
                    resqStreamReader = new StreamReader(repStream, Encoding.GetEncoding(charSet));
                    while (-1 < resqStreamReader.Peek())
                    {
                        sb.Append(resqStreamReader.ReadLine());
                    }
                    resqStreamReader.Close();
                    repStream.Close();
                }
                response.Close();
                return sb.ToString();
            }
            catch (WebException ex)
            {
                string text = string.Empty;
                var webresponse = (HttpWebResponse)ex.Response;
                if (webresponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    using (Stream data = webresponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            text = reader.ReadToEnd();
                        }
                    }
                }
                return text;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                resqStreamReader?.Dispose();
                repStream?.Dispose();
                reqStream?.Dispose();
                request?.Abort();
                response?.Dispose();
            }
        }
        #endregion

        #region 拼接字段中的数据
        /// <summary>
        /// 拼接字段中的数据
        /// </summary>
        /// <param name="iDict">集合类型的变量</param>
        /// <returns>返回生成的url链接</returns>
        public static string ToLink(this IDictionary<string, object> iDict)
        {
            var prestr = new StringBuilder();
            foreach (var dict in iDict)
            {
                prestr.Append($"{dict.Key}={dict.Value}&");
            }
            //去掉最後一個&字符
            prestr.Remove(prestr.Length - 1, 1);
            return prestr.ToString();
        }
        #endregion
    }
}