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
            int? currentUserId = GroomMate.Security.AuthHelper.RestoreUserSession(HttpContext, db);
            if (!currentUserId.HasValue) return RedirectToAction("Login", "Account");
            int userId = currentUserId.Value;
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
        [ValidateAntiForgeryToken]
        public ActionResult Submit(int appointmentId, int rating, string comments)
        {
            int? currentUserId = GroomMate.Security.AuthHelper.RestoreUserSession(HttpContext, db);
            if (!currentUserId.HasValue) return RedirectToAction("Login", "Account");
            int userId = currentUserId.Value;
            var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentID == appointmentId && a.UserID == userId && a.Status == "Completed");

            if (appointment == null) return HttpNotFound();

            var hasFeedback = db.Feedbacks.Any(f => f.AppointmentID == appointmentId);
            if (hasFeedback)
            {
                return RedirectToAction("AlreadySubmitted");
            }

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
