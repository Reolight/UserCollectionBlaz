using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using UserCollectionBlaz.Data;

namespace UserCollectionBlaz.Service;

public class EmailService : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly string _email;
    public EmailService(IOptions<EmailSettings> options)
    {
        _smtpClient = new SmtpClient(options.Value.Host, 587);
        _smtpClient.Credentials = new NetworkCredential(options.Value.Email, options.Value.Password);
        _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        _smtpClient.EnableSsl = true;
        _email = options.Value.Email;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(_email, subject);
        mail.To.Add(new MailAddress(email));
        mail.Body = htmlMessage;
        mail.IsBodyHtml = true;
        await _smtpClient.SendMailAsync(mail);
    }
}