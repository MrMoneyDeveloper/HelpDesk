using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Keys
{
    public class AppSettingKeys
    {
        public const string SharePointUsername = "as_share_point_username";
        public const string SharePointPassword = "as_share_point_password";
        public const string SharePointUrl = "as_share_point_url";
        public const string SharePointSite = "as_share_point_site";
        public const string SharePointDomain = "as_share_point_domain";
        public const string SharePointLibrary = "as_share_point_library";
        public const string ReportServerUrl = "eservices_report_server_url";
        public const string ReportPath = "eservices_report_path";
        public const string ReportServerUsername = "eservices_report_server_username";
        public const string ReportServerPassword = "eservices_report_server_password";
        public const string ReportServerDomain = "eservices_report_server_domain";
        public const string GeneratedBillFileLocation = "eservices_generated_bill_file_location";
        public const string GeneratedRmsBillsLocation = "eservices_generated_rms_bills_root_location";
        public const string SortedRmsBillsLocation = "eservices_sorted_rms_bills_location";
        public const string CustomerHelperEmails = "as_customer_help_emails";
        public const string RevlineHelperEmail = "as_revline_help_emails";
        public const string PropertyMarketValue = "as_va_property_market_value";
        public const string PropertyRatingCategoryCode = "as_va_property_rating_category_code"; 
        public const string EservicesApplication = "eservices_applicationID";
        public const string EservicesEmailAccount = "eservices_email_account";
        public const string EservicesDefaultEmailTemplate = "eservices_default_email_template";     
        public const string RatesRebateEmailAccount = "rates_rebate_default_email_template";
        public const string LinkedAccountsAssociatedEmailTemplate = "linked_accounts_associated_email_template";
        public const string LinkedAccountsUnassociatedEmailTemplate = "linked_accounts_unassociated_email_template";
        public const string CopyAccountWatermark = "as_copy_account_watermark";
        public const string LinkedAccountsActiveMessage = "linked_accounts_active_message";
        public const string LinkedAccountsPendingAccountApprovalMessage = "linked_accounts_pending_account_approval_message";
        public const string LinkedAccountsPendingAccountApprovalAndEmailActivationMessage = "linked_accounts_pending_account_and_email_activation_message";
        public const string LinkedEmailsPendingEmailActivationMessage = "linked_emails_pending_email_activation_message";
        public const string LinkedEmailsPendingEmailActivationAndLinkedAccountApprovalMessage = "linked_emails_pending_email_activation_and_linked_account_approval_message";
        public const string IpAddressRange = "eservices_customer_centres_ip_range";
        public const string UpdateNotification = "eservices_update_notification";
        public const string IncentivePolicyDocumentLocation = "incentive_policy_document_location";
        public const string IncentivePolicyDefaultEmailTemplate = "incentive_policy_default_email_template";
        public const string UploadNetworkDirectory = "eservices_upload_network_directory";
        public const string UploadHtmlDirectory = "upload_html_directory";
        public const string IncentivePolicyTermsAndConditions = "incentive_policy_ts_cs";

        // API Services
        public const string MeterConsumptionUrl = "meter_consumption_url";
        public const string AcbBusinessAccountNumber = "acb_business_account_number";
        public const string SupportEmailList = "as_support_email_list";
        public const string AVSCustomerID = "avs_customer_id";
        public const string AVSCustomerKey = "avs_customer_api_key";
        public const string AVSEndUrl = "avs_first_check_url";
        public const string AVSCheckUrl = "avs_check_14_day_url";

        // Venus/ Solar Intergration
        public const string CustomerAccountsApi = "api_customer_accounts";
        public const string AccountBalanceApi = "api_account_balance";
        public const string AccountBalanceApiUsername = "api_account_balance_username";
        public const string AccountBalanceApiPassword = "api_account_balance_password";

        // Masterpass Integration
        public const string MasterpassUrl = "as_masterpass_url";
        public const string MasterpassUsername = "as_masterpass_username";
        public const string MasterpassPassword = "as_masterpass_password";

        // SMPP Settings
        public const string SmppHost = "as_smpp_host";
        public const string SmppUsername = "as_smpp_username";
        public const string SmppPassword = "as_smpp_password";
        public const string SmppPort = "as_smpp_port";

        // Masterpass Daily File
        public const string MasterpassDailyUrl = "as_masterpass_daily_url";
        public const string MasterpassDailyPath = "as_masterpass_daily_path_url";
        public const string MasterpassDailyStarttime = "as_masterpass_daily_starttime";
        public const string MasterpassDailyEndtime = "as_masterpass_daily_endtime";
        public const string MasterpassDailyFilename = "as_masterpass_daily_filename";
        public const string MasterpassDailyExecutiontime = "as_masterpass_daily_executiontime";
        public const string MasterpassDailyTest = "as_masterpass_test";

        // Masterpass Daily File
        public const string MasterpassHourlyPath = "as_masterpass_hourly_path";
        public const string MasterpassHourlyFilename = "as_masterpass_hourly_filename";

        // Payment History
        public const string PaymenthistoryPollingDirectory = "as_paymenthistory_polling_directory";

        //Recurring Debit Orders
        public const string RecurringDebitOrdersStrikeDayEdit = "as_recurring_debit_orders_strikeday_edit";
        public const string RecurringDebitOrdersStrikeDayRemove = "as_recurring_debit_orders_strikeday_remove";

        //Sequence Counters
        public const string DebitOrderBatchSequence = "debit_order_daily_sequence_counter";
        public const string EFTBatchSequence = "eft_daily_sequence_counter";
        public const string DebitOrderBatchSequenceLimit = "debit_order_daily_sequence_limiter";
        public const string EFTBatchSequenceLimit = "eft_daily_sequence_limiter";

        //App Config toggle Settings
        public const string AccountLock = "account_lock_out";
        public const string DOMultiplePaymentsDaily = "debit_order_multiple_payments_daily";
        public const string DOMultiplePaymentsMonthly= "debit_order_multiple_payments_monthly";
        public const string DOMultiplePaymentsAmount= "debit_order_multiple_payments_amount";

        //Ad Account for Reports
        public const string AdUserName = "ad_user_name";
        public const string AdPassword = "ad_password";
        public const string adDomain = "ad_domain";
        public const string adGenBillLink = "ad_bill_Link";
        public const string adGenDateRangelink = "ad_date_range_report";
        public const string adGenDetailedlink = "ad_detailed_report";

    }
}