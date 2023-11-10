using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using C8.eServices.Mvc.Controllers;
using C8.eServices.Mvc.Helpers;
using C8.eServices.Mvc.Keys;
using C8.eServices.Mvc.Models;
using C8.eServices.Mvc.Models.Audits;
using C8.eServices.Mvc.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using EntityType = C8.eServices.Mvc.Models.EntityType;

namespace C8.eServices.Mvc.DataAccessLayer
{
    public class eServicesDbContext : IdentityDbContext<SystemIdentityUser>
    {
        public eServicesDbContext()
            : base("eServicesDbContext")
        {
            // TODO: JK.20160801a - Improve database availability check.
            //DbInterception.Add(new DbConextCommandInterceptor() );
        }

        public eServicesDbContext(SystemUser currentSystemUser)
            : base("eServicesDbContext")
        {
            CurrentSystemUser = currentSystemUser;
            // TODO: JK.20160801a - Improve database availability check.
            //DbInterception.Add( new DbConextCommandInterceptor() );
        }

        public SystemUser CurrentSystemUser { get; set; }

        // Entity Collections.
        public DbSet<Audit> Audits { get; set; }
        //public DbSet<Account> Accounts { get; set; }
        //public DbSet<AccountInformation> AccountInformations { get; set; }
        //public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<AgentAudit> AgentAudits { get; set; }
        public DbSet<AgentCustomer> AgentCustomers { get; set; }
        public DbSet<AgentCustomerAudit> AgentCustomerAudits { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationAudit> ApplicationAudits { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationRoleAudit> ApplicationRoleAudits { get; set; }
        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }
        public DbSet<ApplicationUserRoleAudit> ApplicationUserRoleAudits { get; set; }
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<AppSettingAudit> AppSettingAudits { get; set; }
        //public DbSet<Bank> Banks { get; set; }
        //public DbSet<BankAccount> BankAccounts { get; set; }
        //public DbSet<BankAccountType> BankAccountTypes { get; set; }
        //public DbSet<BankCheck> BankChecks { get; set; }
        //public DbSet<BillDirectory> BillDirectories { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<CategoryTypeAudit> CategoryTypeAudits { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentAudit> CommentAudits { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryAudit> CountryAudits { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerAccount> CustomerAccounts { get; set; }
        public DbSet<CustomerAudit> CustomerAudits { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<CustomerTypeAudit> CustomerTypeAudits { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentAudit> DocumentAudits { get; set; }
        public DbSet<DocumentCheckList> DocumentCheckLists { get; set; }
        public DbSet<DocumentCheckListAudit> DocumentCheckListAudits { get; set; }
        public DbSet<DocumentReference> DocumentReferences { get; set; }
        public DbSet<DocumentReferenceAudit> DocumentReferenceAudits { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentTypeAudit> DocumentTypeAudits { get; set; }
        //public DbSet<EmisCase> EmisCases { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<EntityAgent> EntityAgents { get; set; }
        public DbSet<EntityAgentAudit> EntityAgentAudits { get; set; }
        public DbSet<EntityAudit> EntityAudits { get; set; }
        public DbSet<EntityType> EntityTypes { get; set; }
        public DbSet<EntityTypeAudit> EntityTypeAudits { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<IdentificationType> IdentificationTypes { get; set; }
        public DbSet<IdentificationTypeAudit> IdentificationTypeAudits { get; set; }
        public DbSet<LinkedEmail> LinkedEmails { get; set; }
        public DbSet<LinkedMobile> LinkedMobiles { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<LocationTypeAudit> LocationTypeAudits { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<LogAudit> LogAudits { get; set; }
        public DbSet<LogType> LogTypes { get; set; }
        public DbSet<LogTypeAudit> LogTypeAudits { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteAudit> NoteAudits { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }
        public DbSet<NoteTypeAudit> NoteTypeAudits { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<NotificationAudit> NotificationAudits { get; set; }
        public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }
        public DbSet<NotificationSubscriptionAudit> NotificationSubscriptionAudits { get; set; }
        public DbSet<NotificationTypeAudit> NotificationTypeAudits { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<QueryAudit> QueryAudits { get; set; }
        public DbSet<QueryType> QueryTypes { get; set; }
        public DbSet<QueryTypeAudit> QueryTypeAudits { get; set; }
        //public DbSet<RecipientTypeAudit> RecipientTypeAudits { get; set; }
        //public DbSet<ReferenceType> ReferenceTypes { get; set; }
        //public DbSet<ReferenceTypeAudit> ReferenceTypeAudits { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestAudit> RequestAudits { get; set; }
        public DbSet<ReferenceType> ReferenceTypes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusAudit> StatusAudits { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<StatusTypeAudit> StatusTypeAudits { get; set; }
        public DbSet<Support> SupportQueries { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }
        public DbSet<SystemUserAudit> SystemUserAudits { get; set; }
        public DbSet<SystemUserException> SystemUserException { get; set; }
        public DbSet<SystemUserLogTime> SystemUserLogTimes { get; set; }
        public DbSet<SystemUserType> SystemUserTypes { get; set; }
        public DbSet<SystemUserTypeAudit> SystemUserTypeAudits { get; set; }
        public DbSet<TitleType> TitleTypes { get; set; }
        public DbSet<TitleTypeAudit> TitleTypeAudits { get; set; }






        public DbSet<OnboardingRequest> OnboardingRequests { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<EntityUser> EntityUsers { get; set; }
        public DbSet<OnboardCleintUserViewModel> OnboardCleintUserViewModels { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Sub_Catergory> Sub_Catergories { get; set; }
        public DbSet<FaultCode> FaultCodes { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AllocatedProduct> AllocatedProducts { get; set; }
        public DbSet<ProductResource> ProductResources { get; set; }
        public DbSet<IncidentCaller> IncidentCallers { get; set; }
        public DbSet<PriorityIncidentUser> PriorityIncidentUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeType> EmployeeTypes { get; set; }
        public DbSet<Designation> Designations { get; set; }
       

        public override int SaveChanges()
        {
            var changeCount = 0;
            string entityState = string.Empty;

            try
            {
                var transactionDateTime = DateTime.Now;
                var changeSet = ChangeTracker.Entries();
                int? currentUserId = CurrentSystemUser != null ? CurrentSystemUser.Id : (int?)null;
                string tableName = string.Empty;
                List<AuditEnitity> singleTableAudit = new List<AuditEnitity>();
                List<AuditEnitity> perTableAudits = new List<AuditEnitity>();
                this.Database.CommandTimeout = 180;
                
                foreach (var entry in changeSet.Where(e => e.Entity is IAuditable))
                {
                    Type entryEntityType = entry.Entity.GetType();
                    string primaryKeyName = GetPrimaryKeyProperty(entryEntityType);
                    int primaryKey = Convert.ToInt32(entryEntityType.GetProperty(primaryKeyName).GetValue(entry.Entity));
                    entityState = entry.State.ToString();
                    tableName = GetTableName(entryEntityType);

                    ((IAuditable)entry.Entity).ModifiedBySystemUserId = currentUserId;
                    ((IAuditable)entry.Entity).ModifiedDateTime = transactionDateTime;

                    switch (entry.State)
                    {
                        case EntityState.Added:

                            ((IAuditable)entry.Entity).CreatedBySystemUserId = currentUserId;
                            ((IAuditable)entry.Entity).CreatedDateTime = transactionDateTime;
                            ((BaseModel)entry.Entity).IsActive = true;
                            ((BaseModel)entry.Entity).IsDeleted = false;
                            ((BaseModel)entry.Entity).IsLocked = false;

                            foreach (var propertyName in entry.CurrentValues.PropertyNames)
                            {
                                singleTableAudit.Add(new AuditEnitity()
                                {
                                    Audit = new Audit()
                                    {
                                        Action = entityState,
                                        TableName = tableName,
                                        PrimaryKey = primaryKey,
                                        ColumnName = propertyName,
                                        CurrentValue = Convert.ToString(entry.CurrentValues[propertyName]),
                                        OriginalValue = string.Empty,
                                        AuditBySystemUserId = currentUserId,
                                        AuditDateTime = transactionDateTime,

                                    },
                                    Entity = entry.Entity
                                });
                            }

                            perTableAudits.Add(new AuditEnitity()
                            {
                                Audit = new Audit()
                                {
                                    Action = entityState
                                },
                                Entity = entry.Entity
                            });

                            break;
                        case EntityState.Modified:
                        case EntityState.Deleted:
                            ((IAuditable)entry.Entity).CreatedBySystemUserId = ((IAuditable)entry.Entity).CreatedBySystemUserId;
                            ((IAuditable) entry.Entity).CreatedDateTime = ((IAuditable) entry.Entity).CreatedDateTime; 

                            foreach (var propertyName in entry.CurrentValues.PropertyNames)
                            {
                                if ((entry.CurrentValues[propertyName] != null && entry.GetDatabaseValues()[propertyName] != null) && !entry.CurrentValues[propertyName].Equals(entry.GetDatabaseValues()[propertyName]))
                                    singleTableAudit.Add(new AuditEnitity()
                                    {
                                        Audit = new Audit()
                                        {
                                            Action = entityState,
                                            TableName = tableName,
                                            PrimaryKey = primaryKey,
                                            ColumnName = propertyName,
                                            CurrentValue = Convert.ToString(entry.CurrentValues[propertyName]),
                                            OriginalValue = Convert.ToString(entry.OriginalValues[propertyName]),
                                            AuditBySystemUserId = currentUserId,
                                            AuditDateTime = transactionDateTime
                                        },
                                        Entity = entry.Entity
                                    });
                            }

                            perTableAudits.Add(new AuditEnitity()
                            {
                                Audit = new Audit()
                                {
                                    Action = entityState
                                },
                                Entity = entry.Entity
                            });

                            break;
                    }
                }

                // var changeCount = base.SaveChanges();
                changeCount = base.SaveChanges();

                // JK.20140902a - Auditing is processed here.
                foreach (var auditEnitity in singleTableAudit)
                {
                    try
                    {
                        if (auditEnitity.Entity != null)
                        {
                            Type entryEntityType = auditEnitity.Entity.GetType();
                            string primaryKeyName = GetPrimaryKeyProperty(entryEntityType);
                            int primaryKey =
                                Convert.ToInt32(entryEntityType.GetProperty(primaryKeyName).GetValue(auditEnitity.Entity));

                            auditEnitity.Audit.PrimaryKey = primaryKey;
                        }

                        // Single table audit.
                        Audits.Add(auditEnitity.Audit);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                   
                }

                foreach (var e in perTableAudits)
                {
                    SaveAudit(e);
                }

                //Save audit trail.
                base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    string entityName = validationErrors.Entry.Entity.GetType().Name;
                    string controller = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("controller");
                    string action = HttpContext.Current.Request.RequestContext.RouteData.GetRequiredString("action");

                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        //Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        //EventLogHelper.Log(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                        SecurityHelper.LogError(new Exception(string.Format("Entity Name: {0} State: {1} Controller: {2} Action: {3} Property: {4} Error: {5}", entityName, entityState,
                            controller, action, validationError.PropertyName, validationError.ErrorMessage)), null);
                    }
                }
            }
            return changeCount;
        }

        /// <summary>
        /// Saves the audits for per table auditing.
        /// </summary>
        /// <param name="entry">The entry.</param>
        private void SaveAudit(AuditEnitity entry)
        {
            try
            {
                var name = entry.Entity.GetType().Name;
                var space = entry.Entity.GetType().Namespace;
                var type = Type.GetType(string.Format("{0}.Audits.{1}Audit", space, name));

                object audit = null;

                if (type != null)
                {
                    audit = Activator.CreateInstance(type);

                    var props =
                        entry.Entity.GetType()
                            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                    audit.GetType().GetProperty("Action").SetValue(audit, entry.Audit.Action);

                    foreach (var prop in props)
                    {
                        if (prop.CanWrite && prop.CanRead)
                            switch (prop.PropertyType.ToString())
                            {
                                case "System.String":
                                case "System.string":
                                case "System.Int32":
                                case "System.Nullable`1[System.Int32]":
                                case "System.Int64":
                                case "System.bool":
                                case "System.Boolean":
                                case "System.Nullable`1[System.Boolean]":
                                case "System.DateTime":
                                case "System.Nullable`1[System.DateTime]":
                                //case "System.NullReferenceException":
                                    //LM.20160906a - Handles null properties
                                    if (prop.GetValue(entry.Entity, null) != null)
                                    {
                                        audit.GetType().GetProperty(prop.Name).SetValue(audit, prop.GetValue(entry.Entity));
                                    }
                                    break;
                            }
                    }

                    // JK.20160509a - TODO: Replace with reflection.
                    switch (name)
                    {
                        //case "Account":
                        case "Agent":
                            AgentAudits.Add((AgentAudit)audit);
                            break;
                        case "AgentCustomer":
                            AgentCustomerAudits.Add((AgentCustomerAudit)audit);
                            break;
                        case "Application":
                            ApplicationAudits.Add((ApplicationAudit)audit);
                            break;
                        case "ApplicationRole":
                            ApplicationRoleAudits.Add((ApplicationRoleAudit)audit);
                            break;
                        case "ApplicationUserRole":
                            ApplicationUserRoleAudits.Add((ApplicationUserRoleAudit)audit);
                            break;
                        case "AppSetting":
                            AppSettingAudits.Add((AppSettingAudit)audit);
                            break;
                        case "CategoryTypeAudit":
                            CategoryTypeAudits.Add((CategoryTypeAudit) audit);
                            break;
                        case "Comment":
                            CommentAudits.Add((CommentAudit)audit);
                            break;
                        case "Country":
                            CountryAudits.Add((CountryAudit)audit);
                            break;
                        case "Customer":
                            CustomerAudits.Add((CustomerAudit)audit); 
                            break;
                        case "CustomerType":
                            CustomerTypeAudits.Add((CustomerTypeAudit)audit);
                            break;
                        case "Document":
                            DocumentAudits.Add((DocumentAudit)audit);
                            break;
                        case "DocumentType":
                            DocumentTypeAudits.Add((DocumentTypeAudit)audit);
                            break;
                        case "DocumentCheckList":
                            DocumentCheckListAudits.Add((DocumentCheckListAudit)audit);
                            break;
                        case "DocumentReference":
                            DocumentReferenceAudits.Add((DocumentReferenceAudit)audit);
                            break;
                      
                        case "Entity":
                            EntityAudits.Add((EntityAudit)audit);
                            break;
                        case "EntityType":
                            EntityTypeAudits.Add((EntityTypeAudit)audit);
                            break;
                        case "IdentificationType":
                            IdentificationTypeAudits.Add((IdentificationTypeAudit)audit);
                            break;
                        case "LocationType":
                            LocationTypeAudits.Add((LocationTypeAudit)audit);
                            break;
                        case "Log":
                            LogAudits.Add((LogAudit)audit);
                            break;
                        case "LogType":
                            LogTypeAudits.Add((LogTypeAudit)audit);
                            break;
                        case "Notification":
                            NotificationAudits.Add((NotificationAudit)audit);
                            break;
                        case "NotificationType":
                            NotificationTypeAudits.Add((NotificationTypeAudit)audit);
                            break;
                        case "NotificationSubscription":
                            NotificationSubscriptionAudits.Add((NotificationSubscriptionAudit)audit);
                            break;
                        case "Note":
                            NoteAudits.Add((NoteAudit)audit);
                            break;
                        case "NoteType":
                            NoteTypeAudits.Add((NoteTypeAudit)audit);
                            break;
                        case "Request":
                            RequestAudits.Add((RequestAudit)audit);
                            break;
                        case "Status":
                            StatusAudits.Add((StatusAudit)audit);
                            break;
                        case "StatusType":
                            StatusTypeAudits.Add((StatusTypeAudit)audit);
                            break;
                        case "SystemUser":
                            SystemUserAudits.Add((SystemUserAudit)audit);
                            break;
                        case "SystemUserType":
                            SystemUserTypeAudits.Add((SystemUserTypeAudit)audit);
                            break;
                        case "TitleType":
                            TitleTypeAudits.Add((TitleTypeAudit)audit);
                            break;
                        case "Query":
                            QueryAudits.Add((QueryAudit)audit);
                            break;
                    }
                }
            }
            catch (Exception x)
            {
                throw x;
            }
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();

        /// <summary>
        /// Gets the entity set.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Entity type not found in GetTableName</exception>
        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;
                string typeName = ObjectContext.GetObjectType(type).Name;
                var es =
                    octx.MetadataWorkspace.GetItemCollection(DataSpace.SSpace).GetItems<EntityContainer>().SelectMany(
                        c => c.BaseEntitySets.Where(e => e.Name == typeName)).FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }

        /// <summary>
        /// Gets the name of the table of the entity type.
        /// Used for auditing entities.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return string.Format("{0}", es.MetadataProperties["Table"].Value);
        }

        /// <summary>
        /// Gets the primary key property of an entity type.
        /// Used to get the value of the primary key for auditing.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string GetPrimaryKeyProperty(Type type)
        {
            EntitySetBase es = GetEntitySet(type);
            return es.ElementType.KeyMembers[0].Name;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // JK.20140902a - Include this to remove cascade deletions.
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            //modelBuilder.Entity<Sheet>()
            //    .HasOptional(f => f.BulkSheet)
            //    .WithMany()
            //    .HasForeignKey(f => f.SheetId);

            //modelBuilder.Entity<Sheet>()
            //    .HasOptional(f => f.SheetType)
            //    .WithMany()
            //    .HasForeignKey(f => f.SheetId);

            modelBuilder.Entity<SystemUser>()
                .HasOptional(f => f.CreatedBySystemUser)
                .WithMany()
                .HasForeignKey(f => f.CreatedBySystemUserId);

            modelBuilder.Entity<SystemUser>()
                .HasOptional(f => f.ModifiedBySystemUser)
                .WithMany()
                .HasForeignKey(f => f.ModifiedBySystemUserId);

         
        }
    }
}