using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationModule.SMTP.Models;

namespace NotificationModule.SMTP.Services;

public class MailService : IMailService
{
    private readonly MailSettings _settings;

    public MailService(IOptions<MailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<bool> SendAsync(MailData mailData, CancellationToken ct)
    {
        try
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);
            
            foreach (string mailAddress in mailData.To)
                mail.To.Add(MailboxAddress.Parse(mailAddress));
                
            if(!string.IsNullOrEmpty(mailData.ReplyTo))
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));
            
            mail.Headers.Add("Precedence", "bulk");
            mail.Headers.Add("Auto-Submitted", "auto-generated");
            mail.Headers.Add("X-Auto-Response-Suppress", "OOF, AutoReply");
            mail.Headers.Add("X-Mailer", "HotelBookPro/1.0");
            
            if (mailData.Bcc != null)
            {
                foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }
            if (mailData.Cc != null)
            {
                foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }
            
            var unsubscribeEmail = mailData.From ?? _settings.From;
            mail.Headers.Add("List-Unsubscribe", $"<mailto:{unsubscribeEmail}?subject=unsubscribe>");
            mail.Headers.Add("List-Unsubscribe-Post", "List-Unsubscribe=One-Click");
            
            mail.Subject = mailData.Subject;

            var body = new BodyBuilder();
            body.HtmlBody = mailData.Body;
            mail.Body = body.ToMessageBody();
            mail.Date = DateTimeOffset.Now;
            
            using (var client = new SmtpClient()) 
            {
               
                await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_settings.UserName, _settings.Password);
                await client.SendAsync(mail, ct);
                await client.DisconnectAsync(true);
            }

            return true;
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Mail sending failed: {ex.Message}");
            return false;
        }
    }
}