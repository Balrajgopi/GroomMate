using System.Data.Entity; // Add this using statement
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GroomMate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // *** THIS IS THE SECOND CRITICAL FIX ***
            // This line tells Entity Framework to COMPLETELY DISABLE all automatic database
            // initializers. This is the best practice when using Code-First Migrations,
            // as it gives you full control and prevents EF from trying to change the
            // database behind your back.
            Database.SetInitializer<Models.GroomMateContext>(null);
        }
    }
}
