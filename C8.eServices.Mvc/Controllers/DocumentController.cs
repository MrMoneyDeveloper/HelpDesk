using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using C8.eServices.Mvc.DataAccessLayer;
using C8.eServices.Mvc.Helpers;
using C8.eServices.Mvc.Keys;
using C8.eServices.Mvc.Models;
using C8.eServices.Mvc.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace C8.eServices.Mvc.Controllers
{
    public class DocumentController : Controller
    {
        //private eServicesDbContext _context = new eServicesDbContext();
        private BaseHelper _base = new BaseHelper();
        public DocumentController()
        {
            eServicesDbContext _context = new eServicesDbContext();
            IdentityManager = new IdentityManager(_context);
            UserManager =
                new UserManager<SystemIdentityUser>(
                new UserStore<SystemIdentityUser>(_context));
        }

        public IdentityManager IdentityManager { get; set; }
        public UserManager<SystemIdentityUser> UserManager { get; private set; }

        private int SystemUserId = -1;
        //
        // GET: /Document/
        [EncryptedActionParameter]
        [Authorize(Roles = "Administrators" + "," + "Customers" + "," + "Super Administrators" + "," + "Clerks")]
        public ActionResult Index(int? referenceId, int? customerId, int? referenceTypeId, int? applicationId, int? agentId, string returnUrl, int? ratesRebateId, int? incentivePolicyId, string errorList = null)
        {
            eServicesDbContext _context = new eServicesDbContext();
            if (referenceId == null || referenceTypeId == null || applicationId == null)
                return HttpNotFound();
            _base.Initialise(_context);
            var agent = _base.Agent;

            if (User != null && User.Identity.IsAuthenticated)
            {
                IdentityManager.CurrentUser(User);
                SystemUserId = IdentityManager.CurrentUser(User).Id;
            }

            var referenceType = _context.ReferenceTypes.Find(referenceTypeId);
            var application = _context.Applications.Find(applicationId);

            if (application == null) throw new Exception("Invalid application.");
            if (referenceType == null) throw new Exception("Invalid reference type.");

            //var systemUser = new IdentityManager().CurrentUser( User );
            //var customer = _context.Customers.SingleOrDefault( o => o.SystemUserId == systemUser.Id );

            // Always defaults ID document to application. 
            var idDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.IdDocument);
            var documentCheckLists = new List<DocumentCheckList>();
            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == idDocumentType.Id && dcl.ReferenceTypeId == referenceTypeId));

            // Gets the document types needed for user selection.
            //var ckDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.CkDocument);
            //var cipcDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.CipcDocument);
            //var baaDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.AuthorithyBlackAdministrationAct);
            //var partnersDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.PartnershipAgreement);
            //var ptoDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.PtoCertificate);
            //var executorDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.Executorship);
            var authorityDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.Authority);


            // If there are additional documents need for an application.
            var addDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.AdditionalDocuments);

            var ratesRebateProperty = new RatesRebateProperty();
            var incentivePolicyProperty = new IncentivePolicyProperty();
            var dvm = new DocumentsViewModel
            {
                ReturnUrl = returnUrl,
                // CustomerId = (agentId == null) ? customerId : referenceId,
                CustomerId = customerId ?? referenceId,
                ApplicationId = (int)applicationId,
                Application = application,
                ReferenceTypeId = (int)referenceTypeId,
                ReferenceType = referenceType,
                ReferenceId = (int)referenceId,
                IsUploadView = true,
                Documents = _context.Documents.Include(o => o.File).Where(o => o.ReferenceId == referenceId && o.ReferenceTypeId == referenceTypeId && o.IsActive && !o.IsDeleted).ToList()
            };

            // Checks the reference type of the documents needed.
            switch (referenceType.Key)
            {
                case ReferenceTypeKeys.LinkedAccounts:
                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == authorityDocumentType.Id && dcl.ReferenceTypeId == referenceTypeId));
                    break;
                case ReferenceTypeKeys.LinkedAccountsManagingAgent:
                    documentCheckLists.Clear();
                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == authorityDocumentType.Id && dcl.ReferenceTypeId == referenceTypeId ));
                    break;
            }

            var addDoc = _context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == addDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id);
            if (documentCheckLists.All(chk => chk.Id != addDoc.Id))
            {
                documentCheckLists.Add(addDoc);
            }
            //documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == addDocumentType.Id && dcl.ReferenceTypeId == referenceTypeId && dcl.ApplicationId == applicationId));
            dvm.DocumentCheckLists = documentCheckLists;

            if (application.Key == ApplicationKeys.RatesRebate)
            {
                ViewBag.RatesRebateStatus = ratesRebateProperty.RatesRebate.Status.Key;
                ViewBag.PropertyId = ratesRebateProperty.PropertyId;

                var nav = new NavigationProperty
                {
                    CustomerId = ratesRebateProperty.RatesRebate.OwnerCustomerId,
                    PropertyId = ratesRebateProperty == null ? 0 : ratesRebateProperty.PropertyId,
                    RatesRebateId = ratesRebateProperty.RatesRebate.Id,
                    AgentId = agent == null ? 0 : agent.Id,
                    ApplicationId = application.Id,
                    ReferenceTypeId = referenceType.Id,
                    ReferenceId = referenceId,
                    Step = ViewCodeKeys.StepFive
                };

                ViewBag.NavigationParameters = nav;
            }
 
            foreach (var customerDocument in dvm.Documents)
            {
                if (SystemUserId != -1 && !SecurityHelper.VerifySystemUserOwnership(SystemUserId, customerDocument, ViewCodeKeys.UpdateCustomerDocuments))
                {
                    SecurityHelper.LogError(new Exception("Malicious Activity"), null);
                    return RedirectToAction("Index", "Error");
                }
                if (customerDocument.File != null)
                    customerDocument.File.Data = SecureActionLinkExtension.Encrypt(string.Format("fileId={0}", customerDocument.FileId));

                var docCheckList =
                     _context.DocumentCheckLists.Include(d => d.DocumentType)
                         .FirstOrDefault(c => c.Id == customerDocument.DocumentCheckListId && c.IsActive && !c.IsDeleted);
                customerDocument.DocumentLocation = string.Format("uploads/{0}/{1}", docCheckList.DocumentType.Name,
                    customerDocument.DocumentName);
                    //DocumentHelper.GetDocumentAbsoluteUrl(customerDocument.DocumentLocation);
                    //SecureActionLinkExtension.Encrypt(string.Format("generatedFileLocation={0}", DocumentHelper.GetDocumentAbsoluteUrl(customerDocument.DocumentLocation)));
                customerDocument.Data = HttpUtility.UrlEncode(SecureActionLinkExtension.Encrypt(string.Format("documentId={0}", customerDocument.Id)));
            }

            foreach (var documentCheckList in dvm.DocumentCheckLists.Where(dcl => dcl != null))
            {

                documentCheckList.DataList = new List<string>();
                documentCheckList.DataList.Add(HttpUtility.UrlEncode(SecureActionLinkExtension.Encrypt(string.Format("documentCheckListId={0}??referenceTypeId={1}??referenceId={2}??customerId={3}??applicationId={4}", documentCheckList.Id, documentCheckList.ReferenceTypeId, referenceId, customerId, applicationId))));
            }
            var CustType = _context.Customers.Where(o=>o.CreatedBySystemUserId==SystemUserId).FirstOrDefault();
            ViewBag.Application = application.Key;
            ViewBag.ReferenceId = referenceId;
            ViewBag.ApplicationId = applicationId;
            ViewBag.ReferenceTypeId = referenceTypeId;
            ViewBag.CustomerId = customerId;
            ViewBag.IncentivePolicyId = incentivePolicyId;
            ViewBag.Data = SecureActionLinkExtension.Encrypt(string.Format("incentivePolicyId={0}", incentivePolicyId));
            ViewBag.CustomerType = CustType.CustomerTypeId;
            return View(dvm);
        }

        /// <summary>
        /// Gets Document checklists for an Application
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="referenceTypeKey"></param>
        /// <returns></returns>
        public List<DocumentCheckList> GetDocumentCheckLists(object obj, string referenceTypeKey)
        {
            var _context = new eServicesDbContext();
            var documentCheckLists = new List<DocumentCheckList>();

            var referenceType =
             _context.ReferenceTypes.FirstOrDefault(r => r.Key == referenceTypeKey);
            //var referenceType =
            //  _context.ReferenceTypes.FirstOrDefault(r => r.Key == ReferenceTypeKeys.RatesRebateProperty);
            if (referenceType == null) throw new Exception(string.Format("Invalid/ missing reference type key - {0}", ReferenceTypeKeys.RatesRebateProperty));

            // Always defaults ID document to application. 
            var idDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.IdDocument);          
            // If there are additional documents need for an application.
            var addDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.AdditionalDocuments);

            // Gets the document types needed for user selection.
            var ckDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.CkDocument);
            var cipcDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.CipcDocument);
            var baaDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.AuthorithyBlackAdministrationAct);
            var partnersDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.PartnershipAgreement);
            var ptoDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.PtoCertificate);
            var authorityDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.Authority);
            var executorDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.Executorship);

            var brDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.BuildingSpecialistReport);
            var raDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.CopyOfRatesAccount);
            var mbDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.CopyMunicipalBill);
            var lgsDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.LetterOfGoodStanding);
            var taxClearanceDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.TaxClearanceCertificate);
            var varRegDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.VatRegistrationCertificate);
            var bpDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.BuildingPlan);
            var costDocumentType = _context.DocumentTypes.SingleOrDefault(dt => dt.Key == DocumentTypeKeys.MilestoneAndCostSchedule);

            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == idDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
            switch (referenceTypeKey)
            {
                case ReferenceTypeKeys.RatesRebateProperty:
                    var ratesRebateProperty = (RatesRebateProperty)obj;
                    var applicantCustomer = _context.Customers.Include(c => c.CustomerType).FirstOrDefault(c => c.Id == ratesRebateProperty.RatesRebate.ApplicantCustomerId);
                    if (applicantCustomer == null) throw new Exception("Missing Applicant Customer");

                    // Checks if application was done as a thrid party.
                    if (!ratesRebateProperty.RatesRebate.InOwnCapacity || applicantCustomer.CustomerType.Key == CustomerTypeKeys.ManagingAgent)
                        documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == authorityDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));

                    // Checks if application was done as an executor.
                    if (ratesRebateProperty.RatesRebate.ExecutorId > 0 || ratesRebateProperty.RatesRebate.ExecutorCustomerId > 0)
                        documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == executorDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));

                    var customerType = _context.CustomerTypes.FirstOrDefault(ct => ct.Id == ratesRebateProperty.Customer.CustomerTypeId && ct.IsActive && !ct.IsDeleted);
                    if (null == customerType) throw new Exception("Invalid or no customer type.");

                    // Builds up the document check list based on the user selections in the application.
                    switch (customerType.Key)
                    {
                        case CustomerTypeKeys.DeemedOwner:
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == baaDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            break;
                        case CustomerTypeKeys.Individual:
                            break;
                        case CustomerTypeKeys.PTO:
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == ptoDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            break;
                        case CustomerTypeKeys.Entity:
                            var entity = _context.Entities.FirstOrDefault(e => e.CustomerId == ratesRebateProperty.Customer.Id);
                            var entityType = _context.EntityTypes.FirstOrDefault(et => et.Id == entity.EntityTypeId);
                            if (entityType == null) throw new Exception("Invalid or no entity type.");

                            // Checks what entity the application pertains to and selects the relevant document.
                            switch (entityType.Key)
                            {
                                case EntityTypeKey.CloseCorporation:
                                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == ckDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                                    break;
                                case EntityTypeKey.Company:
                                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == cipcDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                                    break;
                                case EntityTypeKey.Organisation:
                                    break;
                                case EntityTypeKey.Partnership:
                                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == partnersDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                                    break;
                                case EntityTypeKey.SoleProprietory:
                                    break;
                            }
                            break;
                    }
                    break;
                case ReferenceTypeKeys.IncentivePolicyProperty:
                    referenceType =
                        _context.ReferenceTypes.FirstOrDefault(r => r.Key == ReferenceTypeKeys.IncentivePolicyProperty);
                    if (referenceType == null) throw new Exception(string.Format("Invalid/ missing reference type key - {0}", ReferenceTypeKeys.RatesRebateProperty));

                    var incentivePolicyProperty = (IncentivePolicyProperty)obj;

                    customerType = _context.CustomerTypes.FirstOrDefault(ct => ct.Id == incentivePolicyProperty.IncentivePolicy.Customer.CustomerTypeId && ct.IsActive && !ct.IsDeleted);
                    if (null == customerType) throw new Exception("Invalid or no customer type.");

                    // Builds up the document check list based on the user selections in the application.
                    switch (customerType.Key)
                    {
                        case CustomerTypeKeys.DeemedOwner:
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == baaDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            break;
                        case CustomerTypeKeys.Individual:
                            break;
                        case CustomerTypeKeys.PTO:
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == ptoDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            break;
                        case CustomerTypeKeys.Entity:
                            var entity = _context.Entities.FirstOrDefault(e => e.CustomerId == incentivePolicyProperty.IncentivePolicy.Customer.Id);
                            var entityType = _context.EntityTypes.FirstOrDefault(et => et.Id == entity.EntityTypeId);
                            if (entityType == null) throw new Exception("Invalid or no entity type.");

                            //Add Incentive Policy Documents
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == raDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == lgsDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == taxClearanceDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == varRegDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == bpDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == costDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == mbDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == brDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                            // Checks what entity the application pertains to and selects the relevant document.
                            switch (entityType.Key)
                            {
                                case EntityTypeKey.CloseCorporation:
                                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == ckDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                                    break;
                                case EntityTypeKey.Company:
                                    documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == cipcDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                                    break;
                                case EntityTypeKey.Organisation:
                                    break;
                                case EntityTypeKey.Partnership:
                                   // documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == partnersDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));
                                    break;
                                case EntityTypeKey.SoleProprietory:
                                    break;
                            }
                            break;
                    }
                    break;                 
            }

            documentCheckLists.Add(_context.DocumentCheckLists.Include(dcl => dcl.DocumentType).SingleOrDefault(dcl => dcl.DocumentTypeId == addDocumentType.Id && dcl.ReferenceTypeId == referenceType.Id));

            return documentCheckLists;
        }

        /// <summary>
        /// Renders the document details.
        /// </summary>
        /// <param name="documentCheckListId">The document check list identifier.</param>
        /// <param name="referenceTypeId">The reference type identifier.</param>
        /// <param name="referenceId">The reference identifier.</param>
        /// <param name="customerId">The customer identifier.</param>
        /// <param name="applicationId">The application identifier.</param>
        /// <returns></returns>
        [EncryptedActionParameter]
        [Authorize(Roles = "Administrators" + "," + "Customers" + "," + "Super Administrators" + "," + "Clerks")]
        public ActionResult RenderDocumentDetails(int documentCheckListId, int referenceTypeId, int referenceId, int customerId, int applicationId)
        {
            eServicesDbContext _context = new eServicesDbContext();
            List<Document> docs =
            _context.Documents.Include(d => d.File)
            .Include(d => d.LocationType)
            .Include(d => d.DocumentCheckList)
            .Include(d=> d.DocumentCheckList.DocumentType)
            .Include(d => d.DocumentCheckList.Application).Where(
               d =>
                d.DocumentCheckListId == documentCheckListId && d.ReferenceTypeId == referenceTypeId &&
                d.ReferenceId == referenceId && d.CustomerId == customerId &&
                d.DocumentCheckList.ApplicationId == applicationId && d.IsActive && !d.IsDeleted).ToList();

            foreach (var doc in docs)
            {
                doc.DocumentLocation = string.Format("uploads/{0}/{1}", doc.DocumentCheckList.DocumentType.Name,doc.DocumentName);
                if (doc.LocationType.Key == LocationTypeKeys.Database) 
                    doc.File.Data = SecureActionLinkExtension.Encrypt(string.Format("fileId={0}", doc.FileId));

                doc.Data = HttpUtility.UrlEncode(SecureActionLinkExtension.Encrypt(string.Format("documentId={0}", doc.Id)));
            }

            var obj = new
            {
                status = docs != null && docs.Count > 0 ? "Success" : "Failure",
                view = RenderHelper.PartialView(this, "_DocumentDetailsPartial", docs)
            };

            return Json(obj);
        }
    }
}