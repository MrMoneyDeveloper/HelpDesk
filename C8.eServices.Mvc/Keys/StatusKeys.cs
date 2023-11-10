using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace C8.eServices.Mvc.Keys
{
    public class StatusKeys
    {
        // Rates Rebate.
        public const string RatesRebateApplicationPending = "s_rates_rebate_application_pending";
        public const string RatesRebateApplicationQuery = "s_rates_rebate_application_query";
        public const string RatesRebateApplicationReceived = "s_rates_rebate_application_received";
        public const string RatesRebateInProgress = "s_rates_rebate_in_progress";
        public const string RatesRebateApplicationApproved = "s_rates_rebate_application_approved";
        public const string RatesRebateApplicationSubmitted = "s_rates_rebate_application_submitted";
        public const string RatesRebateApplicationDeclined = "s_rates_rebate_application_declined";

        // Rates Rebate Property.
        public const string RatesRebatePropertyPending = "s_rates_rebate_property_pending";
        public const string RatesRebatePropertyConflict = "s_rates_rebate_property_conflict";
        public const string RatesRebatePropertyVerified = "s_rates_rebate_property_verified";
        public const string RatesRebatePropertyConflictWithValueAssist = "s_conflict";

        // Property.
        public const string PropertyPending = "s_property_pending";
        public const string PropertyConflict = "s_property_conflict";
        public const string PropertyVerified = "s_property_verified";

        // Additional Property Owners.
        public const string RatesRebateAdditionalPropertyOwnersPending = "s_additional_property_owners_pending";
        public const string RatesRebateAdditionalPropertyOwnersConfict = "s_additional_property_owners_conflict";
        public const string RatesRebateAdditionalPropertyOwnersVerified = "s_additional_property_owners_verified";

        // Property Account.
        public const string RatesRebatePropertyAccountPending = "s_rates_rebate_property_account_pending";
        public const string RatesRebatePropertyAccountConflict = "s_rates_rebate_property_account_conflict";
        public const string RatesRebatePropertyAccountVerified = "s_rates_rebate_property_account_verified";

        // Documents.
        public const string DocumentUploaded = "s_document_uploaded";
        public const string DocumentVerified = "s_document_verified";
        public const string DocumentPending = "s_document_pending";

        // Customer.
        public const string CustomerPendingApproval = "s_customer_pending";
        public const string CustomerActive = "s_customer_active";
        public const string CustomerPendingDocuments = "s_customer_pending_document";
        
        // Linked Account.
        public const string LinkedAccountPending = "s_linked_account_pending";
        public const string LinkedAccountActive = "s_linked_account_active";
        public const string LinkedAccountUnlinked = "s_linked_account_unlinked";

        // Agent.
        public const string AgentPending = "s_agent_pending";
        public const string AgentActive = "s_agent_active";

        // Entity.
        public const string EntityPending = "s_entity_pending";
        public const string EntityActive = "s_entity_active";

        // Account.
        public const string AccountActive = "s_account_active";
        public const string AccountPending = "s_account_pending";

        // System User.
        public const string SystemUserAccountActive = "s_system_user_account_active";

        // Email
        public const string PendingEmailVerification = "s_email_verification_pending";
        public const string EmailVerified = "s_email_verified";

        //Customer Query
        public const string CustomerQueryPending = "s_customer_query_pending";
        public const string CustomerQueryInProgress = "s_customer_query_inprogress";
        public const string CustomerQueryResolved = "s_customer_query_resolved";

        //Incentive Policy Property 
        public const string IncentivePolicyPropertyPending = "s_incentive_policy_property_pending";
        public const string IncentivePolicyPropertyVerified = "s_incentive_policy_property_verified";

        //Incentive Policy Application
        public const string IncentivePolicyApplicationSubmitted = "s_incentive_policy_application_submitted";
        public const string IncentivePolicyApplicationReSubmitted = "s_incentive_policy_application_resubmitted";
        public const string IncentivePolicyApplicationInQuery = "s_incentive_policy_application_inquery";
        public const string IncentivePolicyApplicationPending = "s_incentive_policy_application_pending";
        public const string IncentivePolicyApplicationApproved = "s_incentive_policy_application_approved";
        public const string IncentivePolicyApplicationDeclined = "s_incentive_policy_application_declined";
        public const string IncentivePolicyApplicationProcessing = "s_incentive_policy_application_processing";

        // Debit Order
        public const string DebitOrderSent = "s_debit_order_sent";
        public const string DebitOrderPending = "s_debit_order_pending";
        public const string DebitOrderSuccess = "s_debit_order_success";
        public const string DebitOrderFailed = "s_debit_order_failed";

        // Support Query
        public const string SupportQueryPending = "s_support_query_pending";
        public const string SupportQueryClosed = "s_support_query_closed";
        public const string SupportQueryResolved = "s_support_query_resolved";

        // Linked Mobile
        public const string LinkedMobilePending = "s_linked_mobile_pending";
        public const string LinkedMobileActive = "s_linked_mobile_active";
        public const string LinkedMobileUnlinked = "s_linked_mobile_unlinked";

        // Masterpass
        public const string MasterpassNoticationRecieved = "s_masterpass_notication_recieved";
        public const string MasterpassNoticationProcessed = "s_masterpass_notication_processed";

        //SMS
        public const string SMSSent = "s_sms_sent";
        public const string SMSPending = "s_sms_pending";
        public const string SMSSuccess = "s_sms_success";
        public const string SMSFailed = "s_sms_failed";

        //Onboarding
        public const string OnboardingPending = "s_onboarding_pending_approval";
        public const string PendingUpdate = "s_client_pending_update";
        public const string OnboardingDeclined = "s_client_declined_onboarding";
    }
}