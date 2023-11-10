using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.ViewModels
{
    public class ViewHelperModel
    {
        public Int32 ProductId { get; set; }
        public  String ProductName { get; set; }
        public  Int32 AdministratorId { get; set; }
        public  String AdministratorName { get; set; }
    }
}