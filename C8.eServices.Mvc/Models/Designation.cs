using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Models
{
    public class Designation : BaseType
    {
        [Column(Order = 13)]
        [Display(Name = "Employee Id Type")]
        public int EmployeeTypeId { get; set; }
        [ForeignKey("EmployeeTypeId")]
        public EmployeeType EmployeeType { get; set; }
    }
}