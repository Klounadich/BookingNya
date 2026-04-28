using BookingModule.Commands;
using BookingModule.Infrastructure;
using BookingModule.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace BookingModule.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly BookingDbContext _context;

    public BookingRepository(BookingDbContext context)
    {
        _context = context;
    }
    public async Task<bool?> StartSaga(SagaStatesModel data , BookingModel booking)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.SagaStates.Add(data);
                _context.Bookings.Add(booking);
                if (await _context.SaveChangesAsync()  > 0)
                {
                   await  transaction.CommitAsync();
                    return true;
                }
                return false;
                
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        
    }

    public async Task<SagaStatesModel?> GetSagaStateBySagaIdAsync(Guid saga_id)
    {
        return await _context.SagaStates
            .Where(x => x.saga_id == saga_id)
            .SingleOrDefaultAsync();
        
    }
    public async Task<BookingModel?> GetBookingBySagaIdAsync(Guid saga_id)
    {
        return await _context.Bookings
            .Where(x => x.saga_id == saga_id)
            .SingleOrDefaultAsync();
        
    }
    
    public async Task<bool> UpdateSagaStateAsync(SagaStatesModel data)
    {
        try
        {
            _context.SagaStates.Update(data);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    
    private async Task<bool?> UpdateBookingAsync(BookingModel data)
    {
        try
        {
            _context.Bookings.Update(data);
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<GetBookingResponce> GetBookings(GetBookingsRequest request)
    {
        var responce = await _context.Bookings.AsNoTracking().Where(X => X.user_id.ToString() == request.user_id && X.status != BookingStatus.Cancelled 
                                                          && X.status != BookingStatus.Pending ).Select(x => new GetBookingCard(
                x.room_id.ToString(),
                x.guest_email,
                 x.check_in, 
                x.check_out,
                 x.total_price,
                 x.payment_method
            ))
            .ToListAsync();
        return new GetBookingResponce  ( responce );
    }

    public async Task<bool> UpdateSagaAsync(SagaStatesModel saga_data ,  BookingModel booking_data)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                if (await UpdateSagaStateAsync(saga_data) != false && await UpdateBookingAsync(booking_data) != false)
                {
                    await transaction.CommitAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        return false;
    }

    

    
}