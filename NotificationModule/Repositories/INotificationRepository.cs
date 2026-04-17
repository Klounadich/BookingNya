using NotificationModule.Commands;
using NotificationModule.Models;

namespace NotificationModule.Repositories;

public interface INotificationRepository
{
    public Task<bool> AddAsync(NotificationModel notification);
    public Task<bool> UpdateAsync(NotificationModel notification);
    
    Task<(bool?,int?)>CheckCodeAsync(ConfirmCodeCommand data);
}