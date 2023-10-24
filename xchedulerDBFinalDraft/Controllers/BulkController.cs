using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;
using System.Linq;

namespace xchedulerDBFinalDraft.Controllers
{
    public class BulkController : Controller
    {
        private const string SendGridApiKey = "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";

        public ActionResult BulkEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> BulkEmail(string RecipientEmails, string EmailSubject, HttpPostedFileBase file, string EmailContent)
        {
            try
            {
                var client = new SendGridClient(SendGridApiKey);

                // Split the comma or semicolon separated email addresses into a list
                var recipientEmailList = RecipientEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                                                       .Select(email => email.Trim())
                                                       .ToList();

                foreach (var toEmail in recipientEmailList)
                {
                    var msg = new SendGridMessage()
                    {
                        From = new EmailAddress("rohithoff97@gmail.com", "Rohith Ravindran"),
                        Subject = EmailSubject,
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
                }

                ViewBag.Message = "Bulk emails sent successfully!";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error sending bulk emails: " + ex.Message;
            }

            return View();
        }
    }
}
