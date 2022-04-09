using HST.Art.Core;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using HST.Utillity;
using HST.Art.Service;
using System.Collections.Generic;
using System.Web.Configuration;
using System.IO;
using System.Linq;
using NPOI.SS.UserModel;

namespace HST.Art.Web
{
    public class ApplicationBase : Controller
    {
        private Dictionary<int, string> _cityDic;
        private Dictionary<int, string> _provinceDic;
        private const int MAX_COOKIESTIME = 4;//最大cookies储存时间
        public string ErrorMsg = string.Empty;
        string locAddr = WebConfigurationManager.AppSettings["WebUrl"].ToString();//图片上传地址
        public string IP
        {
            get
            {
                return PlatformAgent.Get().IPAddress;
            }
        }
        public int UserId
        {
            get
            {
                Account account = GetAccount();
                if (account != null)
                {
                    return account.Id;
                }

                return 0;
            }
        }

        public string UserName
        {
            get
            {
                Account account = GetAccount();
                if (account != null)
                {
                    return account.UserName;
                }

                return string.Empty;
            }
        }

        public string UserAgent
        {
            get
            {
                return JsonConvert.SerializeObject(PlatformAgent.Get());
            }
        }

        private static IntegratedService _integratedService;
        /// <summary>
        /// 日志服务
        /// </summary>
        public static IntegratedService IntegratedService
        {
            get
            {
                _integratedService = new IntegratedService();
                return _integratedService;
            }
            set { _integratedService = value; }
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            FillAgent();
            if (filterContext.RouteData.DataTokens["area"] != null && filterContext.RouteData.DataTokens["area"].Equals("manage"))
            {
                SetCurrentCookies = filterContext.RequestContext.HttpContext.Request.Cookies;
                string cookieStr = GetCookieStr();
                if (string.IsNullOrEmpty(cookieStr))
                {
                    string tmpRouteName = RouteData.GetRequiredString("controller");
                    string tmpActionName = RouteData.GetRequiredString("action");
                    if (!tmpRouteName.Equals("account", StringComparison.InvariantCultureIgnoreCase) || (tmpRouteName.Equals("account", StringComparison.InvariantCultureIgnoreCase) && string.Equals(tmpActionName, "index", StringComparison.InvariantCultureIgnoreCase)))
                    {
                        //filterContext.Result = new RedirectResult("/Account/Login");
                        filterContext.Result = new EmptyResult();
                        filterContext.HttpContext.Response.Write("<script>top.location ='/manage/account/login';</script>");
                        filterContext.HttpContext.Response.End();
                    }
                }
                else if (string.IsNullOrEmpty(CookiesEvent.GetCookies(SetCurrentCookies, "accountInfo")))
                {
                    RemoveStoredData();
                    filterContext.Result = new EmptyResult();
                    filterContext.HttpContext.Response.Write("<script>top.location ='/manage/account/login';</script>");
                    filterContext.HttpContext.Response.End();
                }
            }
        }

        /// <summary>
        /// 防伪cookie
        /// </summary>
        public HttpCookieCollection SetCurrentCookies
        {
            get; set;
        }

        public Dictionary<int, string> City
        {
            get
            {
                Dictionary<int, string> dicCity = JsonConvert.DeserializeObject<Dictionary<int, string>>(Constant.AREA_CITY);
                if (dicCity != null) _cityDic = dicCity;
                return _cityDic;
            }
        }

        public Dictionary<int, string> Province
        {
            get
            {
                Dictionary<int, string> dicProvince = JsonConvert.DeserializeObject<Dictionary<int, string>>(Constant.AREA_PROVINCE);
                if (dicProvince != null) _provinceDic = dicProvince;
                return _provinceDic;
            }
        }

        public bool LoginBase(string username, string pwd)
        {
            AccountService accountService = new AccountService();
            Account model = accountService.GetSingleMember(username, pwd, out ErrorMsg);
            if (model != null)
            {
                HttpCookie cookie = new HttpCookie("accountInfo");
                cookie.Value = Base64Generate(JsonConvert.SerializeObject(model));
                cookie.Expires = DateTime.Now.AddHours(MAX_COOKIESTIME);
                if (Response != null)
                {
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 取得用户信息
        /// </summary>
        /// <returns></returns>
        public Account GetAccount()
        {
            string cookieStr = CookiesEvent.GetCookies(SetCurrentCookies, "accountInfo");
            if (!string.IsNullOrEmpty(cookieStr))
            {
                Account account = JsonConvert.DeserializeObject<Account>(GetBase64Generate(cookieStr));
                account.UserName = System.Web.HttpUtility.UrlDecode(account.UserName);
                return account;
            }

            return null;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult LogoutBase()
        {
            RemoveStoredData();
            return RedirectLogin();
        }

        /// <summary>
        /// 登录跳转
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectLogin()
        {
            return Redirect("~/manage/home/index");
        }
        public ActionResult RedirectError()
        {
            return new RedirectResult("/Account/Login");
            // return Redirect("/home/welcome");
        }
        /// <summary>
        /// 清除认证信息
        /// </summary>
        public void RemoveStoredData()
        {
            if (this.HttpContext != null)
            {
                CookiesEvent.ClearCookiesAll(this.HttpContext);
            }
            else
            {
                CookiesEvent.ClearCookies(System.Web.HttpContext.Current, "accountInfo");
            }
            CacheHelper.RemoveAll();
        }

        /// <summary>
        /// 返回认证cookie字符串
        /// </summary>
        /// <returns></returns>
        private string GetCookieStr()
        {

            return CookiesEvent.GetCookies(SetCurrentCookies, "accountInfo");
        }

        /// <summary>
        /// 64位加密方式
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string Base64Generate(string encryptStr)
        {
            return EncryptHelper.Encode(encryptStr + "|" + "ee7018a6AA5b53e50");
        }

        /// <summary>
        /// 获取加密密码
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private string GetBase64Generate(string decryptStr)
        {
            string result = EncryptHelper.Decode(decryptStr);

            if (!string.IsNullOrEmpty(result))
            {
                var tt = result.Split('|');
                if (tt[0] != null && tt[1] == "ee7018a6AA5b53e50")
                    return tt[0] ?? tt[0];
            }
            return string.Empty;
        }

        #region 虚方法

        public virtual JsonResult Delete(int id)
        {
            throw new ArgumentNullException("Calling methods requires overriding base classes");
        }

        public virtual JsonResult Publish(int id)
        {
            throw new ArgumentNullException("Calling methods requires overriding base classes");
        }

        public virtual JsonResult Shelves(int id)
        {
            throw new ArgumentNullException("Calling methods requires overriding base classes");
        }
        #endregion

        protected string GetThumb(string imgSrc)
        {
            if (string.IsNullOrEmpty(imgSrc)) { return string.Empty; }
            string fileName = Path.GetFileName(imgSrc);
            string thumbFileName = "small_" + fileName;
            return locAddr + imgSrc.Replace(fileName, thumbFileName);
        }

        protected string GetAreaStr(string province, string city)
        {
            if (string.IsNullOrEmpty(city)) return "";
            string proStr = Province.Where(g => g.Key == Convert.ToInt32(province)).FirstOrDefault().Value;
            string cityStr = City.Where(g => g.Key == Convert.ToInt32(city)).FirstOrDefault().Value;

            return proStr + "-" + cityStr;
        }

        protected bool IsExcel(string suffix)
        {
            bool result = false;
            List<string> suffixList = new List<string>() { ".xlsx", ".xls" };
            if (!string.IsNullOrEmpty(suffix))
                result = suffixList.Contains(suffix);
            return result;
        }

        protected string CellSwitch(ICell cell)
        {
            if (cell == null) return "";
            switch (cell.CellType)
            {
                case CellType.Numeric: return DateUtil.IsCellDateFormatted(cell) ? cell.DateCellValue.ToString("yyyy-M-d") : cell.NumericCellValue.ToString();
                case CellType.String: return cell.StringCellValue;
                case CellType.Boolean: return cell.BooleanCellValue.ToString();
                case CellType.Error: return cell.ErrorCellValue.ToString();
                case CellType.Formula: return cell.StringCellValue;
                default: return "";
            }
        }

        private void FillAgent()
        {
            var agent = new { UserAgent = Request.UserAgent, Platform = Request.Browser.Platform, Version = Request.Browser.Version, BrowserName = Request.Browser.Browser, BrowserType = Request.Browser.Type, IPAddress = Request.UserHostAddress };
            PlatformAgent.Set(JsonConvert.SerializeObject(agent));
        }

        protected void AddLog(string actionName, string resLog, string req = "", LogType logType = LogType.Error, LogSource logSourece = LogSource.Admin)
        {
            SystemLog sLog = new SystemLog();
            sLog.ActionName = actionName;
            sLog.ClientIp = IP;
            sLog.ControllerName = this.GetType().Name;
            sLog.ResultLog = resLog;
            sLog.Source = logSourece;
            sLog.Type = logType;
            sLog.UserAgent = UserAgent;
            sLog.UserId = UserId;
            sLog.ReqParameter = req;
            IntegratedService.AddLog(sLog);
        }

        protected JsonResult EmptyJsonResult()
        {
            return Json(new
            {
                recordsFiltered = 0,
                recordsTotal = 0,
                data = new List<object>()
            }, JsonRequestBehavior.AllowGet);
        }

        protected void ExceptionTest()
        {
            string a = null;
            a.ToString();
        }
    }
}