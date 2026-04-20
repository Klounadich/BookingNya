using Microsoft.EntityFrameworkCore;
using NotificationModule.Commands;
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
    public async Task<bool> UpdateAsync(NotificationModel notification)
    {
        _notificationDbContext.Update(notification);
        if (await _notificationDbContext.SaveChangesAsync() > 0)
        {
            return true;
        }
        return false;
        
    }

    public async Task<(bool?,int?)> CheckCodeAsync(ConfirmCodeCommand data)
    {
        var codeFromDb = await _notificationDbContext.Notifications.Where(x => x.saga_id == data.SagaId && x.attempts <=3)
            .Select(x => x.content).SingleOrDefaultAsync();
        if ( codeFromDb != null && codeFromDb == data.ConfirmationCode)
        {
            return (true,0);
        }

        var model =await _notificationDbContext.Notifications.Where(x => x.saga_id == data.SagaId)
            .SingleOrDefaultAsync();
        if (model == null)
        {
            return (null,0);
        }
        model.attempts++;
         _notificationDbContext.Update(model);
        await _notificationDbContext.SaveChangesAsync();
        return (false,model.attempts);
    }
}