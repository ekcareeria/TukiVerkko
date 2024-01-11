using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;

namespace TukiVerkko1.Models
{
    public class EmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            var smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var smtpUserName = ConfigurationManager.AppSettings["SmtpUserName"];
            var smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(smtpUserName, smtpPassword);
                client.EnableSsl = true;

                var message = new MailMessage
                {
                    From = new MailAddress(smtpUserName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(to);

                client.Send(message);
            }
        }
    }
}