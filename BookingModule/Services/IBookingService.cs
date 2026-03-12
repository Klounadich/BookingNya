using BookingModule.Commands;
using Microsoft.AspNetCore.Http;

namespace BookingModule.Services;

public interface IBookingService
{
    
    public Task<StartSagaResult> StartBooking(BookingRequestCommand data);
    
}