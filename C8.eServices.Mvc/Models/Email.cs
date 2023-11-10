using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using C8.eServices.Mvc.DataAccessLayer;
using C8.eServices.Mvc.Keys;

namespace C8.eServices.Mvc.Models
{
    public class Email
    {
        private eServicesDbContext db = new eServicesDbContext();
        private CesarDbContext core = new CesarDbContext();

        public Email()
        {
            IdentityManager = new IdentityManager(db);
        }

        public IdentityManager IdentityManager { get; set; }

        public void GenerateEmail(string recipientEmail, string subject, string body,
        string referenceId, bool hasAttachment, string emailTemp, string recipientName = "", string ccList = "",
        string bccList = "", string referenceNumber = "", byte[] attachment = null, string attachmentName = "", List<byte[]> attachments = null, List<string> fileNames = null)
        {
            try
            {
                if (recipientEmail == null)
                    return;

                var emailApplication = db.AppSettings.FirstOrDefault(a => a.Key == AppSettingKeys.EservicesApplication);
                var emailAccount = db.AppSettings.FirstOrDefault(a => a.Key == AppSettingKeys.EservicesEmailAccount);
                var emailTemplate = db.AppSettings.FirstOrDefault(a => a.Key == emailTemp);

                if (emailApplication == null) throw new Exception("Invalid Application setting");
                if (emailAccount == null) throw new Exception("Invalid Application setting");
                if (emailTemplate == null) throw new Exception("Invalid Application setting");

                var emailBody = emailTemplate.Value;
                emailBody = emailBody.Replace("#NAME#", (recipientName != "") ? recipientName : "Sir/ Madam");
                emailBody = emailBody.Replace("#BODYTEXT#", body);

                if (!string.IsNullOrEmpty(referenceNumber))
                    emailBody = emailBody.Replace("#REFERENCENUMBER#", referenceNumber);

                var email = new EmailQueueItem
                {
                    ApplicationId = Convert.ToInt32(emailApplication.Value),
                    QueueDateTime = DateTime.Now,
                    EmailAccountId = Convert.ToInt32(emailAccount.Value),
                    ToList = recipientEmail,
                    CcList = (ccList != "") ? ccList : null,
                    BccList = (bccList != "") ? bccList : null,
                    Subject = subject,
                    Body = emailBody,
                    IsHtml = true,
                    FailureCount = 0,
                    ReferenceId = referenceId,
                    HasAttachments = hasAttachment
                };

                core.EmailQueue.Add(email);
                core.SaveChanges();

                //Single attachments
                if (!hasAttachment) return;
                if (attachment != null)
                {
                    AddEmailAttachement(Convert.ToInt32(emailApplication.Value), attachment, email.EmailQueueId, attachmentName);
                }

                //Attach more than 1 attachments
                if (attachments == null || fileNames == null) return;
                for (var i = 0; i < attachments.Count; i++)
                {
                    if (attachments[i] != null && fileNames[i] != null)
                    {
                        AddEmailAttachement(Convert.ToInt32(emailApplication.Value), attachments[i], email.EmailQueueId, fileNames[i]);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Adds Email Queue Item
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="attachment"></param>
        /// <param name="emailQueueId"></param>
        /// <param name="fileName"></param>
        private void AddEmailAttachement(int applicationId, byte[] attachment, int emailQueueId, string fileName)
        {
            try
            {
                var emailAttachment = new EmailAttachmentQueue
                {
                    ApplicationId = applicationId,
                    Attachment = attachment,
                    ContentType = "application/pdf",
                    EmailQueueId = emailQueueId,
                    Filename = fileName
                };

                core.EmailAttachmentQueue.Add(emailAttachment);
                core.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<string> ExtractEmails(string text)
        {
            if (text.Trim().Length == 0) return new List<string>();

            JavaScriptSerializer js = new JavaScriptSerializer();
            string[] emails = js.Deserialize<string[]>(text);
            List<string> emailList = new List<string>();
            const string MatchEmailPattern =
                @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$";
            Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (var e in emails)
            {
                // Find matches.
                MatchCollection matches = rx.Matches(e);
                // Report the number of matches found.
                int noOfMatches = matches.Count;
                // Report on each match.

                foreach (Match match in matches)
                {
                    emailList.Add(match.Value.ToString());
                }
            }
            
            return emailList;
        }
    }
}