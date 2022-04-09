using HST.Art.Core;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HST.Art.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JSBundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            #region MyRegion
            //Exception lastError = Server.GetLastError(); //获取最近的异常
            //if (lastError != null)
            //{
            //    HttpException httpException = lastError as HttpException;
            //    if (new HttpRequestWrapper(Request).IsAjaxRequest())
            //    {
            //        //ajax请求
            //    }
            //    else
            //    {
            //        //页面请求
            //        Response.ClearContent();
            //        RouteData routeData = new RouteData();
            //        routeData.Values.Add("controller", "Exception");
            //        if (httpException != null)
            //        {
            //            switch ((ErrorCode)httpException.GetHttpCode())
            //            {
            //                case ErrorCode.ErrorReqrest: routeData.Values.Add("action", "HttpError400"); break;
            //                case ErrorCode.NoFile: routeData.Values.Add("action", "HttpError404"); break;
            //                case ErrorCode.ServerError: routeData.Values.Add("action", "HttpError500"); break;
            //                case ErrorCode.Unauthorized: routeData.Values.Add("action", "HttpError401"); break;
            //                case ErrorCode.ParameterNull: routeData.Values.Add("action", "HttpError501"); break;
            //            }
            //        }
            //        else
            //        {
            //            routeData.Values.Add("action", "HttpError404");
            //        }
            //        routeData.Values.Add("error", lastError.Message);
            //        IController errorController = null;// new ExceptionController();
            //        errorController.Execute(new RequestContext(
            //                   new HttpContextWrapper(Context), routeData));

            //        Server.ClearError();//清空异常避免继续处理。
            //    }
            //}
            #endregion

        }
    }
}
