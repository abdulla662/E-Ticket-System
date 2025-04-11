using System.Net.Mail;
using System.Net;
using E_Ticket_System.Utility;

namespace E_Ticket_System.Models
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mail@gmail.com", "pass")
            };

            return client.SendMailAsync(
                new MailMessage(from: "mail@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true

                }

                );

        }
    }

}