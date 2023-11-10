using C8.eServices.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.ViewModels
{
    public class OnboardCleintUserViewModel : BaseModel
    {
        public Client Client { get; set; }
        public EntityUser EntityUser { get; set; }
    }
}