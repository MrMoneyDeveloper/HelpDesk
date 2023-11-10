using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.ViewModels
{
    public class BillViewModel
    {
        public string AccountNumber { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Index { get; set; }
    }
}