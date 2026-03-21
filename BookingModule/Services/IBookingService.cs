using BookingModule.Commands;

namespace BookingModule.Services;

public interface IBookingService
{
    
    public Task<StartSagaResult> StartBooking(BookingRequestCommand data);
    
}