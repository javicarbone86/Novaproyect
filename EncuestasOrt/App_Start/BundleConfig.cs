using System.Web;
using System.Web.Optimization;

namespace EncuestasOrt
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las EncuestasOrt que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            bundles.Add(new StyleBundle("~/Content/chosen.css").Include(
                      "~/Content/chosen.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/chosen.js").Include(
                      "~/Scripts/chosen.jquery.js"));


            bundles.Add(new ScriptBundle("~/bundles/Chart").Include(
                      "~/Scripts/Chart.js"));

            bundles.Add(new ScriptBundle("~/bundles/Grid").Include(
                        "~/ScriptsGrid/grid-0.4.3.js"));


            // Para la depuración, establezca EnableOptimizations en false. Para obtener más información,
            // visite http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
