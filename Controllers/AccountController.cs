using GroomMate.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security; // Required for FormsAuthentication

namespace GroomMate.Controllers
{
    public class AccountController : Controller
    {
        private readonly GroomMateContext db = new GroomMateContext();

        // GET: Account/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.Username.Equals(user.Username, System.StringComparison.OrdinalIgnoreCase)))
                {
                    ModelState.AddModelError("Username", "This username is already taken.");
                    return View(user);
                }

                var customerRole = db.Roles.FirstOrDefault(r => r.RoleName == "Customer");
                if (customerRole == null)
                {
                    ModelState.AddModelError("", "A system configuration error occurred.");
                    return View(user);
                }

                user.RoleID = customerRole.RoleID;
                user.IsDeleted = false;

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login", "Account");
            }
            return View(user);
        }

        // *** START: NEW LOGIN/LOGOUT ACTIONS ***

        // GET: Account/Login
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Find the user in the database
                var user = db.Users.Include(u => u.Role) // Eagerly load the Role
                                   .FirstOrDefault(u => u.Username.Equals(model.Username, System.StringComparison.OrdinalIgnoreCase)
                                                      && u.Password == model.Password && !u.IsDeleted);

                if (user != null)
                {
                    // Set authentication cookie
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);

                    // Store user details in session for easy access
                    Session["UserID"] = user.UserID;
                    Session["Username"] = user.Username;
                    Session["Role"] = user.Role.RoleName;

                    // Redirect to the originally requested page, or a dashboard
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    // Redirect based on user role
                    switch (user.Role.RoleName)
                    {
                        case "Admin":
                            return RedirectToAction("AdminDashboard", "Dashboard");
                        case "Staff":
                            return RedirectToAction("StaffDashboard", "Dashboard");
                        default: // Customer
                            return RedirectToAction("CustomerDashboard", "Dashboard");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // Clears the session
            return RedirectToAction("Index", "Home");
        }

        // *** END: NEW LOGIN/LOGOUT ACTIONS ***

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    // A simple ViewModel for the login page
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
