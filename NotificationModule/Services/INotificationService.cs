using NotificationModule.Commands;

namespace NotificationModule.Services;

public interface INotificationService
{
    public Task<bool> SendConfirmationAsync(SendConfirmationCommand command);
}