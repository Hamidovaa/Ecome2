using MailKit.Net.Smtp;
using MimeKit;
using Ecome2.Models;

namespace Ecome2.EXtentions
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
    }
    public class EmailSender : IEmailService
    {
        public readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string to, string subject, string message)
        {
            //try
            //{

            //var emailSettings = _configuration.GetSection("EmailSettings");

            //var mimeMessage = new MimeMessage();
            //mimeMessage.From.Add(new MailboxAddress(emailSettings["FromName"], emailSettings["FromEmail"]));
            //mimeMessage.To.Add(MailboxAddress.Parse(to));
            //mimeMessage.Subject = subject;
            //mimeMessage.Body = new TextPart("plain") { Text = message };

           
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("Ecome", "Hemidoffa55@gmail.com"));
            mimeMessage.To.Add(MailboxAddress.Parse(to));
            mimeMessage.Subject = "subject";
            mimeMessage.Body = new TextPart("html") { Text = message };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync("Hemidoffa55@gmail.com", "ehakfkeoqhiiamhv");
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
            var msg = new MimeMessage();
            var mailFrom = new MailboxAddress("Admin", "Hemidoffa55@gmail.com");
            var mailTo = new MailboxAddress("User", to);
            //}
            //catch (Exception ex)
            //{

            //    Console.WriteLine($"Error: {ex}");
            //}
        }
    }


}