using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace HST.Art.Web
{
    internal class AsIsBundleOrderer : IBundleOrderer
    {
        public virtual IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
    public class JSBundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            ScriptBundle mandatoryJs = new ScriptBundle("~/bundles/saos");
            mandatoryJs.Orderer = new AsIsBundleOrderer();
            mandatoryJs.Include(
                 "~/Content/lib/jquery/jquery.min.js",
                 "~/Content/lib/jquery/jquery-migrate.min.js",
                  "~/Content/lib/jquery-ui/jquery-ui.min.js",
                 "~/Content/lib/uniform/jquery.uniform.js").Include(
                 "~/Content/lib/jquery-animateNumber/jquery.animateNumber.min.js", new CssRewriteUrlTransformWrapper()).Include(
                 "~/Content/lib/fancybox/dist/jquery.fancybox.min.js", new CssRewriteUrlTransformWrapper()).Include(
                 "~/Content/lib/layer/2.4/layer.js", new CssRewriteUrlTransformWrapper()).Include(
                 "~/Content/lib/h-ui/js/H-ui.min.js", new CssRewriteUrlTransformWrapper()).Include(
                 "~/Content/lib/h-ui.admin/js/H-ui.admin.js", new CssRewriteUrlTransformWrapper()).Include(
                  "~/Content/lib/laypage/1.2/laypage.js", new CssRewriteUrlTransformWrapper()).Include(
                   "~/Content/lib/My97DatePicker/4.8/WdatePicker.js", new CssRewriteUrlTransformWrapper()).Include(
                  "~/Content/lib/vue/vue.min.js",
                   "~/Content/lib/vue/vue-resource.min.js",
                    "~/Content/lib/h-ui.admin/js/custom.js"
                 );

            bundles.Add(mandatoryJs);

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Content/lib/datatables/1.10.0/jquery.dataTables.min.js", new CssRewriteUrlTransformWrapper()));

            bundles.Add(new ScriptBundle("~/bundles/validate").Include(

            "~/Content/lib/jquery.validation/1.14.0/jquery.validate.js",
           "~/Content/lib/jquery.validation/1.14.0/validate-methods.js",
            "~/Content/lib/jquery.validation/1.14.0/messages_zh.js",
            "~/Content/js/jquery.validate.unobtrusive.min.js"
            ));
            bundles.Add(new ScriptBundle("~/bundles/ajaxAsync").Include(
         "~/Content/js/jquery.unobtrusive-ajax.min.js"
          ));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        "~/Content/lib/Bootstrap/bootstrap.min.js",
          "~/Content/lib/bootstrap-select/bootstrap-select.min.js",
        "~/Content/lib/jquery.matchHeight/jquery.matchHeight.js",
        "~/Content/lib/patternfly/js/patternfly.min.js"
         ));
            bundles.Add(new ScriptBundle("~/bundles/D-C3").Include(
               "~/Content/lib/C-D3/c3.min.js",
               "~/Content/lib/C-D3/d3.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/webclient").Include(
                "~/Content/js/jQuery.1.9.1.js",
             "~/Content/js/swiper.min.js",
             "~/Content/js/owl.carousel.js",  
               "~/Content/js/jquery.blockui.min.js",
               "~/Content/lib/h-ui.admin/js/custom.js"
              ));
        }
    }
}