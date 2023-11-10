using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace C8.eServices.Mvc.Models
{
    public class Client : BaseModel
    {
        [Column(Order = 10)]
        [Display(Name = "Company Name")]
        public string Company { get; set; }

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
        public string Domain { get; set; }

        [Column(Order = 15)]
        public int StatusId { get; set; }
        [ForeignKey("StatusId")]
        [Display(Name = "Status")]
        [ScriptIgnore]
        public Status Status { get; set; }

        [Column(Order = 16)]
        [Display(Name = "Period To")]
        public DateTime? PeriodTo { get; set; }

        [Column(Order = 17)]
        [Display(Name = "Office Address")]
        public string OfficeAddress{ get; set; }

        [Column(Order = 18)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Column(Order = 19)]
        [Display(Name = "Working Hours")]
        public string OperationalHours { get; set; }

        public string PrimaryContactPerson
        {
            get { return $"{((RepresentedByUser == null) ? "" : RepresentedByUser.FirstName)} {((RepresentedByUser == null) ? "" : RepresentedByUser.LastName)}"; }
        }
        public string Countr
        {
            get { return Country.Name; }
        }
        public string Industr
        {
            get { return Industry.IndustryName; }
        }
        public string StatusValue
        {
            get { return Status.Name; }
        }

        [Column(Order = 20)]
        public Nullable<int> RepresentedByUserId { get; set; }
        [ForeignKey("RepresentedByUserId")]
        [Display(Name = "RepresentedByUser")]
        [ScriptIgnore]
        public SystemUser RepresentedByUser { get; set; }

        [Column(Order = 21)]
        public Nullable<int> AdminByUserId { get; set; }
        [ForeignKey("AdminByUserId")]
        [Display(Name = "AdminByUserId")]
        [ScriptIgnore]
        public SystemUser AdminByUser { get; set; }


        [NotMapped]
        public List<SystemUser> SystemUsers { get; set; }
    }
}