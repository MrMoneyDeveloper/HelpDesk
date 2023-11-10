using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace C8.eServices.Mvc.Models
{
    public class SystemIdentityUser : IdentityUser
    {
        [Required]
        public int SystemUserId { get; set; }

        public string UnconfirmedEmail { get; set; }

        [ForeignKey("SystemUserId")]
        public virtual SystemUser SystemUser { get; set; }
    }
}