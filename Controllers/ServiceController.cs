using GroomMate.Models;
using System.Linq;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    // NO [Authorize] attribute at the class level.
    public class ServiceController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // This action is now public, allowing anyone to see the services.
        public ActionResult Index()
        {
            var services = db.Services.Where(s => s.IsActive).ToList();
            return View(services);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
