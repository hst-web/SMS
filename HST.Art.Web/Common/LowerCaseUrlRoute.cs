/*----------------------------------------------------------------
// 文件名：LowerCaseUrlRoute .cs
// 功能描述： Url小写路由
// 创建者：sysmenu
// 创建时间：2019-5-6
//----------------------------------------------------------------*/
using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace HST.Art.Web
{
    /// <summary>
    /// Url小写路由
    /// </summary>
    public class LowerCaseUrlRoute : Route
    {
        /// <summary>
        /// 区域、控制器、活动
        /// </summary>
        private static readonly string[] requiredKeys = new[] { "area", "controller", "action" };

        /// <summary>
        /// 路由数据转小写
        /// </summary>
        /// <param name="values"></param>
        private void LowerRouteValues(RouteValueDictionary values)
        {
            foreach (var key in requiredKeys)
            {
                if (values.ContainsKey(key) == false) continue;

                var value = values[key];
                if (value == null) continue;

                var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
                if (valueString == null) continue;

                values[key] = valueString.ToLower();
            }
            
                var otherKyes = values.Keys
                .Except(requiredKeys, StringComparer.InvariantCultureIgnoreCase)
                .ToArray();

            foreach (var key in otherKyes)
            {
                var value = values[key];
                try
                {
                    values.Remove(key);
                    values.Add(key, value);
                }
                catch { }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">url中path部分</param>
        /// <param name="routeHandler">路由处理</param>
        public LowerCaseUrlRoute(string url, IRouteHandler routeHandler)
        : base(url, routeHandler)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="url">url中path部分</param>
        /// <param name="defaults">默认路由</param>
        /// <param name="routeHandler">路由处理</param>
        public LowerCaseUrlRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
        : base(url, defaults, routeHandler)
        { }

        /// <summary>
        /// 生成URL
        /// </summary>
        /// <param name="requestContext">请求上限文</param>
        /// <param name="values">路由键值对</param>
        /// <returns></returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            //LowerRouteValues(requestContext.RouteData.Values);
            LowerRouteValues(values);
            //LowerRouteValues(Defaults);
            return base.GetVirtualPath(requestContext, values);
        }
    }

    /// <summary>
    /// Url小写路由映射助手
    /// </summary>
    public static class LowerCaseUrlRouteMapHelper
    {
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url)
        {
            return routes.MapLowerCaseUrlRoute(name, url, null, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return routes.MapLowerCaseUrlRoute(name, url, defaults, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return routes.MapLowerCaseUrlRoute(name, url, null, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return routes.MapLowerCaseUrlRoute(name, url, defaults, constraints, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return routes.MapLowerCaseUrlRoute(name, url, defaults, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null) throw new ArgumentNullException("routes");
            if (url == null) throw new ArgumentNullException("url");
            LowerCaseUrlRoute route2 = new LowerCaseUrlRoute(url, new MvcRouteHandler());
            route2.Defaults = new RouteValueDictionary(defaults);
            route2.Constraints = new RouteValueDictionary(constraints);
            route2.DataTokens = new RouteValueDictionary();
            LowerCaseUrlRoute item = route2;
            
            if ((namespaces != null) && (namespaces.Length > 0))
                item.DataTokens["Namespaces"] = namespaces;
            routes.Add(name, item);
            return item;
        }

        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url)
        {
            return context.MapLowerCaseUrlRoute(name, url, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults)
        {
            return context.MapLowerCaseUrlRoute(name, url, defaults, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, string[] namespaces)
        {
            return context.MapLowerCaseUrlRoute(name, url, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints)
        {
            return context.MapLowerCaseUrlRoute(name, url, defaults, constraints, null);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, string[] namespaces)
        {
            return context.MapLowerCaseUrlRoute(name, url, defaults, null, namespaces);
        }
        public static LowerCaseUrlRoute MapLowerCaseUrlRoute(this AreaRegistrationContext context, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if ((namespaces == null) && (context.Namespaces != null))
                namespaces = context.Namespaces.ToArray<string>();
            LowerCaseUrlRoute route = context.Routes.MapLowerCaseUrlRoute(name, url, defaults, constraints, namespaces);
            route.DataTokens["area"] = context.AreaName;
            bool flag = (namespaces == null) || (namespaces.Length == 0);
            route.DataTokens["UseNamespaceFallback"] = flag;
            return route;
        }
    }
}
