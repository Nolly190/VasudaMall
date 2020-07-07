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
                        "~/vasuda-template/css/simple.css",
                        "~/Content/spinner.css"
                ));

            bundles.Add(new ScriptBundle("~/vasuda-template/script").Include(
                    "~/vasuda-template/js/jquery-2.0.3.min.js",
                    "~/vasuda-template/js/jquery-3.3.1.min.js",
                    "~/vasuda-template/js/bootstrap.min.js",
                    "~/vasuda-template/js/bigSlide.js",
                    "~/vasuda-template/js/index.js",
                    "~/vasuda-template/js/jquery.easing.1.3.js",
                    "~/vasuda-template/js/jquery.skitter.js",
                    "~/vasuda-template/js/jquery.skitter.min.js",
                    "~/Scripts/products.js",
                    "~/vasuda-template/js/owl.carousel.js"
                    ));

            //Admin lte styles and scripts...
            bundles.Add(new ScriptBundle("~/admin-lte/script").Include(
                // "~/Scripts/jquery-1.10.2.min.js",
                //"~/Scripts/jquery-3.4.1.min.js",
                "~/admin-lte/plugins/jquery/jquery.min.js",
                "~/admin-lte/plugins/jquery-ui/jquery-ui.min.js",
                "~/admin-lte/dist/js/adminlte.js",
                "~/admin-lte/dist/js/pages/dashboard.js",
                "~/admin-lte/plugins/bootstrap/js/bootstrap.bundle.min.js",
                "~/admin-lte/plugins/jqvmap/jquery.vmap.min.js",
                "~/admin-lte/plugins/jqvmap/maps/jquery.vmap.usa.js",
                "~/admin-lte/plugins/jquery-knob/jquery.knob.min.js",
                "~/admin-lte/plugins/moment/moment.min.js",
                "~/admin-lte/plugins/daterangepicker/daterangepicker.js",
                "~/admin-lte/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                "~/admin-lte/plugins/summernote/summernote-bs4.min.js",
                "~/admin-lte/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                "~/admin-lte/plugins/datatables/jquery.dataTables.js",
                "~/admin-lte/plugins/datatables-bs4/js/dataTables.bootstrap4.js",
                "~/admin-lte/plugins/bootstrap/js/bootstrap.bundle.min.js"
                //"~/Scripts/jquery.validate.min.js",
                //"~/Scripts/jquery.validate.unobtrusive.min.js"
                ));

            bundles.Add(new StyleBundle("~/admin-lte/css").Include(
                "~/admin-lte/dist/css/adminlte.min.css",
                "~/admin-lte/plugins/fontawesome-free/css/all.min.css",
                "~/admin-lte/plugins/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                "~/admin-lte/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                "~/admin-lte/plugins/jqvmap/jqvmap.min.css",
                "~/admin-lte/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                "~/admin-lte/plugins/daterangepicker/daterangepicker.css",
                "~/admin-lte/plugins/summernote/summernote-bs4.css"
                //"~/admin-lte/plugins/datatables-bs4/css/dataTables.bootstrap4.css",
                //"~/admin-lte/plugins/select2/css/select2.min.css",
                //"~/admin-lte/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css"
                /*"~/Content/MyCss.css"*/));
        }
    }
}
