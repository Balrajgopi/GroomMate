using GroomMate.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace GroomMate.Security
{
    public static class AuthHelper
    {
        /// <summary>
        /// Restores or retrieves the current logged-in user's ID from Session or FormsAuthentication identity.
        /// If Session has expired or was cleared after an AppPool recycle, restores Session state from the database.
        /// </summary>
        public static int? RestoreUserSession(HttpContextBase context, GroomMateContext db = null)
        {
            if (context == null) return null;

            var session = context.Session;
            if (session != null && session["UserID"] is int sessionUserId)
            {
                return sessionUserId;
            }

            var userPrincipal = context.User;
            if (userPrincipal != null && userPrincipal.Identity != null && userPrincipal.Identity.IsAuthenticated)
            {
                string username = userPrincipal.Identity.Name?.Trim();
                if (!string.IsNullOrEmpty(username))
                {
                    bool disposeDb = false;
                    if (db == null)
                    {
                        db = new GroomMateContext();
                        disposeDb = true;
                    }

                    try
                    {
                        var user = db.Users.Include(u => u.Role)
                                           .FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && !u.IsDeleted);
                        if (user != null)
                        {
                            if (session != null)
                            {
                                session["UserID"] = user.UserID;
                                session["Username"] = user.Username;
                                session["Role"] = user.Role?.RoleName;
                            }
                            return user.UserID;
                        }
                    }
                    finally
                    {
                        if (disposeDb)
                        {
                            db.Dispose();
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Overload for standard HttpContext.
        /// </summary>
        public static int? RestoreUserSession(HttpContext context, GroomMateContext db = null)
        {
            if (context == null) return null;
            return RestoreUserSession(new HttpContextWrapper(context), db);
        }
    }
}
