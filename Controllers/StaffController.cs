using GroomMate.Models;
using System;
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
            int? currentStaffId = GroomMate.Security.AuthHelper.RestoreUserSession(HttpContext, db);
            if (!currentStaffId.HasValue)
            {
                TempData["ErrorMessage"] = "Your session has expired. Please log in again.";
                return RedirectToAction("Login", "Account");
            }
            if (appointment.StaffId != currentStaffId.Value)
            {
                // If not, it's an unauthorized action. Return a "Bad Request" error.
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "You are not assigned to this appointment.");
            }

            // Only confirmed appointments can be marked as completed
            if (!string.Equals(appointment.Status, "Confirmed", System.StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Only confirmed appointments can be marked as completed.";
                return RedirectToAction("StaffDashboard", "Dashboard");
            }

            // Prevent completing future appointments based on India Standard Time (IST)
            DateTime currentIst = GroomMate.Security.TimeZoneHelper.GetCurrentIst();
            if (appointment.AppointmentDate > currentIst)
            {
                TempData["ErrorMessage"] = "Cannot complete an appointment that is scheduled in the future.";
                return RedirectToAction("StaffDashboard", "Dashboard");
            }

            // Update the status of the appointment
            appointment.Status = "Completed";
            db.SaveChanges();

            // Send email notification to customer about completion
            GroomMate.Security.EmailService.SendAppointmentNotification(appointment.AppointmentID, "Completed");

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
