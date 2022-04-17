using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.Utillity
{
    public class FGSMSHelper
    {
        private const string apikey = "N75044fef2";
        private const string secret = "75044f4dc59baca1";

        /// <summary>
        /// 模板短信
        /// </summary>
        /// <param name="signId">短信签名</param>
        /// <param name="templateId">模板id</param>
        /// <param name="content">content为变量值，多个变量以||进行分隔，按顺序排列 如：变量1||变量2</param>
        /// <param name="mobile">手机号码  多个英文,隔开</param>
        /// <returns></returns>
        public static FGSMSResponse TemplateSMS(string signId, string templateId, string content, string mobile)
        {
            FGSMSResponse response = new FGSMSResponse();
            var dic = new SortedDictionary<string, object>(StringComparer.Ordinal)
                        {
                            {"apikey",apikey},//通知短信/营销短信产品接口账号
                            {"secret",secret},//与接口账号对应的秘钥
                            {"content", content},//短信内容
                            {"sign_id",signId},//短信签名
                            {"template_id",templateId},
                            {"mobile",mobile}//手机号码  多个英文,隔开
                        };

            string res = HttpTool.HttpRequest("https://api.4321.sh/sms/template", dic.Serialize(), "application/json");

            try
            {
                response = JsonUtils.Deserialize<FGSMSResponse>(res);
            }
            catch (JsonReaderException)
            {
                response.code = -1;
                response.msg = res;
            }

            return response;
        }

        /// <summary>
        /// 普通短信
        /// </summary>
        /// <param name="signId">短信签名</param>
        /// <param name="content">要发送的文本内容</param>
        /// <param name="mobile">手机号码  多个英文,隔开</param>
        /// <returns></returns>
        public static FGSMSResponse ContentSMS(string signId, string content, string mobile)
        {
            FGSMSResponse response = new FGSMSResponse();
            var dic = new SortedDictionary<string, object>(StringComparer.Ordinal)
                        {
                            {"apikey",apikey},//通知短信/营销短信产品接口账号
                            {"secret",secret},//与接口账号对应的秘钥
                            {"content", content},//短信内容
                            {"sign_id",signId},//短信签名
                            {"mobile",mobile}//手机号码  多个英文,隔开
                        };

            string res = HttpTool.HttpRequest("https://api.4321.sh/sms/send", dic.Serialize(), "application/json");

            try
            {
                response = JsonUtils.Deserialize<FGSMSResponse>(res);
                //重试一次
                if (!response.success && response.code > 15)
                {
                    res = HttpTool.HttpRequest("https://api.4321.sh/sms/send", dic.Serialize(), "application/json");
                    response = JsonUtils.Deserialize<FGSMSResponse>(res);
                }
            }
            catch (JsonReaderException)
            {
                response.code = -1;
                response.msg = res;
            }

            return response;
        }

        /// <summary>
        /// 导出发送报表，需要间隔5s 一次请求
        /// </summary>
        /// <param name="count">最大200条</param>
        /// <returns></returns>
        public static FGSMSReportResponse GetSMSReport(int count = 200)
        {
            FGSMSReportResponse response = new FGSMSReportResponse();
            var dic = new SortedDictionary<string, object>(StringComparer.Ordinal)
                            {
                                {"apikey",apikey},//通知短信/营销短信产品接口账号
                                {"secret",secret},//与接口账号对应的秘钥
                                {"count",count+""}//获取报告条数 10-200
                            };
            var res = HttpTool.HttpRequest("https://api.4321.sh/sms/report", dic.Serialize());

            try
            {
                response= JsonUtils.Deserialize<FGSMSReportResponse>(res);
            }
            catch (JsonReaderException)
            {
                response.code = -1;
                response.msg = res;
            }

            return response;
        }
    }

    public class FGSMSResponse
    {
        public string msg_no { get; set; }
        public string msg { get; set; }
        public int count { get; set; }
        public int code { get; set; }
        public bool success
        {
            get
            {
                return code == 0;
            }
        }
        public string errorMsg
        {
            get
            {
                string ret = "";
                if (code == 0) { ret = "发送成功"; }
                else if (code == 1) { ret = "发送失败：登录授权有误"; }
                else if (code == 15) { ret = "发送失败：余额不足，请及时充值"; }
                else if (code == 100) { ret = "发送失败：内部错误"; }
                else if (code == 10001) { ret = "发送失败：参数为空"; }
                else if (code == 10002) { ret = "发送失败：参数有误"; }
                else if (code == 10003) { ret = "发送失败：发送号码个数限制"; }
                else if (code == 10004) { ret = "发送失败：暂无状态报告ID"; }
                else if (code == 10005) { ret = "发送失败：暂无上行"; }
                else if (code == 10006) { ret = "发送失败：暂无配置通道"; }
                else if (code == 10007) { ret = "发送失败：发送频率限制"; }
                else if (code == 10008) { ret = "发送失败：超流速限制"; }
                else if (code == 10009) { ret = "发送失败：取号失败"; }
                else if (code == -1) { ret = "系统异常"; }
                return ret;
            }
        }
    }

    public class FGSMSReportResponse
    {
        public List<ReportInfo> data { get; set; }
        public bool success
        {
            get
            {
                return code == 0;
            }
        }
        public string msg { get; set; }
        public int code { get; set; }
    }

    public class ReportInfo
    {
        public string msg_no { get; set; }
        public string mobile { get; set; }
        public string status { get; set; }
        public bool success
        {
            get
            {
                return string.Equals("DELIVRD", status, StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
