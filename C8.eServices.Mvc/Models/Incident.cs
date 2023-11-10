using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Models
{
    // Base class for other entities

    public class Incident : BaseModel
    {
        // Ticket subject
        [Column(Order = 6)]
        [Display(Name = "Ticket Subject")]
        public string Subject { get; set; }

        // Description of the incident
        [Column(Order = 7)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        // Error URL
        [Column(Order = 8)]
        [Display(Name = "Error Url")]
        public string URL { get; set; }

        // Indicates if it's a caller ticket
        [Column(Order = 9)]
        [Display(Name = "Caller Ticket")]
        public bool IsCallerTicket { get; set; }

        // User who captured the incident
        public int CapturedByUserId { get; set; }
        [ForeignKey("CapturedByUserId")]
        [Display(Name = "Captured By")]
        [Column(Order = 10)]
        public SystemUser CapturedByUser { get; set; }

        // Status of the incident
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        [Display(Name = "Status")]
        [Column(Order = 11)]
        public Status Status { get; set; }

        // Reference number associated with the incident
        [Column(Order = 12)]
        [Display(Name = "Reference Number")]
        public string Reference { get; set; }

        // Product related to the incident
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [Display(Name = "Product")]
        [Column(Order = 13)]
        public Product Product { get; set; }

        // Sub-category of the incident
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        [Display(Name = "Sub-Category")]
        [Column(Order = 14)]
        public Sub_Catergory SubCategory { get; set; }

        // Category of the incident
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [Display(Name = "Category")]
        [Column(Order = 15)]
        public ServiceCategory Category { get; set; }

        [Column(Order = 16)]
        [Display(Name = "Ticket Subject")]
        public DateTime LoggedTime { get; set; }

        // Other SLA and time-related properties can be added here
    }

    // Entity representing a service category
    public class ServiceCategory : BaseType
    {
        // You can add specific properties related to service categories here
    }

    // Entity representing a sub-category
    public class Sub_Catergory : BaseType
    {
        // Priority associated with the sub-category
        public int? PriorityId { get; set; }
        [ForeignKey("PriorityId")]
        [Display(Name = "PriorityId")]
        [Column(Order = 6)]
        public Priority Priority { get; set; }

        // Service category associated with the sub-category
        public int? ServiceCategoryId { get; set; }
        [ForeignKey("ServiceCategoryId")]
        [Display(Name = "ServiceCategory")]
        [Column(Order = 7)]
        public ServiceCategory ServiceCategory { get; set; }

        // SLA value associated with the sub-category
        public int? SLA_Value { get; set; }
    }


    // Entity representing a fault code
    public class FaultCode : BaseType
    {
        // Sub-category associated with the fault code
        public int? Sub_CatergoryId { get; set; }
        [ForeignKey("Sub_CatergoryId")]
        [Display(Name = "Sub_CatergoryId")]
        [Column(Order = 6)]
        public Sub_Catergory Sub_Catergory { get; set; }
    }

    // Entity representing a priority
    public class Priority : BaseType
    {
        [Key] // Mark Id as the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate Id values
        [Column(Order = 1)]
        public new int Id { get; set; }

        // Minimum time associated with the priority
        [Display(Name = "Minimum Value")]
        [Column(Order = 6)]
        public int MinTime { get; set; }

        // Maximum time associated with the priority
        [Display(Name = "Maximum Value")]
        [Column(Order = 7)]
        public int MaxTime { get; set; }
       

    }

    // Entity representing a product
    public class Product : BaseType
    {
        // Additional product-related properties can be added here
    }

    // Entity representing an allocated product
    public class AllocatedProduct : BaseType
    {
        // Product associated with the allocated product
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [Display(Name = "ProductId")]
        [Column(Order = 6)]
        public Product Product { get; set; }

        // Client associated with the allocated product
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        [Display(Name = "ClientId")]
        [Column(Order = 7)]
        public Client Client { get; set; }
    }

    // Entity representing a product resource
    public class ProductResource : BaseType
    {
        // Product associated with the product resource
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [Display(Name = "ProductId")]
        [Column(Order = 6)]
        public Product Product { get; set; }

        // System user associated with the product resource
        public int SystemUserId { get; set; }
        [ForeignKey("SystemUserId")]
        [Display(Name = "SystemUserId")]
        [Column(Order = 7)]
        public SystemUser SystemUser { get; set; }
    }

    // Entity representing an incident caller
    public class IncidentCaller : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
    }

    // Entity representing priority for incidents
    public class PriorityIncidentUser : BaseType
    {
        // Additional properties for priority incident users can be added here
    }
}
