using BookingModule.Infrastructure;
using BookingModule.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BookingModule.Repositories;

public class BookingRepository : IBookingRepository
{
    public readonly BookingDbContext _db;

    public BookingRepository(BookingDbContext db)
    {
        _db = db;
    }
    public async Task<bool> StartSaga(SagaStatesModel data , BookingModel booking)
    {
        using (var transaction = await _db.Database.BeginTransactionAsync())
        {
            try
            {
                _db.SagaStates.Add(data);
                _db.Bookings.Add(booking);
                if (await _db.SaveChangesAsync()  > 0)
                {
                   await  transaction.CommitAsync();
                    return true;
                }
                return false;
                
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        
    }

    public async Task<SagaStatesModel> GetSagaStateBySagaIdAsync(Guid saga_id)
    {
        var Result = await _db.SagaStates
            .Where(x => x.saga_id == saga_id)
            .SingleOrDefaultAsync();
        return Result;
    }
    
    public async Task<bool> UpdateSagaStateAsync(SagaStatesModel data)
    {
        _db.SagaStates.Update(data);
        if (await _db.SaveChangesAsync() > 0)
        {
            return  true;
        }
        return false;
    }
}