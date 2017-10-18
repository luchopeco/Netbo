using System.Web;
using System.Web.Optimization;

namespace WebComunidad
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                     "~/Content/bootstrap.css",
                     "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Content/datatable").Include(
                "~/Content/datatable/css/jquery.dataTables.css"));

            bundles.Add(new StyleBundle("~/Content/themecss").Include(
                      "~/Content/Theme/vendors/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/Theme/vendors/font-awesome-4.7.0/css/font-awesome.min.css",
                      //"~/Content/Theme/vendors/nprogress/nprogress.css",
                      // "~/Content/Theme/vendors/iCheck/skins/flat/green.css",
                      //"~/Content/Theme/vendors/bootstrap-progressbar/css/bootstrap-progressbar-3.3.4.min.css",
                      "~/Content/Theme/vendors/jqvmap/dist/jqvmap.min.css",
                      "~/Content/Theme/build/css/custom.min.css",
                      "~/Content/jquery.dataTables.min.css",
                       "~/Content/Theme/vendors/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css",
                        "~/Content/css-propia.css",
                      "~/Content/themes/base/jquery-ui.css"
                      ));

            bundles.Add(new ScriptBundle("~/Content/themejs").Include(
                   "~/Scripts/jquery-1.12.4.js",
                   "~/Scripts/jquery-ui-1.12.1.js",
                   "~/Content/Theme/vendors/bootstrap/dist/js/bootstrap.min.js",
                   //"~/Content/Theme/vendors/fastclick/lib/fastclick.js",
                   //"~/Content/Theme/vendors/nprogress/nprogress.js",
                   //"~/Content/Theme/vendors/Chart.js/dist/Chart.min.js",
                   //"~/Content/Theme/vendors/gauge.js/dist/gauge.min.js",
                   //"~/Content/Theme/vendors/bootstrap-progressbar/bootstrap-progressbar.min.js",
                   "~/Content/Theme/vendors/iCheck/icheck.min.js",
                   "~/Content/Theme/vendors/skycons/skycons.js",
                   //"~/Content/Theme/vendors/flot.orderbars/js/jquery.flot.orderBars.js",
                   //"~/Content/Theme/vendors/flot-spline/js/jquery.flot.spline.min.js",
                   //"~/Content/Theme/vendors/flot.curvedlines/curvedLines.js",
                   "~/Content/Theme/vendors/DateJS/build/date.js",
                   //"~/Content/Theme/vendors/jqvmap/dist/jquery.vmap.js",
                   //"~/Content/Theme/vendors/jqvmap/dist/maps/jquery.vmap.world.js",
                   //"~/Content/Theme/vendors/jqvmap/examples/js/jquery.vmap.sampledata.js",
                   //"~/Content/Theme/js/moment/moment.min.js",
                   "~/Content/Theme/js/datepicker/daterangepicker.js",
                   "~/Content/Theme/build/js/custom.min.js",
                   "~/Content/Theme/js/jquery.dataTables.min.js",
                   "~/Content/Theme/vendors/jquery.inputmask/dist/min/jquery.inputmask.bundle.min.js",
                   "~/Content/Theme/vendors/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.concat.min.js",
                     "~/Content/Chart.js"
                    ));
            bundles.Add(new ScriptBundle("~/Content/datatablejs").Include(
                 "~/Content/datatable/js/jquery.dataTables.min.js"
                  ));
            BundleTable.EnableOptimizations = true;
        }
    }
}
