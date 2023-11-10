using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using C8.eServices.Mvc.Models;

namespace C8.eServices.Mvc.ViewModels
{
    public class DocumentsViewModel
    {
        public int ReferenceId { get; set; }
        public int ReferenceTypeId { get; set; }
        public int ApplicationId { get; set; }
        public int ? CustomerId { get; set; }
        public int StatusId { get; set; }

        public string ApplicationReferenceNumber { get; set; }
        public string ReturnUrl { get; set; }

        public bool IsUploadView { get; set; }

        public ReferenceType ReferenceType { get; set; }
        public Application Application { get; set; }
        public List<DocumentCheckList> DocumentCheckLists { get; set; }

        public List<Document> Documents { get; set; }
    }
}