using GroomMate.Models;
using System;
using System.Web.Security;
using System.Linq;

namespace GroomMate.Security
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get; set; }

        public override string[] GetRolesForUser(string username)
        {
            using (var db = new GroomMateContext())
            {
                // --- UPDATED LOGIC ---
                // Now filters out users where IsDeleted is true.
                var user = db.Users.Include("Role").FirstOrDefault(u => u.Username == username && !u.IsDeleted);
                if (user != null && user.Role != null)
                {
                    return new string[] { user.Role.RoleName };
                }
                return new string[] { };
            }
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (var db = new GroomMateContext())
            {
                // --- UPDATED LOGIC ---
                // Also filters out users where IsDeleted is true.
                var user = db.Users.Include("Role").FirstOrDefault(u => u.Username == username && !u.IsDeleted);
                return user != null && user.Role != null && user.Role.RoleName == roleName;
            }
        }

        // The methods below are not required for this project, 
        // but they must be implemented to satisfy the abstract class.
        #region Not Implemented Methods
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
