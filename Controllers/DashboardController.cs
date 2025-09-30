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

            return View(appointments);
        }

        // GET: Dashboard/StaffDashboard
        [Authorize(Roles = "Staff")]
        public ActionResult StaffDashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int currentStaffId = (int)Session["UserID"];
            var staffAppointments = db.Appointments
                .Where(a => a.StaffId == currentStaffId)
                .Include(a => a.User)
                .Include(a => a.Service)
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(staffAppointments);
        }

        // *** THIS IS THE NEW ACTION TO FIX THE 404 ERROR ***
        // GET: Dashboard/CustomerDashboard
        [Authorize(Roles = "Customer")]
        public ActionResult CustomerDashboard()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int currentUserId = (int)Session["UserID"];

            // Fetch all appointments for the currently logged-in customer
            var customerAppointments = db.Appointments
                .Where(a => a.UserID == currentUserId)
                .Include(a => a.Service)
                .Include(a => a.Staff) // Include staff details
                .OrderByDescending(a => a.AppointmentDate)
                .ToList();

            return View(customerAppointments);
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
