using NotificationModule.Infrastructure;
using NotificationModule.Models;
using NotificationModule.Services;

namespace NotificationModule.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly NotificationDbContext _notificationDbContext;

    public NotificationRepository(NotificationDbContext notificationDbContext)
    {
        _notificationDbContext = notificationDbContext;
    }
    public async Task<bool> AddAsync(NotificationModel notification)
    {
        await _notificationDbContext.AddAsync(notification);
        if (await _notificationDbContext.SaveChangesAsync() > 0)
        {
            return true;
        }
        return false;
        
    }
}