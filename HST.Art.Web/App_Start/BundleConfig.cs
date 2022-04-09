using System.Web;
using System.Web.Optimization;
namespace HST.Art.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/appcss").Include(
                    "~/Content/lib/h-ui/css/H-ui.min.css", new CssRewriteUrlTransformWrapper()).Include(
                    "~/Content/lib/h-ui.admin/css/H-ui.admin.css", new CssRewriteUrlTransformWrapper()).Include(
                    "~/Content/lib/Hui-iconfont/1.0.8/iconfont.min.css", new CssRewriteUrlTransformWrapper()).Include(
                    "~/Content/lib/h-ui.admin/skin/default/skin.css", new CssRewriteUrlTransformWrapper()).Include(
                     "~/Content/lib/webuploader/0.1.5/webuploader.css", new CssRewriteUrlTransformWrapper()).Include(
                     "~/Content/lib/uniform/css/uniform.default.min.css", new CssRewriteUrlTransformWrapper()).Include(
                     "~/Content/lib/fancybox/dist/jquery.fancybox.min.css", new CssRewriteUrlTransformWrapper()).Include(
                     "~/Content/lib/h-ui.admin/css/style.css"
                ));

            bundles.Add(new StyleBundle("~/bootstrap").Include(
               "~/Content/lib/Bootstrap/css/bootstrap.min.css", new CssRewriteUrlTransformWrapper()).Include(
               "~/Content/lib/bootstrap-select/bootstrap-select.min.css", new CssRewriteUrlTransformWrapper()));
            bundles.Add(new StyleBundle("~/login").Include("~/Content/lib/h-ui.admin/css/base.css", new CssRewriteUrlTransformWrapper()).Include("~/Content/lib/h-ui.admin/css/reg.css", new CssRewriteUrlTransformWrapper()));
            bundles.Add(new StyleBundle("~/patternfly").Include(
                 "~/Content/lib/patternfly/css/patternfly.min.css", new CssRewriteUrlTransformWrapper()).Include(
                  "~/Content/lib/patternfly/css/patternfly-additions.min.css", new CssRewriteUrlTransformWrapper()));
            bundles.Add(new StyleBundle("~/webclient").Include(
                 "~/Content/css/css.css", new CssRewriteUrlTransformWrapper()).Include(
                   "~/Content/css/style.css", new CssRewriteUrlTransformWrapper()).Include(
                  "~/Content/css/response.css", new CssRewriteUrlTransformWrapper()));

            bundles.Add(new StyleBundle("~/swiper").Include(
                 "~/Content/css/swiper.min.css", new CssRewriteUrlTransformWrapper()).Include(
                "~/Content/css/owl.carousel.min.css", new CssRewriteUrlTransformWrapper()));
        }
    }

    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);

        }
    }
}