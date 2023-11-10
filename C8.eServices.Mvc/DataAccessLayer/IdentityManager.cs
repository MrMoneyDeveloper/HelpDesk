using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.eServices.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace C8.eServices.Mvc.DataAccessLayer
{
    public class IdentityManager
    {
        private eServicesDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityManager"/> class.
        /// </summary>
        public IdentityManager()
        {
            _context = new eServicesDbContext();
            UserManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(_context));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityManager"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public IdentityManager(eServicesDbContext context)
        {
            _context = context;
            UserManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(_context));
        }

        /// <summary>
        /// Gets or sets the user manager.
        /// </summary>
        /// <value>
        /// The user manager.
        /// </value>
        public UserManager<SystemIdentityUser> UserManager { get; set; }

        /// <summary>
        /// Roles the exists.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(_context));
            return rm.RoleExists(name);
        }

        public bool UserExists(string name)
        {
            var um = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(_context));

            return um.FindByName(name) != null;
        }

        /// <summary>
        /// Creates the role.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool CreateRole(string name)
        {
            try
            {
                var rm = new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(_context));
                var idResult = rm.Create(new IdentityRole(name));

                return idResult.Succeeded;
            }
            catch (Exception x)
            {

                throw x;
            }
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public bool CreateUser(SystemIdentityUser user, string password)
        {
            var idResult = UserManager.Create(user, password);

            return idResult.Succeeded;
        }

        /// <summary>
        /// Adds the user to role.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public bool AddUserToRole(string userId, string roleName)
        {
            var idResult = UserManager.AddToRole(userId, roleName);

            return idResult.Succeeded;
        }

        /// <summary>
        /// Clears the user roles.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        public void ClearUserRoles(string userId)
        {
            var user = UserManager.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);

            foreach (var role in currentRoles)
            {
                //um.RemoveFromRole( userId, role.Role.Name );
            }
        }

        /// <summary>
        /// Finds the system user.
        /// </summary>
        /// <param name="systemUserGuid">The application user unique identifier.</param>
        /// <returns></returns>
        public SystemIdentityUser CurrentSystemUser(string systemUserGuid)
        {
            SystemIdentityUser aUser = UserManager.FindById(systemUserGuid);
            _context.CurrentSystemUser = aUser.SystemUser;
            return aUser;
        }

        /// <summary>
        /// Finds the system user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public SystemIdentityUser CurrentSystemUser(System.Security.Principal.IPrincipal user)
        {
            if (user != null)
            {
                SystemIdentityUser aUser = UserManager.FindById(user.Identity.GetUserId());
                _context.CurrentSystemUser = aUser.SystemUser;
                return aUser;
            }

            return null;
        }

        /// <summary>
        /// Finds the user.
        /// </summary>
        /// <param name="systemUserGuid">The application user unique identifier.</param>
        /// <returns></returns>
        public SystemUser CurrentUser(string systemUserGuid)
        {
            SystemIdentityUser aUser = UserManager.FindById(systemUserGuid);
            _context.CurrentSystemUser = aUser.SystemUser;
            return aUser.SystemUser;
        }

        /// <summary>
        /// Finds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public SystemUser CurrentUser(System.Security.Principal.IPrincipal user)
        {
            if (user != null)
            {
                SystemIdentityUser aUser = UserManager.FindById(user.Identity.GetUserId());
                _context.CurrentSystemUser = aUser.SystemUser;
                return aUser.SystemUser;        
            }

            return null;
        }

        public string CurrentUserId(string userName)
        {
            if (userName != null)
            {
                var aUser = UserManager.FindByName(userName);
                return aUser.Id;
            }

            return null;
        }
    }
}