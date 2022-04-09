using System.Web.Mvc;

namespace HST.Art.Web.Areas.manage
{
    public class manageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowerCaseUrlRoute(
                "manage_default",
                "manage/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, new string[] { "HST.Art.Web.Areas.manage.Controllers" }
            );
        }
    }
}