using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Keys
{
    public class ViewCodeKeys
    {
        public const int UpdateAgentCustomerProfile = 1;
        public const int RatesRebateThirdPartyCustomerProfile = 2;
        public const int CreateAgentCustomerProfile = 3;
        public const int IndividualCustomerProfile = 4;
        public const int UpdateCustomerLinkedAccount = 5;
        public const int ViewCustomerLinkedAccount = 6;
        public const int UpdateCustomerDocuments = 7;
        public const int LoadRatesRebateProperties = 8;
        public const int UpdateRatesRebateProperty = 9;
        public const int ViewUploadedFile = 10;
        public const int CreateCustomerLinkedAccount = 11;
        public const int ViewRatesRebates = 12;
        public const int ViewRatesRebateProperties = 13;
        public const int DownloadBills = 14;
        public const int UpdateCustomerIncentivePolicyApplication = 15;
        //Navigation Keys
        public const string StepOne = "PrincipaleOwner";
        public const string StepTwo1 = "CreateProperty";
        public const string StepTwo2 = "EditProperty";
        public const string StepThree = "Three";
        public const string StepFour = "Four";
        public const string StepFive = "Five";
        public const string FinalStep = "Final";
        public const string DefaultStep = "Delete";  
    }
}