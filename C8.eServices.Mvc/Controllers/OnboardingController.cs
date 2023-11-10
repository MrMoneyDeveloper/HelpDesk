using C8.eServices.Mvc.DataAccessLayer;
using C8.eServices.Mvc.Helpers;
using C8.eServices.Mvc.Keys;
using C8.eServices.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web.Mvc;

namespace C8.eServices.Mvc.Controllers
{
    public class OnboardingController : Controller
    {
        BaseHelper _base = new BaseHelper();
        eServicesDbContext db = new eServicesDbContext();
        private eServicesDbContext _context;
        //public OnboardingController()
        //   : this(new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(new eServicesDbContext())))
        //{
        //    //allow alphanumeric usernames  
        //    //UserManager.UserValidator = new UserValidator<SystemIdentityUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        //}

        //public OnboardingController(UserManager<SystemIdentityUser> userManager)
        //{
        //    UserManager = userManager;
        //    var provider = new DpapiDataProtectionProvider("eServices");
        //    UserManager.UserValidator = new UserValidator<SystemIdentityUser>(UserManager) { AllowOnlyAlphanumericUserNames = false };
        //    UserManager.UserTokenProvider = new DataProtectorTokenProvider<SystemIdentityUser>(
        //                                    provider.Create("EmailConfirmation"))
        //    {
        //        TokenLifespan = TimeSpan.FromDays(14)
        //    };
        //}

        //public OnboardingController(eServicesDbContext context)
        //{
        //    eServicesDbContext _context = new eServicesDbContext();
        //    UserManager =
        //        new UserManager<SystemIdentityUser>(
        //            new UserStore<SystemIdentityUser>(_context));
        //}

        public static UserManager<SystemIdentityUser> UserManager { get; private set; }


        public OnboardingController()
        {
            _context = new eServicesDbContext();
            UserManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(_context));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityManager"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public OnboardingController(eServicesDbContext context)
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


        [Authorize]
        [DecryptParameter]
        public ActionResult Index(string queueKey)
        {
            ViewBag.queueKey = queueKey;
            switch (queueKey)
            {
                case OnboardingKeys.PendingApproval:
                    ViewBag.Onboarding = "- Pending Approval";
                    break;
                case OnboardingKeys.DeclinedApproval:
                    ViewBag.Onboarding = "- Declined Approval";
                    break;
                case OnboardingKeys.ApprovedOnboarding:
                    ViewBag.Onboarding = "- Approved";
                    break;
            }
            return View();
        }



        [Authorize]
        public ActionResult Clients()
        {
            _base.Initialise(db);
            return View();
        }

        public ActionResult IndexLoadData(string queueKey)
        {
            List<OnboardingRequest> requests = new List<OnboardingRequest>();
            using (eServicesDbContext context = new eServicesDbContext())
            {
                requests = context.OnboardingRequests
                    .Include(d => d.Country)
                    .Include(d => d.Industry)
                    .Include(d => d.Status)
                    .Where(c => c.IsActive).ToList();
            }
            switch (queueKey)
            {
                case OnboardingKeys.PendingApproval:
                    requests = requests.Where(o => o.Status.Key == StatusKeys.OnboardingPending).ToList();
                    break;
                case OnboardingKeys.DeclinedApproval:
                    requests = requests.Where(o => o.Status.Key == StatusKeys.OnboardingDeclined).ToList();
                    break;
                case OnboardingKeys.ApprovedOnboarding:
                    requests = requests.Where(o => o.Status.Key == StatusKeys.AccountActive).ToList();
                    break;
            }

            foreach (OnboardingRequest request in requests)
            {
                request.Data = new AesCrypto().Encrypt("requestId=" + request.Id + "&queueKey=" + queueKey);
                request.key = queueKey;
            }

            return Json(requests, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult loadClients()
        {
            List<Client> requests = new List<Client>();
            try
            {

                using (eServicesDbContext context = new eServicesDbContext())
                {
                    requests = context.Clients
                        .Include(d => d.Country)
                        .Include(d => d.RepresentedByUser)
                        .Include(d => d.Industry)
                        .Include(d => d.Status)
                        .Where(c => c.IsActive).ToList();
                }

                foreach (Client client in requests)
                {
                    client.Data = new AesCrypto().Encrypt("Id=" + client.Id);
                }

                return Json(requests, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(requests, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult SubmitOnboarding(string company, string vat, string industry, string country, string domain, string firstname, string lastname, string email, string phone)
        {
            object results = new { status = false, Msg = "an error has occured" };
            //results = new { status = true, Msg = "an error has occured", email = email };
            try
            {
                using (eServicesDbContext context = new eServicesDbContext())
                {
                    OnboardingRequest request = new OnboardingRequest
                    {
                        CompanyName = company,
                        VatNumber = vat,
                        IndustryId = context.Industries.FirstOrDefault(a => a.Key == industry).Id,
                        CountryId = context.Countries.FirstOrDefault(a => a.Key == country).Id,
                        CompanyDomain = domain,
                        FistName = firstname,
                        LastName = lastname,
                        EmailAddress = email,
                        ContactNumber = phone,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        CreatedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                        StatusId = context.Status.FirstOrDefault(a => a.Key == StatusKeys.OnboardingPending).Id,
                        PeriodTo = DateTime.Now
                    };
                    context.OnboardingRequests.Add(request);
                    context.SaveChanges();

                    Email _email = new Email();
                    _email.GenerateEmail(email, "XET Help Desk Onboarding",
                                    "XET Solutions Help Desk login notification on " + String.Format("{0:F}", DateTime.Now),
                                    request.Id.ToString(CultureInfo.InvariantCulture), false, AppSettingKeys.EservicesDefaultEmailTemplate, $"{firstname} {lastname}");
                    results = new { status = true, Msg = "an error has occured", email = email };
                }

            }
            catch (Exception)
            {

            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult OnboardigRequest()
        {
            ViewBag.PossibleTitles = new List<string>
            {
               "Mr.", "Mrs.","Miss","Dr.","Prof."
              // Add other titles as needed
            };

            //List<string> countries = new List<string>
            //{
            //    "Afghanistan", "Albania", "Algeria", "Andorra", "Angola", "Antigua and Barbuda",
            //"Argentina", "Armenia", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain",
            //"Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bhutan",
            //"Bolivia", "Bosnia and Herzegovina", "Botswana", "Brazil", "Brunei", "Bulgaria",
            //"Burkina Faso", "Burundi", "Cabo Verde", "Cambodia", "Cameroon", "Canada", "Central African Republic",
            //"Chad", "Chile", "China", "Colombia", "Comoros", "Congo", "Costa Rica", "Croatia", "Cuba",
            //"Cyprus", "Czech Republic", "Democratic Republic of the Congo", "Denmark", "Djibouti", "Dominica",
            //"Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia",
            //"Eswatini", "Ethiopia", "Fiji", "Finland", "France", "Gabon", "Gambia", "Georgia", "Germany",
            //"Ghana", "Greece", "Grenada", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti",
            //"Holy See", "Honduras", "Hungary", "Iceland", "India", "Indonesia", "Iran", "Iraq",
            //"Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati",
            //"Kuwait", "Kyrgyzstan", "Laos", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libya",
            //"Liechtenstein", "Lithuania", "Luxembourg", "Madagascar", "Malawi", "Malaysia", "Maldives",
            //"Mali", "Malta", "Marshall Islands", "Mauritania", "Mauritius", "Mexico", "Micronesia",
            //"Moldova", "Monaco", "Mongolia", "Montenegro", "Morocco", "Mozambique", "Myanmar", "Namibia",
            //"Nauru", "Nepal", "Netherlands", "New Zealand", "Nicaragua", "Niger", "Nigeria", "North Korea",
            //"North Macedonia", "Norway", "Oman", "Pakistan", "Palau", "Palestine State", "Panama",
            //"Papua New Guinea", "Paraguay", "Peru", "Philippines", "Poland", "Portugal", "Qatar",
            //"Romania", "Russia", "Rwanda", "Saint Kitts and Nevis", "Saint Lucia", "Saint Vincent and the Grenadines",
            //"Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia", "Seychelles",
            //"Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa",
            //"South Korea", "South Sudan", "Spain", "Sri Lanka", "Sudan", "Suriname", "Sweden", "Switzerland",
            //"Syria", "Tajikistan", "Tanzania", "Thailand", "Timor-Leste", "Togo", "Tonga", "Trinidad and Tobago",
            //"Tunisia", "Turkey", "Turkmenistan", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates",
            //"United Kingdom", "United States", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Vietnam",
            //"Yemen", "Zambia", "Zimbabwe"
            //};

            _base.Initialise(db);
            eServicesDbContext context = new eServicesDbContext();

            ViewBag.country = context.Countries.Where(a => a.IsActive).ToList();
            ViewBag.industry = context.Industries.Where(a => a.IsActive).ToList();

            //foreach (var country in countries)
            //{
            //    db.Countries.Add(new Country { Name = country, Description = country, IsActive = false, IsDeleted = true, IsLocked = true, Key = $"c_{country.ToLower()}" });
            //}
            //db.SaveChanges();

            //ViewBag.Countries = new SelectList(db.Countries.Where(a => a.IsActive), "Key", "Name");
            //ViewBag.Industries = new SelectList(db.Industries.Where(a => a.IsActive), "Id", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult OnboardigRequest(OnboardingRequest model, string IndustyKey, string CountryKey)
        {
            object results = new { status = false, Msg = "an error has occured" };
            //results = new { status = true, Msg = "an error has occured", email = email };
            try
            {
                using (eServicesDbContext context = new eServicesDbContext())
                {

                    OnboardingRequest request = new OnboardingRequest
                    {
                        CompanyName = model.CompanyName,
                        VatNumber = model.VatNumber,
                        IndustryId = context.Industries.Where(a => a.Key == IndustyKey).FirstOrDefault().Id,
                        CountryId = context.Countries.Where(a => a.Key == CountryKey).FirstOrDefault().Id,
                        CompanyDomain = model.CompanyDomain,
                        FistName = model.FistName,
                        LastName = model.LastName,
                        EmailAddress = model.EmailAddress,
                        ContactNumber = model.ContactNumber,
                        PostalCode = model.PostalCode,
                        PeriodTo = model.PeriodTo,
                        OperationalHours = model.OperationalHours,
                        OfficeAddress = model.OfficeAddress,
                        Title = model.Title,
                        Designation = model.Designation,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        CreatedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                        StatusId = context.Status.Where(a => a.Key == StatusKeys.OnboardingPending).FirstOrDefault().Id,
                    };

                    context.OnboardingRequests.Add(request);
                    context.SaveChanges();

                    Email _email = new Email();
                    _email.GenerateEmail(model.EmailAddress, "XET Help Desk Onboarding",
                                    "XET Solutions Help Desk login notification on " + String.Format("{0:F}", DateTime.Now),
                                    request.Id.ToString(CultureInfo.InvariantCulture), false, AppSettingKeys.EservicesDefaultEmailTemplate, $"{model.FistName} {model.LastName}");
                    //results = new { status = true, Msg = "an error has occured", email = model.EmailAddress };
                    
                    TempData["SuccessMessage"] = "User onboarded successfully";
                    return RedirectToAction("OnboardigRequest"); // Redirect to the same action to display the success message
                }

            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Failed to onboard user";
            }
            return RedirectToAction("OnboardigRequest"); // Redirect to the same action to display the error message
        }



        [Authorize]
        [DecryptParameter]
        public ActionResult Client(int requestId, string queueKey)
        {
            if (requestId == 0)
                throw new Exception("Invalid Client");

            OnboardingRequest onboardingRequest = db.OnboardingRequests
                .Include(c => c.Industry).Include(c => c.Country)
                .FirstOrDefault(a => a.Id == requestId);
            onboardingRequest.key = queueKey;

            ViewBag.country = new SelectList(db.Countries.Where(a => a.IsActive).ToList(), "Id", "Name");
            ViewBag.actionkeys = new SelectList(db.ActionTypes.Where(a => a.IsActive).ToList(), "Key", "Name");
            ViewBag.industry = new SelectList(db.Industries.Where(a => a.IsActive).ToList(), "Id", "IndustryName");
            return View(onboardingRequest);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Client(OnboardingRequest model, string approvalKey, string queueKey, int requestId = 0)
        {
            if (string.IsNullOrEmpty(approvalKey))
                throw new Exception(string.Format("Invalid user action - {0}", approvalKey));
            try
            {
                using (eServicesDbContext context = new eServicesDbContext())
                {
                    _base.Initialise(context);
                    switch (approvalKey)
                    {
                        case ActionTypeKeys.ApproveClientOnboarding:
                            Client aClient = aClientProfile(context, model);
                            SystemUser aUser = aClientAdministrator(context, model, aClient);
                            aRequestUpdate(context, model, aClient, aUser.Id, ActionTypeKeys.ApproveClientOnboarding);
                            break;
                        case ActionTypeKeys.DeclineClientOnboarding:
                            model = context.OnboardingRequests.Find(model.Id);
                            model.StatusId = context.Status.FirstOrDefault(c => c.Key == "s_client_declined_onboarding").Id;
                            context.Entry(model).State = EntityState.Modified;
                            context.SaveChanges();
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

            }


            OnboardingRequest onboardingRequest = db.OnboardingRequests
                .Include(c => c.Industry).Include(c => c.Country)
                .FirstOrDefault(a => a.Id == requestId);

            ViewBag.country = new SelectList(db.Countries.Where(a => a.IsActive).ToList(), "Id", "Name");
            ViewBag.industry = new SelectList(db.Industries.Where(a => a.IsActive).ToList(), "Id", "IndustryName");

            return RedirectToAction("Index", "Onboarding", new { q = new AesCrypto().Encrypt("queueKey=" + queueKey) });
        }

        public static SystemUser aClientAdministrator(eServicesDbContext context, OnboardingRequest model, Client aClient)
        {
            try
            {
                var email = model.EmailAddress;
                var username = email.Split('@').First();
                var phone = model.ContactNumber;
                phone = string.Format("0{0}", phone.Substring(phone.Length - 9));
                var user = new SystemIdentityUser { UserName = username, Email = email, PhoneNumber = phone, PhoneNumberConfirmed = false, EmailConfirmed = false };
                user = new SystemIdentityUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = false,
                    SystemUser = new SystemUser()
                    {
                        FirstName = model.FistName,
                        LastName = model.LastName,
                        UserName = username,
                        MobileNumber = phone,
                        EmailAddress = email,
                        IsPasswordReset = true,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        ModifiedDateTime = DateTime.Now,
                        ClientId = aClient.Id
                    }
                };
                var randomPassword = GeneratePassword(10);
                var result = UserManager.Create(user, randomPassword);

                switch (result.Succeeded)
                {
                    case true:
                        IdentityManager aManager = new IdentityManager();
                        _ = aManager.AddUserToRole(user.Id, "Administrators");

                        var _email = new Email();
                        var sms = new CesarSMS();
                        var statusIdsms = context.Status.FirstOrDefault(o => o.Key == StatusKeys.SMSPending).Id;
                        var systemUser = context.SystemUsers.FirstOrDefault(s => s.Id == user.SystemUserId);
                        var msgBody = "XET Help Desk Profile has been created on " + String.Format("{0:F}", DateTime.Now)
                            + "<br/>"
                            + "Here are the user credentials:"
                            + "Username: " + user.UserName
                            + "Password: " + randomPassword;

                        if (user.SystemUser != null)
                            _email.GenerateEmail(systemUser.EmailAddress, "XET Help Desk Profile",
                               msgBody, systemUser.Id.ToString(CultureInfo.InvariantCulture), false, AppSettingKeys.EservicesDefaultEmailTemplate, systemUser.FullName);
                        if (systemUser.MobileNumber != null)
                            sms.GenerateSMS(systemUser.MobileNumber, msgBody, systemUser.Id.ToString(CultureInfo.InvariantCulture), statusIdsms, systemUser.FullName);

                        break;
                    default:

                        break;
                }
                return user.SystemUser;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public static void aRequestUpdate(eServicesDbContext context, OnboardingRequest aRequest, Client aClient, int aUserId, string aStatusKey = ActionTypeKeys.ApproveClientOnboarding)
        {
            Status aStatus = new Status();
            aRequest = context.OnboardingRequests.Find(aRequest.Id);
            if (aStatusKey == ActionTypeKeys.ApproveClientOnboarding)
            {
                aRequest.IsActive = false;
                aRequest.IsDeleted = true;

                aClient.RepresentedByUserId = aUserId;
                context.Entry(aClient).State = EntityState.Modified;
                context.Entry(aRequest).State = EntityState.Modified;
                context.SaveChanges();
            }
            else
            {
                aRequest.IsActive = false;
                aRequest.IsDeleted = true;
                //aRequest.StatusId = aStatus.Id;
                context.Entry(aRequest).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        #region Generate User Password
        public static string GeneratePassword(int length)
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
        public static Client aClientProfile(eServicesDbContext context, OnboardingRequest model)
        {
            try
            {
                var email = model.EmailAddress;
                var username = email.Split('@').First();
               
                var user = new SystemIdentityUser
                {
                    UserName = username,
                    Email = email,
                    PhoneNumber = model.ContactNumber,
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    SystemUser = new SystemUser
                    {
                        FirstName = model.FistName,
                        LastName = model.LastName,
                        MobileNumber = model.ContactNumber,
                        EmailAddress = email,
                        IsPasswordReset = true,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        ModifiedDateTime = DateTime.Now
                    }
                };
                var randomPassword = GeneratePassword(10);

                var userManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(context));
                var result = userManager.Create(user, randomPassword);

                if (result.Succeeded)
                {
                    // Additional client creation logic
                    var client = new Client
                    {
                        Company = model.CompanyName,
                        IndustryId = model.IndustryId,
                        CountryId = model.CountryId,
                        VatNumber = model.VatNumber,
                        Domain = model.CompanyDomain,
                        StatusId = context.Status.FirstOrDefault(a => a.Key == StatusKeys.PendingUpdate).Id,
                        RepresentedByUserId = user.SystemUser.Id
                    };

                    context.Clients.Add(client);
                    context.SaveChanges();

                    // Email and SMS generation logic
                    var _email = new Email();
                    var sms = new CesarSMS();
                    var statusIdsms = context.Status.FirstOrDefault(o => o.Key == StatusKeys.SMSPending).Id;
                    var msgBody = "<br/>"
                        + "Details:"
                        + "Company: " + model.CompanyName
                        + "Vat Registration: " + model.VatNumber;

                    if (model.EmailAddress != null)
                        _email.GenerateEmail(model.EmailAddress, "XET Help Desk Onboarding Approval ",
                            msgBody, "1".ToString(CultureInfo.InvariantCulture), false, AppSettingKeys.EservicesDefaultEmailTemplate, string.Format("{0} {1}", model.FistName, model.LastName));

                    if (model.ContactNumber != null)
                        sms.GenerateSMS(model.ContactNumber, msgBody, "1".ToString(CultureInfo.InvariantCulture), statusIdsms, string.Format("{0} {1}", model.FistName, model.LastName));

                    // Additional logic after successful client creation and notifications
                    return client;
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }

            return null;
        }


        [DecryptParameter]
        public ActionResult Details(int Id)
        {
            try
            {
                Client _client = _context.Clients.Include(a => a.Country).Include(a => a.Industry).FirstOrDefault(c => c.Id == Id);
                SystemUser representative = _context.SystemUsers.Find(_client.RepresentedByUserId);
                SystemUser administrator = _context.SystemUsers.Find(_client.AdminByUserId);
                //SystemUser clientRepresentative = _context.SystemUsers.FirstOrDefault(x => x.EmailAddress == representative.EmailAddress);
                //SystemUser clientAdministrator = _context.SystemUsers.FirstOrDefault(x => x.EmailAddress == administrator.EmailAddress);

                _client.SystemUsers = _context.SystemUsers.Where(z => z.ClientId == Id).ToList();

                ViewBag.ClientInfoId = Id;
                ViewBag.ClientInfo = _client;
                ViewBag.ClientRep = representative;
                ViewBag.Administrator = administrator;
                ViewBag.ClientUserList = _client.SystemUsers;
                ViewBag.PossibleTitles = new List<string> { "Mr.", "Mrs.", "Miss", "Dr.", "Prof." };

                // Create ViewBag variables for administrator
               
                

                return View(_client);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult OnboardEntityUser(eServicesDbContext context, int entityId, string Firstname, string LastName, string Phone, string Email, string Title, string Designation)
        {
            try
            {
                var username = Email.Split('@').First();
                //var user = new SystemIdentityUser
                //{
                //    UserName = username,
                //    Email = Email,
                //    PhoneNumberConfirmed = true,
                //    EmailConfirmed = true
                //};
                var user = new SystemIdentityUser
                {
                    UserName = username,
                    Email = Email,
                    PhoneNumber = Phone, // Change to use the 'phone' parameter
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    SystemUser = new SystemUser
                    {
                        FirstName = Firstname, // Change to use the 'firstname' parameter
                        LastName = LastName,   // Change to use the 'lastname' parameter
                        EmailAddress = Email,
                        MobileNumber = Phone,   // Change to use the 'phone' parameter
                        Title = Title,
                        ClientId = entityId,
                        Designation = Designation,
                        IsPasswordReset = true,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        ModifiedDateTime = DateTime.Now,
                        CompanyName = context.Clients.Where(z => z.Id == entityId).Select(x => x.Company).ToString()
                    }
                };

                var randomPassword = GeneratePassword(10);

                var userManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(context));
                var result = userManager.Create(user, randomPassword);

                if (result.Succeeded)
                {
                    

                    // Email and SMS generation logic
                    var _email = new Email();
                    var sms = new CesarSMS();
                    var statusIdsms = context.Status.FirstOrDefault(o => o.Key == StatusKeys.SMSPending).Id;
                    var msgBody = "<br/>" + "Details:";

                    if (Email != null)
                    {
                        _email.GenerateEmail(Email, "XET Help Desk Onboarding Approval",
                            msgBody, "1", false, AppSettingKeys.EservicesDefaultEmailTemplate, string.Format("{0} {1}", Firstname, LastName));
                    }

                    if (Phone != null)
                    {
                        sms.GenerateSMS(Phone, msgBody, "1", statusIdsms, string.Format("{0} {1}", Firstname, LastName));
                    }

                    // Additional logic after successful user onboarding and notifications
                    return Json(new { status = true, message = "User onboarded successfully", email = Email });
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }

            return Json(new { status = false, message = "Failed to onboard user" });
        }



        public ActionResult OnboardEmployee()
        {

            using (eServicesDbContext context = new eServicesDbContext())
            {
                ViewBag.EmployeeTypes = context.EmployeeTypes.ToList();
                // Filter Designations based on the selected EmployeeType
                ViewBag.Designations = context.Designations.ToList();

                ViewBag.PossibleTitles = new List<string>
                    {
                    "Mr.", "Mrs.","Miss","Dr.","Prof."
                        // Add other titles as needed
                    };
                try
                {

                    return View();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public JsonResult GetDesignationsByEmployeeType(int employeeTypeId)
        {
            using (eServicesDbContext context = new eServicesDbContext())
            {
                var filteredDesignations = context.Designations
                    .Where(d => d.EmployeeTypeId == employeeTypeId)
                    .ToList();

                return Json(filteredDesignations, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult OnboardEmployee(eServicesDbContext context, string Firstname, string LastName, string Phone, string Email, string Title, int Designation)
        {

            var DesignationName = context.Designations.Where(x => x.Id == Designation).Select(x => x.Name).First();
            try
            {
                var username = Email.Split('@').First();

                var employee = new Employee
                {
                    DesignationId = Designation,
                    IsActive = true,
                    IsDeleted = false,
                    IsLocked = false,
                    ModifiedDateTime = DateTime.Now,
                };

                // Add the employee to the context and save changes
                context.Employees.Add(employee);
                context.SaveChanges();


                var user = new SystemIdentityUser
                {
                    UserName = username,
                    Email = Email,
                    PhoneNumber = Phone, // Change to use the 'phone' parameter
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    SystemUser = new SystemUser
                    {
                        FirstName = Firstname, // Change to use the 'firstname' parameter
                        LastName = LastName,   // Change to use the 'lastname' parameter
                        EmailAddress = Email,
                        MobileNumber = Phone,   // Change to use the 'phone' parameter
                        Title = Title,
                        //ClientId = entityId,
                        Designation = DesignationName,
                        EmployeeId = employee.Id,
                        CompanyName = "XETGROUP",
                        UserName = username,
                        IsPasswordReset = true,
                        IsActive = true,
                        IsDeleted = false,
                        IsLocked = false,
                        ModifiedDateTime = DateTime.Now,

                    }
                };

                var randomPassword = GeneratePassword(10);

                var userManager = new UserManager<SystemIdentityUser>(new UserStore<SystemIdentityUser>(context));
                var result = userManager.Create(user, randomPassword);

                if (result.Succeeded)
                {

                    // Email and SMS generation logic
                    var _email = new Email();
                    var sms = new CesarSMS();
                    var statusIdsms = context.Status.FirstOrDefault(o => o.Key == StatusKeys.SMSPending).Id;
                    var msgBody = "<br/>" + "Details:";

                    if (Email != null)
                    {
                        _email.GenerateEmail(Email, "XET Help Desk Onboarding Approval",
                            msgBody, "1", false, AppSettingKeys.EservicesDefaultEmailTemplate, string.Format("{0} {1}", Firstname, LastName));
                    }

                    if (Phone != null)
                    {
                        sms.GenerateSMS(Phone, msgBody, "1", statusIdsms, string.Format("{0} {1}", Firstname, LastName));
                    }

                    TempData["SuccessMessage"] = "User onboarded successfully"; // Store success message in TempData
                    return RedirectToAction("OnboardEmployee"); // Redirect to the same action to display the result message
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to onboard user"; // Store error message in TempData
                return RedirectToAction("OnboardEmployee"); // Redirect to the same action to display the result message
            }

            return RedirectToAction("OnboardEmployee"); // Redirect to the same action to display the result message
        }




        public ActionResult EmployeeIndex(eServicesDbContext context)
        {
            var onboardedEmployees = context.SystemUsers
                .Where(z => z.EmployeeId != null && z.Employee.IsActive)
                .ToList();

            ViewBag.OnboardedEmployees = onboardedEmployees; //call the viewbag in the tabel have fun
            return View();
        }





    }
}