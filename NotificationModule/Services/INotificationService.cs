using NotificationModule.Commands;

namespace NotificationModule.Services;

public interface INotificationService
{
    public Task<bool> SendConfirmationAsync(SendConfirmationCommand command);
    public Task<(bool?,int?)> ConfirmCode(ConfirmCodeCommand data);
}