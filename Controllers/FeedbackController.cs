using GroomMate.Models;
using System.Linq;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        // BEST PRACTICE: Mark the DbContext as readonly.
        private readonly GroomMateContext db = new GroomMateContext();

        public ActionResult Submit(int appointmentId)
        {
            int userId = (int)Session["UserID"];
            var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentID == appointmentId && a.UserID == userId && a.Status == "Completed");
            var hasFeedback = db.Feedbacks.Any(f => f.AppointmentID == appointmentId);

            if (appointment == null || hasFeedback)
            {
                return RedirectToAction("AlreadySubmitted");
            }
            ViewBag.AppointmentId = appointmentId;
            return View();
        }

        [HttpPost]
        public ActionResult Submit(int appointmentId, int rating, string comments)
        {
            int userId = (int)Session["UserID"];
            var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentID == appointmentId && a.UserID == userId);

            if (appointment == null) return HttpNotFound();

            var feedback = new Feedback { AppointmentID = appointmentId, Rating = rating, Comments = comments };
            db.Feedbacks.Add(feedback);
            db.SaveChanges();

            return RedirectToAction("ThankYou");
        }

        public ActionResult ThankYou() => View();
        public ActionResult AlreadySubmitted() => View();

        // BEST PRACTICE: Dispose the DbContext to release database connections.
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
