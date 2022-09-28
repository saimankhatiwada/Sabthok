using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SabthokFoodModel.Utillity
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailToSend = new MimeMessage();
            emailToSend.From.Add(MailboxAddress.Parse("hello@sabthokfood.com"));
            emailToSend.To.Add(MailboxAddress.Parse(email));
            emailToSend.Subject = subject;
            emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = htmlMessage };

            using(var emailclient = new SmtpClient())
            {
                emailclient.Connect("smtp.gmail.com",587,MailKit.Security.SecureSocketOptions.StartTls);
                emailclient.Authenticate("saimankhatiwada9611@gmail.com", "kjeyclnvxpeuxtjq");
                emailclient.Send(emailToSend);
                emailclient.Disconnect(true);
            }

            return Task.CompletedTask;
        }
    }
}
