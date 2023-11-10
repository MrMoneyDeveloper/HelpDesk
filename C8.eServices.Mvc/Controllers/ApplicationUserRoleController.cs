using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using C8.eServices.Mvc.Models;
using C8.eServices.Mvc.DataAccessLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using C8.eServices.Mvc.Helpers;
using C8.eServices.Mvc.Keys;
using MoreLinq;

namespace C8.eServices.Mvc.Controllers
{
    public class ApplicationUserRoleController : Controller
    {
        #region ApplicationUserRole Init
        private eServicesDbContext db = new eServicesDbContext();
        BaseHelper _base = new BaseHelper();

        public ApplicationUserRoleController()
        {
            IdentityManager = new IdentityManager(db);
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            UserManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(db));
        }

        public IdentityManager IdentityManager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
        public UserManager<SystemIdentityUser> UserManager { get; private set; }

        #endregion


        #region ApplicationUserRole Index
        //
        // GET: /ApplicationUserRole/
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Index(int? id)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    var applicationUserRoles = context.ApplicationUserRoles.Include(a => a.SystemUser)
                                .Include(a => a.IdentityRole)
                                .Where(a => a.IsActive && a.IsDeleted == false);
                    string applicationName;

                    if (id != null)
                    {
                        var application = context.Applications.Find(id);
                        applicationUserRoles = applicationUserRoles.Where(a => a.ApplicationId == id);
                        applicationName = application.Name;
                    }
                    else { applicationName = "All Applications"; }

                    ViewBag.ApplicationId = id;
                    ViewBag.ApplicationName = applicationName;
                    return View(applicationUserRoles.ToList());
                }
                catch
                {
                    return View("_Error");
                }
            }
        }

        #endregion


        #region ApplicationUserRole Details
        //
        // GET: /ApplicationUserRole/Details/5
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion


        #region ApplicationUserRole Create GET
        //
        // GET: /ApplicationUserRole/Create
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Create(int? id)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    if (id == null) throw new Exception("Invalid id");

                    var currentRoleApplication = context.ApplicationRoles.Where(ar => ar.IsActive && ar.IsDeleted == false
                                                 && ar.ApplicationId == id).Select(cr => cr.RoleId);
                    var roles = context.Roles.Where(r => currentRoleApplication.Contains(r.Id) && r.Name == "Administrators" || r.Name == "Clerks").ToList();
                    var users = context.Users.Where(u => u.Roles.Count == 0).ToList();
                    var userIds = users.Select(u => u.SystemUserId).ToList();
                    var unassignedSystemUsers = context.ApplicationUserRoles.Include(s => s.SystemUser)
                                                .Where(s => userIds.Contains(s.SystemUserId)).DistinctBy(s => s.SystemUserId).ToList();

                    ViewBag.UserId = new SelectList(unassignedSystemUsers.DistinctBy(s => s.SystemUserId).Select(u => new
                    {
                        u.SystemUserId,
                        u.SystemUser.UserName
                    }), "SystemUserId", "UserName");
                    ViewBag.RoleId = new SelectList(roles, "Name", "Name");
                    ViewBag.ApplicationId = id;

                    return View(unassignedSystemUsers);
                }
                catch
                {
                    return View("_Error");
                }
            }
        }

        #endregion

        #region ApplicationUserRole Create POST
        //
        // POST: /ApplicationUserRole/Create
        [HttpPost]
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Create(FormCollection collection)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    _base.Initialise(context);

                    var userType = Request.Form["userType"];
                    var applicationId = Convert.ToInt32(Request.Form["applicationId"]);
                    var ipAddress = Request.Form["ipAddress"];
                    var role = Request.Form["RoleId"];
                    var randomPassword = GeneratePassword(10);
                    var email = new Email();
                    var identityManager = new IdentityManager();
                    var applicationUserRole = new ApplicationUserRole
                    {
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        ApplicationId = applicationId,
                        RoleId = RoleManager.FindByName(role).Id
                    };

                    var defaultCustomer = context.CustomerTypes.FirstOrDefault(c => c.Key == CustomerTypeKeys.Individual);
                    var defaultIdentification = context.IdentificationTypes.FirstOrDefault(id => id.Key == IdentificationTypeKey.SouthAfricanID);
                    var defaultTitleType = context.TitleTypes.FirstOrDefault(t => t.Key == TitleTypeKeys.Mister);
                    var defaultStatus = context.Status.FirstOrDefault(s => s.Key == StatusKeys.CustomerActive);

                    if (defaultCustomer == null) throw new Exception("Invalid customer type");
                    if (defaultIdentification == null) throw new Exception("Invalid identification");
                    if (defaultTitleType == null) throw new Exception("Invalid title type");
                    if (defaultStatus == null) throw new Exception("Invalid status");

                    var customer = new Customer()
                    {
                        CustomerTypeId = defaultCustomer.Id,
                        IdentificationTypeId = defaultIdentification.Id,
                        CountryOfIssueTypeId = null,
                        IdentificationNumber = "1234567891011",
                        TitleTypeId = defaultTitleType.Id,
                        FirstName = "Test",
                        LastName = "Profile",
                        Gender = null,
                        IsDeceased = false,
                        EmailAddress = "callcentreagent@durban.gov.za",
                        PhysicalAddressCode = 0000,
                        PostalAddressCode = 0000,
                        StatusId = defaultStatus.Id,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false
                    };

                    if (userType == "Existing User")
                    {
                        var systemUserId = Convert.ToInt32(Request.Form["UserId"]);
                        var identityUser = context.Users.FirstOrDefault(i => i.SystemUserId == systemUserId);

                        if (identityUser == null) throw new Exception("Invalid identity user");

                        identityManager.AddUserToRole(identityUser.Id, role);
                        applicationUserRole.SystemUserId = systemUserId;
                        customer.SystemUserId = systemUserId;

                        context.ApplicationUserRoles.Add(applicationUserRole);
                        context.Customers.Add(customer);

                        var applicationAccess = context.Applications.Find(applicationId);
                        const string emailSubject = "Siyakhokha: User Application Role";
                        var emailBody = "<b>You have been successfully added to a new system.</b><br/><br/>" +
                                "<b>Application User Role Details:</b><br/>" +
                                "Application Access: " + applicationAccess.Name + "<br/>" +
                                "Role: " + role + "<br/><br/>";

                        email.GenerateEmail(identityUser.Email,
                            emailSubject, emailBody, identityUser.SystemUserId.ToString(), false,
                            AppSettingKeys.EservicesDefaultEmailTemplate, identityUser.SystemUser.FullName);

                        context.SaveChanges();

                        return RedirectToAction("Index", new { id = applicationId });
                    }

                    var firstName = Request.Form["firstName"];
                    var surname = Request.Form["lastName"];
                    var username = Request.Form["userName"];
                    var emailAddress = Request.Form["emailAddress"];

                    var usernameAssigned = context.SystemUsers.Any(u => u.UserName.ToLower() == username.ToLower()
                        && u.IsActive && u.IsDeleted == false);
                    var emailAssigned = context.SystemUsers.Any(u => u.EmailAddress.ToLower() == emailAddress.ToLower()
                        && u.IsActive && u.IsDeleted == false);

                    if (!usernameAssigned && emailAssigned)
                    {
                        TempData["Error"] = "Email address registered. Please use an alternative email address";
                    }
                    else if (usernameAssigned && !emailAssigned)
                    {
                        TempData["Error"] = "Username registered. Please choose a unique username.";
                    }
                    else if (usernameAssigned && emailAssigned)
                    {
                        TempData["Error"] = "Username and Email address registered. Please use an alternative email address and a unique username.";
                    }
                    else
                    {
                        // JK.20140724a - Passing values from the ViewModel to the Model.
                        var user = new SystemIdentityUser
                        {
                            UserName = username,
                            Email = emailAddress,
                            EmailConfirmed = true,
                            SystemUser = new SystemUser()
                            {
                                FirstName = firstName,
                                LastName = surname,
                                UserName = username,
                                EmailAddress = emailAddress,
                                IsActive = true,
                                IsDeleted = false,
                                IsLocked = false,
                                ModifiedDateTime = DateTime.Now
                            }
                        };

                        // JK.20140724a - Custom profile information.

                        // Send email to User with Username and Temp Password
                        identityManager.CreateUser(user, randomPassword);
                        identityManager.AddUserToRole(user.Id, role);

                        applicationUserRole.SystemUserId = user.SystemUserId;
                        customer.SystemUserId = user.SystemUserId;

                        context.ApplicationUserRoles.Add(applicationUserRole);
                        context.Customers.Add(customer);

                        var applicationAccess = context.Applications.Find(applicationId);
                        const string emailSubject = "Siyakhokha: User Registration";
                        var emailBody = "<b>You have been successfully added onto Siyakhokha Application.</b><br/><br/>" +
                                        "<b>Login Details:</b><br/>" +
                                        "Username: " + user.UserName + "<br/>" +
                                        "Temporary Password: " + randomPassword + "<br/>" +
                                        "Application Access: " + applicationAccess.Name + "<br/>" +
                                        "Role: " + role + "<br/><br/>" +
                                        "<b> Please change temporary password on your first login.</b>";

                        email.GenerateEmail(user.Email, emailSubject, emailBody, user.SystemUserId.ToString(), false, AppSettingKeys.EservicesDefaultEmailTemplate, user.SystemUser.FullName);

                        context.SaveChanges();

                        return RedirectToAction("Index", new { id = applicationId });
                    }

                    var currentRoleApplication = context.ApplicationRoles.Where(ar => ar.IsActive && ar.IsDeleted == false
                    && ar.ApplicationId == applicationId).Select(cr => cr.RoleId);
                    var roles = context.Roles.Where(r => currentRoleApplication.Contains(r.Id) && r.Name == "Administrators" || r.Name == "Clerks").ToList();
                    var users = context.Users.Where(u => u.Roles.Count == 0).ToList();
                    var userIds = users.Select(u => u.SystemUserId).ToList();

                    var unassignedSystemUsers = context.ApplicationUserRoles.Include(s => s.SystemUser)
                                .Where(s => userIds.Contains(s.SystemUserId)).DistinctBy(s => s.SystemUserId).ToList();

                    ViewBag.UserId = new SelectList(unassignedSystemUsers.DistinctBy(s => s.SystemUserId).Select(u => new
                    {
                        u.SystemUserId,
                        u.SystemUser.UserName
                    }), "SystemUserId", "UserName");

                    ViewBag.RoleId = new SelectList(roles, "Name", "Name");
                    ViewBag.ApplicationId = applicationId;

                    return View(unassignedSystemUsers);
                }
                catch
                {
                    return View("_Error");
                }
            }
        }
        #endregion


        #region ApplicationUserRole Edit GET
        //
        // GET: /ApplicationUserRole/Edit/5
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Edit(int? id, int? appId)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    if (id == null) throw new Exception("Invalid id");

                    var applicationuserrole = context.ApplicationUserRoles.Find(id);
                    var systemUser = context.SystemUsers.Find(applicationuserrole.SystemUserId);
                    var currentRoleApplication = context.ApplicationRoles.Where(ar => ar.IsActive && ar.IsDeleted == false
                    && ar.ApplicationId == appId).Select(cr => cr.RoleId);
                    var roles = context.Roles.Where(r => currentRoleApplication.Contains(r.Id) && r.Name == "Administrators" || r.Name == "Clerks").ToList();

                    ViewBag.ApplicationId = appId;
                    ViewBag.SystemUser = systemUser;
                    ViewBag.RoleId = new SelectList(roles, "Name", "Name");

                    return View();
                }
                catch
                {
                    return View("_Error");
                }
            }
        }
        #endregion

        #region ApplicationUserRole Edit POST
        //
        // POST: /ApplicationUserRole/Edit/5
        [HttpPost]
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Edit(int? id, FormCollection collection)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    _base.Initialise(context);

                    var applicationId = Convert.ToInt32(Request.Form["applicationId"]);
                    var ipAddress = Request.Form["ipAddress"];
                    var email = new Email();
                    var applicationuserrole = context.ApplicationUserRoles.Include(a => a.SystemUser)
                                              .FirstOrDefault(a => a.Id == id);

                    if (applicationuserrole == null) throw new Exception("Invalid application user role");
                    var user = UserManager.FindByName(applicationuserrole.SystemUser.UserName);
                    if (user == null) throw new Exception("Invalid user");

                    var currentRole = RoleManager.FindById(applicationuserrole.RoleId).Name;
                    var newRole = Request.Form["RoleId"];
                    var applicationAccess = context.Applications.Find(applicationId);
                    const string emailSubject = "Siyakhokha: Application User Role Modified";
                    var emailBody = "<b>Your Application User Role Has Been Modified.</b><br/><br/>" +
                            "<b>Application User Role Details:</b><br/>" +
                            "Application Access: " + applicationAccess.Name + "<br/>" +
                            "Old Role: " + currentRole + "<br/>" +
                            "New Role: " + newRole + "<br/><br/>";

                    email.GenerateEmail(user.Email,
                        emailSubject, emailBody, user.SystemUserId.ToString(), false, AppSettingKeys.EservicesDefaultEmailTemplate, user.SystemUser.FullName);

                    IdentityManager.UserManager.RemoveFromRole(user.Id, currentRole);
                    IdentityManager.AddUserToRole(user.Id, newRole);

                    applicationuserrole.RoleId = RoleManager.FindByName(newRole).Id;
                    applicationuserrole.IsActive = true;
                    applicationuserrole.IsDeleted = false;
                    applicationuserrole.IsLocked = false;

                    context.Entry(applicationuserrole).State = EntityState.Modified;
                    context.SaveChanges();

                    return RedirectToAction("Index", new { id = applicationId });
                }
                catch
                {
                    return View("_Error");
                }
            }
        }

        #endregion


        #region ApplicationUserRole Delete GET
        //
        // GET: /ApplicationUserRole/Delete/5
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Delete(int? id, int? appId)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    if (id == null) throw new Exception("Invalid id");

                    var applicationuserrole = context.ApplicationUserRoles.Include(l => l.SystemUser)
                    .Include(l => l.IdentityRole)
                    .Include(l => l.Application)
                    .FirstOrDefault(l => l.Id == id);

                    if (applicationuserrole == null) throw new Exception("Invalid application user role");

                    ViewBag.ApplicationId = appId;
                    return View(applicationuserrole);
                }
                catch
                {
                    return View("_Error");
                }
            }
        }
        #endregion

        #region ApplicationUserRole Delete POST
        //
        // POST: /ApplicationUserRole/Delete/5
        [HttpPost]
        [Authorize(Roles = "Super Administrators")]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            using (var context = new eServicesDbContext())
            {
                try
                {
                    _base.Initialise(context);

                    var applicationId = Convert.ToInt32(Request.Form["applicationId"]);
                    var email = new Email();

                    if (id == null) throw new Exception("Invalid id");

                    var applicationuserrole = context.ApplicationUserRoles.Find(id);
                    var user = context.Users.First(u => u.SystemUserId == applicationuserrole.SystemUserId);
                    var currentRole = RoleManager.FindById(applicationuserrole.RoleId).Name;
                    var applicationAccess = context.Applications.Find(applicationId);
                    const string emailSubject = "Siyakhokha: Application User Role Revoked";
                    var emailBody = "<b>Your access to " + applicationAccess.Name + " has been revoked .</b><br/><br/>" +
                            "<b>Application User Role Details:</b><br/>" +
                            "Application Access Revoked: " + applicationAccess.Name + "<br/>" +
                            "Role: " + currentRole + "<br/><br/>" +
                            "<b>Please contact the administrator for further details.</b>";

                    email.GenerateEmail(user.Email,
                        emailSubject, emailBody, user.SystemUserId.ToString(), false, AppSettingKeys.EservicesDefaultEmailTemplate,
                        user.SystemUser.FullName);

                    IdentityManager.UserManager.RemoveFromRole(user.Id, currentRole);

                    applicationuserrole.IsActive = false;
                    applicationuserrole.IsDeleted = true;
                    applicationuserrole.IsLocked = false;

                    context.Entry(applicationuserrole).State = EntityState.Modified;
                    context.SaveChanges();

                    return RedirectToAction("Index", new { id = applicationId });
                }
                catch
                {
                    return View("_Error");
                }
            }
        }
        #endregion

        #region Generate User Password
        public string GeneratePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var res = new StringBuilder();
            var rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        #endregion
    }
}
