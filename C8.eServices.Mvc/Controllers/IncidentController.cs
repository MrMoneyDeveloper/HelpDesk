using C8.eServices.Mvc.DataAccessLayer;
using C8.eServices.Mvc.Helpers;
using C8.eServices.Mvc.Keys;
using C8.eServices.Mvc.Models;
using C8.eServices.Mvc.Engines;
using C8.eServices.Mvc.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Policy;
using System.Text;
using System.Web.Mvc;
using System.Net.Http.Headers;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Numeric;
using System.Data.Entity.Infrastructure;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;


namespace C8.eServices.Mvc.Controllers
{
    public class IncidentController : Controller
    {

        private eServicesDbContext db = new eServicesDbContext();
        private eServicesDbContext core = new eServicesDbContext();
        private BaseHelper _baseHelper = new BaseHelper();

        #region
        GenericEntityBuilder<ServiceCategory> GenericServiceCategory;
        GenericEntityBuilder<Product> GenericProduct;
        GenericEntityBuilder<Sub_Catergory> GenericSubCatergory;
        GenericEntityBuilder<Incident> GenericIncident;

        public void InitialiseGenericEntityBuilder()
        {

            this.core = new eServicesDbContext();
            _baseHelper.Initialise(core);
            GenericServiceCategory = new GenericEntityBuilder<ServiceCategory>(core);
            GenericProduct = new GenericEntityBuilder<Product>(core);
            GenericSubCatergory = new GenericEntityBuilder<Sub_Catergory>(core);
            GenericIncident = new GenericEntityBuilder<Incident>(core);

        }
        #endregion




        // GET: HelpDeskTicket
        public ActionResult Index()
        {
            InitialiseGenericEntityBuilder();
            var listCat = GenericServiceCategory.GetActionsTypes<ServiceCategory>();
            //var singleItem = GenericServiceCategory.GetTypeByKey<ServiceCategory>(a => a.MMUID == 1).Key;
            return View(db.Incidents.Include(c => c.Status).Include(c => c.SubCategory).Include(c => c.CapturedByUser).Include(c => c.Category).Include(c => c.Product).Where(a => a.IsActive).ToList());
        }

            [AllowAnonymous]
        public ActionResult LogIncident()
        {

            ViewBag.PossibleTitles = new List<string>
            {
               "Mr.", "Mrs.","Miss","Dr.","Prof."
              // Add other titles as needed
            };

            try
            {
                BaseHelper _base = new BaseHelper();
                _base.Initialise(db);
                eServicesDbContext context = new eServicesDbContext();
                InitialiseGenericEntityBuilder();
                var user = _base.SystemUser;
                ViewBag.Name = user.FirstName;
                ViewBag.Surname = user.LastName;
                ViewBag.Email = user.EmailAddress;
                ViewBag.Mobile = user.MobileNumber;
                ViewBag.AllocatedProductList = new SelectList(db.AllocatedProducts.Where(a => a.ClientId == user.ClientId).ToList(), "Id", "Name");
                ViewBag.Categories = new SelectList(db.ServiceCategories.ToList(), "Id", "Name");
                ViewBag.CreatedBySystemUserId = new SelectList(db.SystemUsers, "Id", "FirstName");
                ViewBag.MMUID = new SelectList(db.Clients, "Id", "Name");
                ViewBag.ModifiedBySystemUserId = new SelectList(db.SystemUsers, "Id", "FirstName");
                ViewBag.Products = new SelectList(context.Products.Where(x => x.IsDeleted == false).ToList());

                // Populate ViewBag.country with a list of countries



                ViewBag.country = context.Countries.Where(a => a.IsActive).ToList();
                ViewBag.industry = context.Industries.Where(a => a.IsActive).ToList();

                return View();
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
            }

            return View(); // Return the view even if an exception occurs
        }

        [HttpPost]
            public ActionResult LogIncident(Incident model, int ProductId, int CategoryId, int SubCategoryId)
            {
                try
                {
                    InitialiseGenericEntityBuilder();
                    model.IsCallerTicket = false;
                    model.Reference = IncidentHelper.GetTicketReference(GenericSubCatergory.GetTypeByKey<Sub_Catergory>(a => a.Id == SubCategoryId).Key.Trim(), core);
                    model.CapturedByUserId = _baseHelper.SystemUser.Id;
                    model.StatusId = 1;
                    model.ProductId = ProductId;
                    model.CategoryId = CategoryId;
                    model.SubCategoryId = SubCategoryId;
                    GenericIncident.AddToTable(model);
                    GenericIncident.Complete();
                }
                catch
                {

                }
                return View("Index", "Home", GenericIncident.GetActionsTypes<Incident>());
            }

            public ActionResult getSubCategories(int id)
            {
                dynamic response = null;
                InitialiseGenericEntityBuilder();
                try
                {
                    response = GenericSubCatergory.GetSubCategories<Sub_Catergory>(a => a.ServiceCategoryId == id);
                }
                catch
                {

                }
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        public ActionResult CreateProduct(eServicesDbContext context)
        {
            // Calculate adminProductListWithFullName in memory
            var adminProductListWithFullName = (from p in context.Products
                                                join pa in context.ProductResources
                                                on p.Id equals pa.ProductId
                                                into paGroup
                                                from pa in paGroup.DefaultIfEmpty()
                                                join su in context.SystemUsers
                                                on pa.SystemUserId equals su.Id
                                                into suGroup
                                                from su in suGroup.DefaultIfEmpty()
                                                select new ViewHelperModel
                                                {
                                                    ProductId = p.Id,
                                                    ProductName = p.Name,
                                                    AdministratorId = su != null ? su.Id : 0,
                                                    AdministratorName = su != null ? su.FirstName + " " + su.LastName : null
                                                }).ToList();

            // Set ViewBag.AdminProductList to the calculated data
            ViewBag.AdminProductList = adminProductListWithFullName;

            // Fetch the list of products from your data source and set it in ViewBag.ProductList
            var productList = context.Products.Where(p => p.IsActive).ToList();
            ViewBag.ProductList = productList;

            // Fetch the list of clients from your data source and set it in ViewBag.ClientList
            var clientList = context.Clients.ToList();
            ViewBag.ClientList = clientList;

            // Create an empty Product
            Product emptyProduct = new Product();

            // Display TempData messages if they exist
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailureMessage = TempData["FailureMessage"];

            return View(emptyProduct);
        }



        private string GetAdministratorName(eServicesDbContext context, int? administratorId)
        {
            if (administratorId.HasValue)
            {
                var administrator = context.SystemUsers.FirstOrDefault(u => u.Id == administratorId);
                return administrator?.FullName;
            }
            return null;
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateProduct(Product product, eServicesDbContext context)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the Key is 3 characters long, and if so, add an underscore
                    if (product.Key != null && product.Key.Length == 3)
                    {
                        product.Key += "_";
                    }

                    // Set other properties for the Product
                    product.CreatedDateTime = DateTime.Now;
                    product.IsActive = true;
                    product.IsDeleted = false;
                    product.IsLocked = false;

                    // Add the product to the context and save changes
                    context.Products.Add(product);
                    context.SaveChanges();

                    // Set a success message using TempData
                    TempData["SuccessMessage"] = "Product created successfully.";

                    // Redirect to the GET action to display the success message and clear the form fields
                    return RedirectToAction("CreateProduct");
                }
                else
                {
                    // If ModelState is not valid, set a failure message using TempData
                    TempData["FailureMessage"] = "Product creation failed.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the product creation process
                // You can log the exception and show an error message to the user

                // Set a failure message using TempData
                TempData["FailureMessage"] = "Product creation failed.";
            }

            // Load EmployeeList and return the view with the ModelState
            ViewBag.EmployeeList = context.SystemUsers.Where(x => x.EmployeeId != null).ToList();
            return View(product);
        }




        [HttpPost]
        public ActionResult ProductAdmin(int[] SelectedEmployees, int[] SelectedProducts, eServicesDbContext context)
        {
            try
            {
                var currentUserName = User.Identity.Name;

                if (SelectedEmployees != null && SelectedProducts != null)
                {
                    // Fetch the SystemUser corresponding to the current user
                    var currentUser = context.SystemUsers.SingleOrDefault(u => u.UserName == currentUserName);

                    foreach (var employeeId in SelectedEmployees)
                    {
                        foreach (var productId in SelectedProducts)
                        {
                            // Create a ProductResource object for each selected employee and product
                            var ProductResource = new ProductResource
                            {
                                SystemUserId = employeeId,
                                ProductId = productId,
                                CreatedBySystemUser = currentUser,
                                Name = context.Products.Where(c => c.Id == productId).Select(c => c.Name).FirstOrDefault().ToString() + " Admin",
                                IsActive = true,
                                IsDeleted = false,
                                IsLocked = false,
                            };

                            // Add the ProductResource to the context
                            context.ProductResources.Add(ProductResource);
                        }
                    }

                    // Save changes to the database
                    context.SaveChanges();

                    // Set a success message using TempData
                    TempData["SuccessMessage"] = "Product administrators added successfully.";

                    // Redirect to the GET action to display the success message and clear the form fields
                    return RedirectToAction("CreateProduct");
                }
                else
                {
                    // If no employees or products were selected, set a failure message using TempData
                    TempData["FailureMessage"] = "Please select at least one employee and one product.";
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during the product administrator addition process
                // You can log the exception and show an error message to the user

                // Set a failure message using TempData
                TempData["FailureMessage"] = "Product administrator addition failed.";
            }

            // Load EmployeeList and ProductList and return the view with the ModelState
            ViewBag.EmployeeList = context.SystemUsers.Where(x => x.EmployeeId != null).ToList();
            ViewBag.ProductList = context.Products.Where(z => z.IsActive == true).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AllocateProduct(int[] SelectedClients, int[] SelectedProducts, eServicesDbContext context)
        {
            try
            {
                if (SelectedClients != null && SelectedProducts != null)
                {
                    foreach (var clientId in SelectedClients)
                    {
                        foreach (var productId in SelectedProducts)
                        {
                            // Create an AllocatedProduct object for each selected client and product
                            var allocatedProduct = new AllocatedProduct
                            {
                                ClientId = clientId,
                                ProductId = productId,
                                CreatedDateTime = DateTime.Now,
                                IsActive = true,
                                IsDeleted = false,
                                IsLocked = false,
                            };

                            // Add the allocatedProduct to the context
                            context.AllocatedProducts.Add(allocatedProduct);
                            context.SaveChanges();

                            // Save changes to the database


                            // Set a success message using TempData
                            TempData["SuccessMessage"] = "Products allocated successfully.";

                            // Redirect to the GET action to display the success message and clear the form fields
                            return RedirectToAction("CreateProduct");
                        }

                    }
 
                }
                else
                {
                    // If no clients or products were selected, set a failure message using TempData
                    TempData["FailureMessage"] = "Please select at least one client and one product.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception to a log file or display it for debugging
                Console.WriteLine(ex.ToString());
                // Handle any exceptions that might occur during the allocation process
                // You can log the exception and show an error message to the user

                // Set a failure message using TempData
                TempData["FailureMessage"] = "Product allocation failed.";
            }

            // Load ClientList and ProductList and return the view with the ModelState
            ViewBag.ClientList = context.Clients.Where(c => c.IsActive).ToList();
            ViewBag.ProductList = context.Products.Where(p => p.IsActive).ToList();
            return RedirectToAction("CreateProduct");
        }
        public ActionResult CreateServiceCatergory(eServicesDbContext context, ServiceCategory serviceCategory)
        {
            var ServiceCatergories = context.ServiceCategories.Where(c => c.IsActive == true).ToList();
            var priorities = context.Priorities.Where(p => p.IsActive).ToList();

            // Display TempData messages if they exist
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailureMessage = TempData["FailureMessage"];
            ViewBag.ServiceCategory = ServiceCatergories;
            ViewBag.PriorityDropdown = new SelectList(priorities, "Id", "Name"); // Create a dropdown list for priorities
            return View();
        }

        [HttpPost]
        public ActionResult SubmitServiceCategory(eServicesDbContext context, string categoryName)
        {
            
                try
                {
                        var serviceCategory = new ServiceCategory
                        {
                            Name = categoryName, // Use the value passed from the form
                            CreatedDateTime = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false,
                            IsLocked = false
                        };

                        context.ServiceCategories.Add(serviceCategory);
                        context.SaveChanges();

                        TempData["SuccessMessage"] = "Service Category created successfully.";

                    return RedirectToAction("CreateServiceCatergory"); // Redirect to a success page or another appropriate action

                }
                catch (Exception ex)
                {
                    TempData["FailureMessage"] = "Failed to create the Service Category: " + ex.Message;
                }
            

            // Return to the CreateServiceCatergory view if there's an error
            return View("CreateServiceCatergory");
        }

        [HttpPost]
        public ActionResult SubmitSelectedServiceCategories(eServicesDbContext context, List<int?> selectedServiceCategoryIds)
        {
            try
            {
                // Assuming you have a DbSet for Sub_Catergory in your DbContext
                var subcategories = context.Sub_Catergories
                    .Where(s => selectedServiceCategoryIds.Contains(s.ServiceCategoryId))
                    .ToList();

                // Create a list of anonymous objects with the properties you want to return
                var subcategoryList = subcategories.Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    // Add more properties as needed
                }).ToList();

                return Json(subcategoryList);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpPost]
      
        public ActionResult CreateSubCategory(eServicesDbContext context, Sub_Catergory subCategory, int priorityId, string subcategoryName, int slaValue, int? selectedServiceCategoryId)
        {
            try
            {
                // Set the priority based on the selected value from the dropdown
                subCategory.PriorityId = priorityId;

                // Set other properties based on the data passed from JavaScript
                subCategory.Name = subcategoryName;
                subCategory.SLA_Value = slaValue;
                subCategory.ServiceCategoryId = selectedServiceCategoryId; // Set the service category ID

                // Assuming you're receiving the 'subCategory' object with all the necessary fields filled in the view.
                // Add the new subcategory to your database context and save changes
                context.Sub_Catergories.Add(subCategory);
                context.SaveChanges();

                TempData["SuccessMessage"] = "Subcategory created successfully.";
            }
            catch (Exception ex)
            {
                TempData["FailureMessage"] = "Failed to create subcategory: " + ex.Message;
            }

            // Redirect back to the view that displays service categories and subcategories
            return RedirectToAction("CreateServiceCategory");
        }


        public ActionResult CreatePriority(eServicesDbContext context, Priority priority)
        {
      

            return View();
        }

        [HttpPost]
        public ActionResult PriorityCreation(Priority priority)
        {
            if (ModelState.IsValid)
            {

                
                try
                {
                    using (eServicesDbContext context = new eServicesDbContext())
                    {
                        BaseHelper _base = new BaseHelper();
                        _base.Initialise(context);
                        //Priority priorities = new Priority();
                        //priorities.MinTime = priority.MinTime;
                        //priorities.MaxTime = priority.MaxTime;
                        //priorities.Name = priority.Name;
                        //priorities.Description = priority.Name;
                        //priority.Id = 1;
                        priority.Key = String.Format("p_{0}", priority.Name.ToLower().Replace(" ", " "));
                        context.Priorities.Add(priority);
                        context.SaveChanges();

                        TempData["SuccessMessage"] = "Priority created successfully"; // Success message

                        // Redirect to a different action after successful submission
                        return RedirectToAction("CreatePriority"); // Replace "Index" and "PriorityController" with your desired action and controller
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Log the exception details
                    System.Diagnostics.Trace.TraceError("DbUpdateException: " + ex.Message);
                    System.Diagnostics.Trace.TraceError("Stack Trace: " + ex.StackTrace);

                    if (ex.InnerException != null)
                    {
                        System.Diagnostics.Trace.TraceError("Inner Exception: " + ex.InnerException.Message);
                        System.Diagnostics.Trace.TraceError("Inner Exception Stack Trace: " + ex.InnerException.StackTrace);
                    }

                    TempData["FailureMessage"] = "Failed to create priority. See the logs for details.";
                    return View("CreatePriority");
                }
            }

            // If the model state is not valid, return the view with validation errors
            return View("CreatePriority");
        }



    }
}


