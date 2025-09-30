using GroomMate.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    [Authorize(Roles = "Customer")]
    public class AppointmentController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // GET: Appointment/Create/{serviceId}
        public ActionResult Create(int serviceId)
        {
            var service = db.Services.Find(serviceId);
            if (service == null || !service.IsActive)
            {
                return HttpNotFound();
            }
            var appointment = new Appointment { ServiceID = service.ServiceID, AppointmentDate = DateTime.Now };
            ViewBag.ServiceName = service.ServiceName;
            return View(appointment);
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Appointment appointment)
        {
            // First, check the business hours before checking ModelState
            string validationError = IsWithinBusinessHours(appointment.AppointmentDate);
            if (!string.IsNullOrEmpty(validationError))
            {
                // If there's a validation error, add it to the ModelState
                ModelState.AddModelError("AppointmentDate", validationError);
            }

            if (ModelState.IsValid)
            {
                if (Session["UserID"] == null) return RedirectToAction("Login", "Account");
                appointment.UserID = (int)Session["UserID"];
                appointment.Status = "Pending";
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("CustomerDashboard", "Dashboard");
            }

            // If we got this far, something failed, redisplay form
            var service = db.Services.Find(appointment.ServiceID);
            ViewBag.ServiceName = service?.ServiceName;
            return View(appointment);
        }

        // POST: Appointment/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            if (Session["UserID"] == null) return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            int currentUserId = (int)Session["UserID"];
            var appointment = db.Appointments.FirstOrDefault(a => a.AppointmentID == id && a.UserID == currentUserId);
            if (appointment == null) return HttpNotFound();
            if (appointment.Status == "Pending" || appointment.Status == "Confirmed")
            {
                appointment.Status = "Cancelled";
                db.SaveChanges();
            }
            return RedirectToAction("CustomerDashboard", "Dashboard");
        }

        // *** HELPER METHOD FOR BUSINESS HOURS VALIDATION ***
        private string IsWithinBusinessHours(DateTime dt)
        {
            TimeSpan time = dt.TimeOfDay;
            DayOfWeek day = dt.DayOfWeek;

            switch (day)
            {
                case DayOfWeek.Tuesday:
                    return "Sorry, the salon is closed on Tuesdays. Please select another day.";

                case DayOfWeek.Saturday:
                    if (time < new TimeSpan(8, 0, 0) || time > new TimeSpan(21, 0, 0))
                        return "Our hours on Saturday are 8:00 AM to 9:00 PM.";
                    break;

                case DayOfWeek.Sunday:
                    if (time < new TimeSpan(10, 0, 0) || time > new TimeSpan(18, 0, 0))
                        return "Our hours on Sunday are 10:00 AM to 6:00 PM.";
                    break;

                default: // Monday, Wednesday, Thursday, Friday
                    if (time < new TimeSpan(9, 0, 0) || time > new TimeSpan(22, 0, 0))
                        return "Our weekday hours are 9:00 AM to 10:00 PM.";
                    break;
            }

            // If no validation rules were broken, return null (meaning success)
            return null;
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
