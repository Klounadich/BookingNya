using NotificationModule.Commands;
using NotificationModule.Models;
using NotificationModule.Repositories;
using Shared.Enums;

namespace NotificationModule.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }
    public async Task<bool> SendConfirmationAsync(SendConfirmationCommand command)
    {
        string rand_confirm_number = new Random().Next(1000, 9999).ToString();
        Console.WriteLine($"Send confirmation {rand_confirm_number}"); // temp , before added smtp
        var model = new NotificationModel
        {
            saga_id = command.saga_id,
            booking_id = command.booking_id,
            type = "confirmation",
            channel = "email",
            recipient = command.email,
            content = $"Send confirmation {rand_confirm_number}",
            status = NotificationStatus.Delivered,
            
        };
        if (await _notificationRepository.AddAsync(model))
        {
            return true;
        }
        return false;
    }
}