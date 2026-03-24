using NotificationModule.Models;

namespace NotificationModule.Repositories;

public interface INotificationRepository
{
    public Task<bool> AddAsync(NotificationModel notification);
}