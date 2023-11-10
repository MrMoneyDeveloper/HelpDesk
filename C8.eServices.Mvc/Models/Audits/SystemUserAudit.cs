using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Models.Audits
{
    public class SystemUserAudit : BaseModelAudit
    {
        [Column(Order = 10)]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Column(Order = 11)]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Column(Order = 12)]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Column(Order = 13)]
        [MaxLength(250)]
        public string CompanyName { get; set; }

        [Column(Order = 14)]
        [MaxLength(250)]
        public string Designation { get; set; }

        [Column(Order = 15)]
        [MaxLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Column(Order = 16)]
        public string SystemUserTypeId { get; set; }

        [Column(Order = 17)]
        [MaxLength(25)]
        public string StatusId { get; set; }

        [Column(Order = 18)]
        public bool IsPasswordReset { get; set; }

        [Column(Order = 19)]
        public bool IsTemporaryPassword { get; set; }
        [Column(Order = 22)]
        [Display(Name = "ID/ Passport No")]
        [MaxLength(100)]
        public string IdentificationNumber { get; set; }
        [Column(Order = 20)]
        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        [Column(Order = 21)]
        public string Code { get; set; }
        [Column(Order = 23)]
        [Display(Name = "Type of Notification")]
        public int? NotificationTypeId { get; set; }
        [ForeignKey("NotificationTypeId")]
        public NotificationType NotificationType { get; set; }
        [Display(Name = "User")]
        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}