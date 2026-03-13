using BookingModule.Commands;
using Microsoft.AspNetCore.Http;
using RoomReservedEvent = InventoryModule.Commands.RoomReservedEvent;

namespace BookingModule.Services;

public interface IBookingService
{
    
    public Task<StartSagaResult> StartBooking(BookingRequestCommand data);
    
}