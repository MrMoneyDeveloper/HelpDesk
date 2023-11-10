using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace C8.eServices.Mvc.Models
{
    public class EntityUser : BaseModel
    {

        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        [Display(Name = "Client")]
        public Client Clients { get; set; }

        [Column(Order = 13)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Column(Order = 14)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Column(Order = 15)]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Column(Order = 16)]
        [Display(Name = "Contact Number")]
        public string PhoneNumber { get; set; }

        [Column(Order = 18)]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Column(Order = 19)]
        [Display(Name = "Designation")]
        public string Deignation { get; set; }

        [Column(Order = 20)]
        [Display(Name = "Period To")]
        public DateTime? PeriodTo { get; set; }
    }
}