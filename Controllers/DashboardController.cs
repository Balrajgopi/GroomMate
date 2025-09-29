using GroomMate.Controllers;
using GroomMate.Models;
using System;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;

namespace GroomMate.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // FOR ADMIN
        [Authorize(Roles = "Admin")]
        public ActionResult AdminDashboard()
        {
            var allAppointments = db.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .ToList();

            ViewBag.StaffList = new SelectList(db.Users.Where(u => u.Role.RoleName == "Staff"), "UserID", "FullName");

            return View(allAppointments);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AssignStaff(int appointmentId, int? staffId) // CORRECTED: Nullable int
        {
            // Server-side validation
            if (staffId == null)
            {
                TempData["ErrorMessage"] = "Please select a staff member to assign.";
                return RedirectToAction("AdminDashboard");
            }

            var appointment = db.Appointments.Find(appointmentId);
            if (appointment != null)
            {
                appointment.StaffId = staffId.Value; // Use .Value for nullable type
                appointment.Status = "Confirmed";
                db.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult MarkAsCompleted(int appointmentId)
        {
            var appointment = db.Appointments.Find(appointmentId);
            if (appointment != null && appointment.Status == "Confirmed")
            {
                appointment.Status = "Completed";
                db.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }

        // FOR STAFF
        [Authorize(Roles = "Staff")]
        public ActionResult StaffDashboard()
        {
            int staffId = (int)Session["UserID"];
            var assignedAppointments = db.Appointments
                .Where(a => a.StaffId == staffId && a.Status == "Confirmed")
                .Include(a => a.Service)
                .Include(a => a.User)
                .ToList();
            return View(assignedAppointments);
        }

        // FOR CUSTOMER
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerDashboard()
        {
            var services = db.Services.ToList();
            return View(services);
        }

        [Authorize(Roles = "Customer")]
        public ActionResult ViewAppointments()
        {
            int userId = (int)Session["UserID"];
            var userAppointments = db.Appointments
                .Where(a => a.UserID == userId)
                .Include(a => a.Service)
                .ToList();
            return View(userAppointments);
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult BookAppointment(int serviceId, DateTime appointmentDate)
        {
            int userId = (int)Session["UserID"];
            var appointment = new Appointment
            {
                ServiceID = serviceId,
                UserID = userId,
                AppointmentDate = appointmentDate,
                Status = "Pending"
            };
            db.Appointments.Add(appointment);
            db.SaveChanges();
            return RedirectToAction("ViewAppointments");
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
