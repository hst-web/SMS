using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HST.Art.Web
{
    public class CookiesEvent
    {
        #region Cookies
        /// <summary>
        /// 清除指定cookie
        /// </summary>
        /// <param name="contenxt">The contenxt.</param>
        /// <param name="cookies">The cookies.</param>
        public static void ClearCookies(HttpContext contenxt, params string[] cookies)
        {
            //清除cookies
            foreach (string key in cookies)
            {
                contenxt.Response.Cookies[key].Expires = DateTime.Now.AddDays(-1);
            }
        }
        /// <summary>
        /// 清空所有cookie
        /// </summary>
        /// <param name="contenxt">The contenxt.</param>
        public static void ClearCookiesAll(HttpContextBase contenxt)
        {
            //清除cookies
            for (int i = 0; i < contenxt.Request.Cookies.Count; i++)
            {
                contenxt.Response.Cookies[contenxt.Request.Cookies[i].Name].Expires = DateTime.Now.AddDays(-1);
            }
        }
        /// <summary>
        /// 取得cookies集合
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookies(HttpCookieCollection collection, string cookieName)
        {
            if (collection != null && !string.IsNullOrEmpty(cookieName))
            {
                return collection[cookieName]==null?"": collection[cookieName].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 取得cookies集合中的一个cookie值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="collectionName"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookies(HttpCookieCollection collection, string collectionName, string cookieName)
        {
            if (collection != null && !string.IsNullOrEmpty(collectionName) && !string.IsNullOrEmpty(cookieName))
            {
                if (collection[collectionName] != null)
                {
                    return collection[collectionName].Values[cookieName];
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 取得cookies中一个值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(HttpCookieCollection collection, string cookieName)
        {
            if (collection != null && !string.IsNullOrEmpty(cookieName))
            {
                if (collection[cookieName] != null)
                {
                    return collection[cookieName].Value;
                }
            }
            return string.Empty;
        }
        #endregion
    }
}
