using GroomMate.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    [Authorize(Roles = "Staff")] // Ensures only users with the "Staff" role can access actions in this controller
    public class StaffController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // POST: Staff/CompleteAppointment/5
        [HttpPost]
        [ValidateAntiForgeryToken] // Security feature to prevent CSRF attacks
        public ActionResult CompleteAppointment(int id)
        {
            // Find the appointment in the database
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                // If the appointment doesn't exist, return a 404 error
                return HttpNotFound();
            }

            // Ensure the logged-in staff member is the one assigned to this appointment
            int currentStaffId = (int)Session["UserID"];
            if (appointment.StaffId != currentStaffId)
            {
                // If not, it's an unauthorized action. Return a "Bad Request" error.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Update the status of the appointment
            appointment.Status = "Completed";
            db.SaveChanges();

            // Redirect the user back to their dashboard to see the updated list
            return RedirectToAction("StaffDashboard", "Dashboard");
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
