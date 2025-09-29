using System.Web.Mvc;
using System.Web.Routing;

namespace GroomMate
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // The Database.SetInitializer line is no longer needed
        }
    }
}
