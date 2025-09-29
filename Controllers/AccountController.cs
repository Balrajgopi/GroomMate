using GroomMate.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace GroomMate.Controllers
{
    public class AccountController : Controller
    {
        // Make the DbContext field readonly for safety and clarity.
        private readonly GroomMateContext db = new GroomMateContext();

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public ActionResult Register(string fullName, string email, string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                ViewBag.Error = "Passwords do not match.";
                return View();
            }
            if (db.Users.Any(u => u.Username == username))
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            // Use FirstOrDefault for a safer query. It returns null if not found.
            var customerRole = db.Roles.FirstOrDefault(r => r.RoleName == "Customer");

            // Add a check to ensure the role exists. This gives a better error if seeding fails.
            if (customerRole == null)
            {
                ViewBag.Error = "A critical error occurred: The 'Customer' role is not configured in the database.";
                return View();
            }

            var user = new User
            {
                FullName = fullName,
                Email = email,
                Username = username,
                Password = password, // Warning: Storing plain-text passwords is a major security risk.
                RoleID = customerRole.RoleID
            };

            db.Users.Add(user);
            db.SaveChanges();

            FormsAuthentication.SetAuthCookie(username, false);
            Session["UserID"] = user.UserID;
            Session["Role"] = "Customer";

            return RedirectToAction("CustomerDashboard", "Dashboard");
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            // The .Include("Role") eagerly loads the related Role data.
            var user = db.Users.Include("Role").FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.Username, false);
                Session["UserID"] = user.UserID;
                Session["Role"] = user.Role.RoleName;

                switch (user.Role.RoleName)
                {
                    case "Admin":
                        return RedirectToAction("AdminDashboard", "Dashboard");
                    case "Staff":
                        return RedirectToAction("StaffDashboard", "Dashboard");
                    case "Customer":
                        return RedirectToAction("CustomerDashboard", "Dashboard");
                }
            }

            ViewBag.ErrorMessage = "Invalid username or password.";
            return View();
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }

        // Add the Dispose method to properly release database connection resources.
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
