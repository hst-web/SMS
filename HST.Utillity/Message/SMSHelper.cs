using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace HST.Utillity
{
    public class SMSHelper
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="accountSid">账户id</param>
        /// <param name="authToken">授权码</param>
        /// <param name="appId">应用id</param>
        /// <param name="templateId">短信模板</param>
        /// <param name="to">手机号</param>
        /// <param name="param">参数</param>
        public static string templateSMS(string accountSid, string authToken,
           string appId, string templateId, string to, string param)
        {
            string ret = "发送成功";
            try
            {
                string date = DateTime.Now.ToString("yyyyMMddHHmmss");

                // 构建URL内容
                string sigstr = MD5Encrypt(accountSid + authToken + date);
                string uriStr;
                string xml = "";
                uriStr = string.Format("https://{0}:{1}/{2}/Accounts/{3}/Messages/templateSMS{4}?sig={5}", "api.ucpaas.com", "443", "2014-06-30", accountSid, xml, sigstr);

                Uri address = new Uri(uriStr);


                // 创建网络请求  
                HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

                // 构建Head
                request.Method = "POST";

                Encoding myEncoding = Encoding.GetEncoding("utf-8");
                byte[] myByte = myEncoding.GetBytes(accountSid + ":" + date);
                string authStr = Convert.ToBase64String(myByte);
                request.Headers.Add("Authorization", authStr);


                // 构建Body
                StringBuilder data = new StringBuilder();

                request.Accept = "application/json";
                request.ContentType = "application/json;charset=utf-8";

                data.Append("{");
                data.Append("\"templateSMS\":{");
                data.Append("\"appId\":\"").Append(appId).Append("\"");
                data.Append(",\"templateId\":\"").Append(templateId).Append("\"");
                data.Append(",\"to\":\"").Append(to).Append("\"");
                data.Append(",\"param\":\"").Append(param).Append("\"");
                data.Append("}}");


                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());

                // 开始请求
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                // 获取请求
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    //{"resp":{"respCode":"000000","templateSMS":{"createDate":"20160223152321","smsId":"6ca051b1599af6791fe7da58ce49692a"}}}
                    // Get the response stream  
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string responseStr = reader.ReadToEnd();
                    if (string.IsNullOrEmpty(responseStr))
                        return string.Empty;
                   
                    JObject job = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(responseStr);
                    job= (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(job["resp"].ToString());
                    string retCode = job["respCode"].ToString(); 
                    ret = GetErrorMsg(retCode);
                }

            }
            catch (Exception)
            {  
            }

            return ret;
        }

        private static string GetErrorMsg(string code)
        {
            string ret = "";
            if (code == "000000"){ ret = "发送成功"; }
            else if (code == "105100"){ ret = "发送失败：短信服务请求异常"; }
            else if (code == "105102") { ret = "发送失败：手机号码不合法"; }
            else if (code == "105106") { ret = "发送失败：不是国内手机号码并且不是国际电话"; }
            else if (code == "105107") { ret = "发送失败：手机号码在黑名单"; }
            else if (code == "105116") { ret = "发送失败：同一天同一用户不能发超过3条相同的短信"; }
            else if (code == "105122") { ret = "发送失败：同一天同一用户不能发超过10条验证码"; }
            else if (code == "105130") { ret = "发送失败：30秒内不能连续发同样的内容"; }
            else if (code == "105131") { ret = "发送失败：30秒内不能给同一号码发送相同模板消息"; }

            return ret;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="source">原内容</param>
        /// <returns>加密后内容</returns>
        public static string MD5Encrypt(string source)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            System.Security.Cryptography.MD5 md5Hasher = System.Security.Cryptography.MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));

            // Create a new Stringbuilder to collect the bytes and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("X2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
