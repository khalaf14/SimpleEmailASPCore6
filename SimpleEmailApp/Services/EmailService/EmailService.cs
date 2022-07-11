using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.StaticFiles;
using MimeKit;
using MimeKit.Text;
using System.Diagnostics;

namespace SimpleEmailApp.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(EmailDto request)
        {
            //
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

            #region attachment
            // email.Attachments.
            var builder = new BodyBuilder();

            // Set the plain-text version of the message text
            builder.HtmlBody = request.Body;

            // We may also want to attach a calendar event for Monica's party...
            var folderName = Path.Combine("Resources", "Files");
            var FullPathWuthFile = Path.Combine(Directory.GetCurrentDirectory(), folderName, "excel_list.xlsx");

            builder.Attachments.Add(FullPathWuthFile);

            // Now we just need to set the message body and we're done
            email.Body = builder.ToMessageBody();
            // email attachments
            #endregion attachment
            using var smtp = new SmtpClient();
            smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
            
        }


    }
}
