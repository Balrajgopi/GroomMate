using GroomMate.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    [Authorize(Roles = "Admin")] // Secures the entire controller for Admin access only
    public class AdminController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // POST: Admin/AssignStaff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignStaff(int appointmentId, int staffId)
        {
            // Find the appointment to be updated
            var appointment = db.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            // Find the staff member to assign
            var staff = db.Users.FirstOrDefault(u => u.UserID == staffId && u.Role.RoleName == "Staff");
            if (staff == null)
            {
                // If a non-staff user is selected, return an error
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid staff member selected.");
            }

            // Update the appointment with the selected staff ID
            appointment.StaffId = staffId;
            db.SaveChanges();

            // Redirect back to the Admin Dashboard to see the change
            return RedirectToAction("AdminDashboard", "Dashboard");
        }

        // POST: Admin/ConfirmAppointment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            // Change the status and save
            appointment.Status = "Confirmed";
            db.SaveChanges();

            return RedirectToAction("AdminDashboard", "Dashboard");
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
