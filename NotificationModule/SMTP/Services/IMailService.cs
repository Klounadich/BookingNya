using NotificationModule.SMTP.Models;

namespace NotificationModule.SMTP.Services;

public interface IMailService
{
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);
}