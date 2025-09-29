using GroomMate.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GroomMate.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private GroomMateContext db = new GroomMateContext();

        public ActionResult CustomerDashboard()
        {
            var services = db.Services.ToList();
            return View(services);
        }

        [HttpPost]
        public ActionResult BookAppointment(int serviceId, DateTime appointmentDate)
        {
            int userId = (int)Session["UserID"];
            var appointment = new Appointment
            {
                UserID = userId,
                ServiceID = serviceId,
                AppointmentDate = appointmentDate,
                Status = "Pending"
            };
            db.Appointments.Add(appointment);
            db.SaveChanges();
            return RedirectToAction("CustomerDashboard");
        }

        public ActionResult ViewAppointments()
        {
            int userId = (int)Session["UserID"];
            var appointments = db.Appointments
                                 .Include(a => a.Service)
                                 .Where(a => a.UserID == userId)
                                 .ToList();
            return View(appointments);
        }

        public ActionResult StaffDashboard()
        {
            int staffId = (int)Session["UserID"];
            var appointments = db.Appointments
                                 .Include(a => a.Service)
                                 .Where(a => a.StaffID == staffId)
                                 .ToList();
            return View(appointments);
        }

        public ActionResult AdminDashboard()
        {
            var appointments = db.Appointments
                                 .Include(a => a.Customer)
                                 .Include(a => a.Service)
                                 .Include(a => a.Staff)
                                 .ToList();
            return View(appointments);
        }

        [HttpPost]
        public ActionResult ConfirmAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Status = "Confirmed";
                db.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }

        [HttpPost]
        public ActionResult RejectAppointment(int id)
        {
            var appointment = db.Appointments.Find(id);
            if (appointment != null)
            {
                appointment.Status = "Rejected";
                db.SaveChanges();
            }
            return RedirectToAction("AdminDashboard");
        }
    }
}
