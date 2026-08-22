using GroomMate.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // GET: Dashboard/AdminDashboard
        [Authorize(Roles = "Admin")]
        public ActionResult AdminDashboard()
        {
            var appointments = db.Appointments
                .Include(a => a.User)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            ViewBag.StaffList = db.Users
                .Where(u => u.Role.RoleName == "Staff" && !u.IsDeleted)
                .Select(u => new SelectListItem
                {
                    Value = u.UserID.ToString(),
                    Text = u.FullName
                }).ToList();

            // Fetch all customer feedbacks eagerly loading the appointment, customer, and service details
            ViewBag.Feedbacks = db.Feedbacks
                .Include(f => f.Appointment.User)
                .Include(f => f.Appointment.Service)
                .ToList();

            return View(appointments);
        }

        // GET: Dashboard/StaffDashboard
        [Authorize(Roles = "Staff")]
        public ActionResult StaffDashboard()
        {
            int? currentStaffId = RestoreUserSession();
            if (!currentStaffId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            var staffAppointments = db.Appointments
                .Where(a => a.StaffId == currentStaffId.Value)
                .Include(a => a.User)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(staffAppointments);
        }

        // GET: Dashboard/CustomerDashboard
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerDashboard()
        {
            int? currentUserId = RestoreUserSession();
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Login", "Account");
            }

            // Fetch all appointments for the currently logged-in customer
            var customerAppointments = db.Appointments
                .Where(a => a.UserID == currentUserId.Value)
                .Include(a => a.Service)
                .Include(a => a.Staff)
                .Include(a => a.Feedback)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(customerAppointments);
        }

        private int? RestoreUserSession()
        {
            return GroomMate.Security.AuthHelper.RestoreUserSession(HttpContext, db);
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
