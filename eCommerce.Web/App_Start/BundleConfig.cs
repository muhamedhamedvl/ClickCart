using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace eCommerce.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region Public Portal Bundles

            //Public Portal CSS Bundles
            bundles.Add(new StyleBundle("~/bundles/content/css").NonOrdering().Include(
                      "~/content/lib/bootstrap-4.4.1/css/bootstrap.min.css",
                      "~/content/templates/fashi/css/owl.carousel.min.css",
                      "~/content/templates/fashi/css/nice-select.css",
                      "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.css",
                      "~/content/templates/fashi/css/slicknav.min.css",
                      "~/content/templates/fashi/css/style.css",
                      "~/content/css/site.css"));
            
            //Public Portal JavaScript/jQuery for Header
            bundles.Add(new ScriptBundle("~/bundles/content/hscripts").NonOrdering().Include(
                        "~/content/lib/jquery-3.4.1/jquery.min.js",
                        "~/content/lib/sweetalert2-9.10.7/sweetalert2.all.min.js",
                        "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.js",
                        "~/content/lib/jquery-validation-1.19.1/jquery.validate.min.js",
                        "~/content/lib/jquery.cookie-1.4.1/jquery.cookie.js",
                        "~/Content/lib/jquery.lazy-1.7.10/jquery.lazy.min.js",
                        "~/Content/lib/darkreader-4.9.16/darkreader.min.js",
                        "~/content/js/site.js"));

            //Public Portal JavaScript/jQuery for Footer
            bundles.Add(new ScriptBundle("~/bundles/content/fscripts").NonOrdering().Include(
                        "~/content/lib/popperjs-1.16.0/popper.js",
                        "~/content/lib/bootstrap-4.4.1/js/bootstrap.min.js",
                        "~/content/templates/fashi/js/jquery.nice-select.min.js",
                        "~/content/templates/fashi/js/jquery.zoom.min.js",
                        "~/content/lib/ms-dropdown-3.5.2/jquery.dd.min.js",
                        "~/content/templates/fashi/js/jquery.slicknav.js",
                        "~/content/templates/fashi/js/owl.carousel.min.js",
                        "~/content/templates/fashi/js/main.js"));

            //Public Portal CSS Bundles for RTL
            bundles.Add(new StyleBundle("~/bundles/content/rtl/css").NonOrdering().Include(
                      "~/content/lib/bootstrap-rtl-4.2.1/css/rtl/bootstrap.min.css",
                      "~/content/templates/fashi/css/owl.carousel.min.css",
                      "~/content/templates/fashi/css/nice-select.css",
                      "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.css",
                      "~/content/templates/fashi/css/slicknav.min.css",
                      "~/content/templates/fashi/css/style.rtl.css",
                      "~/content/css/site.css"));

            //Public Portal JavaScript/jQuery for Header for RTL
            bundles.Add(new ScriptBundle("~/bundles/content/rtl/hscripts").NonOrdering().Include(
                        "~/content/lib/jquery-3.4.1/jquery.min.js",
                        "~/content/lib/sweetalert2-9.10.7/sweetalert2.all.min.js",
                        "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.js",
                        "~/content/lib/jquery-validation-1.19.1/jquery.validate.min.js",
                        "~/content/lib/jquery.cookie-1.4.1/jquery.cookie.js",
                        "~/Content/lib/jquery.lazy-1.7.10/jquery.lazy.min.js",
                        "~/Content/lib/darkreader-4.9.16/darkreader.min.js",
                        "~/content/js/site.js"));

            //Public Portal JavaScript/jQuery for Footer for RTL
            bundles.Add(new ScriptBundle("~/bundles/content/rtl/fscripts").NonOrdering().Include(
                        "~/content/lib/popperjs-1.16.0/popper.js",
                        "~/content/lib/bootstrap-rtl-4.2.1/js/bootstrap.min.js",
                        "~/content/templates/fashi/js/jquery.nice-select.min.js",
                        "~/content/templates/fashi/js/jquery.zoom.min.js",
                        "~/content/lib/ms-dropdown-3.5.2/jquery.dd.min.js",
                        "~/content/templates/fashi/js/jquery.slicknav.js",
                        "~/content/templates/fashi/js/owl.carousel.min.js",
                        "~/content/templates/fashi/js/main.js"));

            #endregion

            #region Dashboard Bundles
            //Dashboard CSS Bundles
            bundles.Add(new StyleBundle("~/bundles/dashboard/content/css").NonOrdering().Include(
                      "~/content/templates/sbadmin2/css/sb-admin-2.min.css", //this also has Bootstrap in it
                      "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.css",
                      "~/content/css/dashboard.css"));

            //Dashboard CSS Bundles for RTL
            bundles.Add(new StyleBundle("~/bundles/dashboard/content/rtl/css").NonOrdering().Include(
                      "~/content/templates/sbadmin2/css/sb-admin-2.rtl.min.css", //this also has Bootstrap in it
                      "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.css",
                      "~/content/css/dashboard.css"));

            //JavaScript/jQuery for Header
            bundles.Add(new ScriptBundle("~/bundles/dashboard/content/scripts").NonOrdering().Include(
                        "~/content/lib/jquery-3.4.1/jquery.min.js",
                        "~/content/lib/sweetalert2-9.10.7/sweetalert2.all.min.js",
                        "~/content/lib/jquery-ui-1.12.1/jquery-ui.min.js",
                        "~/content/lib/jquery-validation-1.19.1/jquery.validate.min.js",
                        "~/content/lib/jquery.cookie-1.4.1/jquery.cookie.js",
                        "~/content/lib/moment.js-2.24.0/moment.min.js",
                        "~/content/lib/popperjs-1.16.0/popper.js",
                        "~/content/lib/bootstrap-4.4.1/js/bootstrap.min.js",
                        "~/content/lib/ms-dropdown-3.5.2/jquery.dd.min.js",
                        "~/Content/lib/darkreader-4.9.16/darkreader.min.js",
                        "~/content/js/dashboard.js"));
            #endregion
        }
    }

    class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }
    
    static class BundleExtentions
    {
        public static Bundle NonOrdering(this Bundle bundle)
        {
            bundle.Orderer = new NonOrderingBundleOrderer();
            return bundle;
        }
    }
}
