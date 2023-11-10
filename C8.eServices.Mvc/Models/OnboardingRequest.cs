using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace C8.eServices.Mvc.Models
{
    public class OnboardingRequest : BaseModel
    {
        [Column(Order = 10)]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Column(Order = 11)]
        [Display(Name = "Industry")]
        public int IndustryId { get; set; }
        [ForeignKey("IndustryId")]
        [Display(Name = "Work Industry")]
        [ScriptIgnore]
        public Industry Industry { get; set; }

        [Column(Order = 12)]
        [Display(Name = "Country")]
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        [Display(Name = "Country of Origin")]
        [ScriptIgnore]
        public Country Country { get; set; }

        [Column(Order = 13)]
        [Display(Name = "VAT No.")]
        public string VatNumber { get; set; }

        [Column(Order = 14)]
        [Display(Name = "Domain")]
        public string CompanyDomain { get; set; }


        [Column(Order = 15)]
        [Display(Name = "First Name")]
        public string FistName { get; set; }

        [Column(Order = 16)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column(Order = 17)]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Column(Order = 18)]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Column(Order = 19)]
        [Display(Name = "Country")]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        [Display(Name = "Status")]
        [ScriptIgnore]
        public Status Status { get; set; }

        [NotMapped]
        public string Countr
        {
            get { return Country.Name; }
        }
        [NotMapped]
        public string Industr
        {
            get { return Industry.IndustryName; }
        }
        [NotMapped]
        public string PrimaryContactPerson
        {
            get { return $"{FistName} {LastName}"; }
        }
        [NotMapped]
        public string key { get; set; }

        [Column(Order = 20)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Column(Order = 21)]
        [Display(Name = "Designation")]
        public string Designation { get; set; }
        

        [Column(Order = 22)]
        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

        [Column(Order = 23)]
        [Display(Name = "Period To")]
        public DateTime PeriodTo { get; set; }

        [Column(Order = 24)]
        [Display(Name = "Operational Hours")]
        public string OperationalHours { get; set; }

        [Column(Order = 25)]
        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

    }
}