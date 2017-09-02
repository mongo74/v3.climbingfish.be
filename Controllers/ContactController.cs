using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using v3.climbingfish.be.Models;

namespace v3.climbingfish.be.Controllers
{
    public class ContactController : SurfaceController
    {
        [ChildActionOnly]
        public ActionResult ContactForm(string targetPage)
        {
            var model = new ContactModel();
            model.TargetPage = targetPage; //"thankpage
            return PartialView("ContactUs", model);
        }
        
        [HttpPost]
        public ActionResult HandleContactForm(ContactModel model)
        {

            var emailTo = ConfigurationManager.AppSettings["ContactEmail"];
           // var emailTo = "climbingfish@yopmail.com";

            int targetPageId;
            var parsed = Int32.TryParse(model.TargetPage, out targetPageId);
            if (!parsed)
                targetPageId = Convert.ToInt32(ConfigurationManager.AppSettings["ContactusResponsePage"]);

            ValidateForm(model);

            if (!ModelState.IsValid) return CurrentUmbracoPage();


            var sendEmailsFrom = model.Email ?? emailTo;
            var body = String.Format("From: {0}, Email: {1}, Tel: {2} and Message: {3}", model.FullName, model.Email, model.Telephone, model.Message);
            var subject = model.Subject ?? "info@climbingfish.be - some question"; 

            try
            {
                var _emailSender = new EmailSender(ConfigurationManager.AppSettings["SmtpServer"]);
                _emailSender.SendEmail(sendEmailsFrom, "", emailTo, subject,body);

                TempData["InfoMessage"] = "Your message has been successfully sent and we will be in touch soon...";

                // Clear all the form fields
                ModelState.Clear();
                model.FullName = string.Empty;
                model.Telephone = string.Empty;
                model.Email = string.Empty;
                model.Message = string.Empty;
                    
                    
                Response.Redirect(Umbraco.NiceUrl(targetPageId));
                    
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message + ex.StackTrace;
            }

            //redirect to current page to clear the form
            return CurrentUmbracoPage();

        }

        private void ValidateForm(ContactModel model)
        {
            //  ModelState.AddModelError("Email", "Dit email adres bestaat al. Gebruik een ander");
            if (string.IsNullOrEmpty(model.FullName))
                ModelState.AddModelError("Fullname", "Vergeet je je naam niet?");

            if(string.IsNullOrEmpty(model.Email))
                ModelState.AddModelError("Email", "Vergeet je je emailadres niet??");

            if (string.IsNullOrEmpty(model.Subject))
                ModelState.AddModelError("Email", "Vergeet je je onderwerp  niet??");

            if (string.IsNullOrEmpty(model.Message))
                ModelState.AddModelError("Email", "Vergeet je je berichtje  niet??");
        }
    }

    public class EmailSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender(string smtpServer)
        {
            var mailLogin = ConfigurationManager.AppSettings["MailUsername"];//Settings.MailUsername;
            var mailPW = ConfigurationManager.AppSettings["MailPassword"];  //Settings.MailPassword;
            _smtpClient = new SmtpClient(smtpServer)
            {
                Credentials = new NetworkCredential(mailLogin, mailPW),
                EnableSsl = false,
                Port = 25
            };
        }

        public void SendEmail(string from, string fromDisplayName, string to, string subject, string body, bool ishtml = true)
        {
            SendEmail(from, fromDisplayName, new List<string> { to }, subject, body, ishtml);
        }

        public void SendEmail(string from, string fromDisplayName, List<string> to, string subject, string body, bool ishtml = true)
        {
            SendEmail(from, fromDisplayName, to, null, subject, body, null, ishtml);
        }

        public void SendEmail(string from, string fromDisplayName, List<string> to, List<string> cc, string subject, string body, List<string> attachments = null, bool ishtml = true)
        {
            var mail = new MailMessage();

            try
            {
                mail.Subject = subject;
                foreach (string email in to)
                {
                    if (!string.IsNullOrWhiteSpace(email))
                        mail.To.Add(email);
                }
                if (cc != null)
                {
                    foreach (string email in cc)
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                            mail.CC.Add(email);
                    }
                }

                mail.From = new MailAddress(from, fromDisplayName);
                mail.Body = body;
                mail.IsBodyHtml = ishtml;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        mail.Attachments.Add(new Attachment(attachment));
                    }
                }
                _smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                string smptcredentials = string.Format("{0}", _smtpClient.Host);
             //   Log.Add(LogTypes.Error, -1, string.Format("ConfirmationMail could not be sent - {0} - {1}", ex, smptcredentials));
            }
        }
    }
}