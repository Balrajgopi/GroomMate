using GroomMate.Models;
using System.Collections.Generic;
using System.Data.Entity;
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

            // Load feedbacks grouped by ServiceID for public display
            var feedbacks = db.Feedbacks
                .Include(f => f.Appointment.User)
                .Include(f => f.Appointment.Service)
                .ToList();

            // Group feedbacks by ServiceID
            var feedbackByService = feedbacks
                .Where(f => f.Appointment != null)
                .GroupBy(f => f.Appointment.ServiceID)
                .ToDictionary(g => g.Key, g => g.ToList());

            ViewBag.FeedbackByService = feedbackByService;

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
