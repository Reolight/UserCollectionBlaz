using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace UserCollectionBlaz.Service;

public class EmailService : IEmailSender
{
    private readonly SmtpClient _smtpClient;

    public EmailService()
    {
        _smtpClient = new SmtpClient("smtp.gmail.com", 587);
        _smtpClient.Credentials = new NetworkCredential("reolightmiene@gmail.com", "wwhpcujwkpmdvfgi");
        _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        _smtpClient.EnableSsl = true;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("reolightmiene@gmail.com", subject);
        mail.To.Add(new MailAddress(email));
        mail.Body = htmlMessage;
        mail.IsBodyHtml = true;
        await _smtpClient.SendMailAsync(mail);
    }
}