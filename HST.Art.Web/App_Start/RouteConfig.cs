using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ZT.SMS.Web.Areas.manage.Controllers;

namespace ZT.SMS.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapLowerCaseUrlRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    new string [] { "1"}
            //);

            //默认路由
            routes.MapLowerCaseUrlRoute(
            "Default",
            "{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             , new string[] { typeof(HomeController).Namespace }
            ).DataTokens.Add("area", "manage");//HomeController  ZT.SMS.Web.Areas.manage.Controllers
        }
    }
}
