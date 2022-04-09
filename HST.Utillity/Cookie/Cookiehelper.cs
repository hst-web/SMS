/*----------------------------------------------------------------

// 文件名：Cookiehelper.cs
// 功能描述：客户端缓存工具类
// 创建者：sysmenu
// 创建时间：2019-3-18
//----------------------------------------------------------------*/
using System;
using System.Web;

namespace HST.Utillity
{
    /// <summary>
    /// Cookie帮助类
    /// </summary>
    public class Cookiehelper
    {
        /// <summary>
        /// 取Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static HttpCookie Get(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        /// <summary>
        /// 取Cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetValue(string name)
        {
            var httpCookie = Get(name);
            if (httpCookie != null)
                return httpCookie.Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="name"></param>
        public static void Remove(string name)
        {
            Remove(Get(name));
        }

        //删除cookie
        public static void Delete(string name)
        {
            HttpContext.Current.Response.Cookies.Remove(name);
        }

        public static void Remove(HttpCookie cookie)
        {
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now;
                Save(cookie);
            }
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expiresHours"></param>
        public static void Save(string name, string value, int expiresHours = 0)
        {
            var httpCookie = Get(name);
            if (httpCookie == null)
                httpCookie = Set(name);

            httpCookie.Value = value;
            Save(httpCookie, expiresHours);
        }

        /// <summary>
        /// 保存cookie
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <param name="expiresHours">时间（小时）</param>
        public static void Save(HttpCookie cookie, int expiresHours = 0)
        {
            string domain = FetchHelper.ServerDomain;
            string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
            if (domain != urlHost)
                cookie.Domain = domain;

            if (expiresHours > 0)
                cookie.Expires = DateTime.Now.AddHours(expiresHours);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expiresHours"></param>
        public static void SaveForMin(string name, string value, int expiresMin = 0)
        {
            var httpCookie = Get(name);
            if (httpCookie == null)
                httpCookie = Set(name);

            httpCookie.Value = value;
            SaveForMin(httpCookie, expiresMin);
        }

        /// <summary>
        /// 保存cookie
        /// </summary>
        /// <param name="cookie">cookie</param>
        /// <param name="expiresHours">时间（分钟）</param>
        public static void SaveForMin(HttpCookie cookie, int expiresMin = 0)
        {
            string domain = FetchHelper.ServerDomain;
            string urlHost = HttpContext.Current.Request.Url.Host.ToLower();
            if (domain != urlHost)
                cookie.Domain = domain;

            if (expiresMin > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expiresMin);

            HttpContext.Current.Response.Cookies.Add(cookie);
        }


        public static HttpCookie Set(string name)
        {
            return new HttpCookie(name);
        }
    }
}
