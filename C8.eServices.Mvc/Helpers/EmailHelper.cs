using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace C8.eServices.Mvc.Helpers
{
    public class EmailHelper
    {
        private string _username = @"Ekurhuleni/Emmbpmdev";//"smtpdev @ithinkweb.co.za";//"noreply@calc8.co.za";
        private string _password = "#Eku2019!";//"dev @itw@Pass123";//"kFqR72JBUK?x";
        private string _smtpServer = "10.31.3.24";//"mail.ithinkweb.co.za";//"host27.axxesslocal.co.za";
        private int _smtpPort = 25;

        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void SendEmail()
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(_smtpServer);

            mail.IsBodyHtml = true;
            mail.From = new MailAddress(!_username.Contains("@") ? "noreply@ekurhuleni.gov.za" : _username);
            mail.To.Add(Recipient);
            mail.Subject = Subject;
            mail.Body = Body;

            SmtpServer.Port = _smtpPort;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new System.Net.NetworkCredential(_username, _password);
            SmtpServer.EnableSsl = true;
            SmtpServer.Timeout = 100000;

            SmtpServer.Send(mail);
        }
    }
}