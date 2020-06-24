using System.Web;
using System.Web.Optimization;

namespace VasudaMall
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //Vasuda template bundles...
            bundles.Add(new StyleBundle("~/vasuda-template/css").Include(
                        "~/vasuda-template/css/bootstrap.min.css",
                        "~/vasuda-template/css/bootstrap-grid.min.css",
                        "~/vasuda-template/css/index.css",
                        "~/vasuda-template/css/animate.css",
                        "~/vasuda-template/css/animate.min.css",
                        "~/vasuda-template/css/animated-slider.css",
                        "~/vasuda-template/css/owl.carousel.min.css",
                        "~/vasuda-template/css/owl.theme.default.min.css",
                        "~/vasuda-template/css/simple.css"
                ));

            bundles.Add(new ScriptBundle("~/vasuda-template/styles").Include(
                    "~/vasuda-template/js/jquery-3.3.1.min.js",
                    "~/vasuda-template/js/jquery-2.0.3.min.js",
                    "~/vasuda-template/js/bootstrap.min.js",
                    "~/vasuda-template/js/bigSlide.js",
                    "~/vasuda-template/js/index.js",
                    "~/vasuda-template/js/jquery.easing.1.3.js",
                    "~/vasuda-template/js/jquery.skitter.js",
                    "~/vasuda-template/js/jquery.skitter.min.js",
                    "~/vasuda-template/js/owl.carousel.js"
                    ));
        }
    }
}
