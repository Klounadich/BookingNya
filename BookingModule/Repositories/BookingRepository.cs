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
    public async Task<bool> StartSaga(SagaStatesModel data)
    {
        _db.SagaStates.Add(data);
        if (await _db.SaveChangesAsync() > 0)
        {
            return  true;
        }
        return false;
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