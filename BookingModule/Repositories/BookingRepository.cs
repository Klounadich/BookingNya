using BookingModule.Infrastructure;
using BookingModule.Models;
using Microsoft.AspNetCore.Http;

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
}