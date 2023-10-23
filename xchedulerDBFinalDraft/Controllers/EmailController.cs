using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;

namespace xchedulerDBFinalDraft.Controllers
{
    public class EmailController : Controller
    {
        private const string SendGridApiKey = "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";
        public ActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(string RecipientEmail, string EmailSubject, HttpPostedFileBase file, string EmailContent)
        {
            string toEmail = RecipientEmail;
            string subject = EmailSubject;

            try
            {
                var client = new SendGridClient(SendGridApiKey);

                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("rohithoff97@gmail.com", "Rohith Ravindran"),
                    Subject = subject,
                    PlainTextContent = EmailContent,
                    HtmlContent = EmailContent
                };

                msg.AddTo(new EmailAddress(toEmail));

                // Attachments
                if (file != null && file.ContentLength > 0)
                {
                    using (var stream = file.InputStream)
                    {
                        await msg.AddAttachmentAsync(file.FileName, stream);
                    }
                }

                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                {
                    ViewBag.Message = "Error sending email.";
                }
                else
                {
                    ViewBag.Message = "Email sent successfully!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error sending email: " + ex.Message;
            }

            return View();

        }



    }

}
