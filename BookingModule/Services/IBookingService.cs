using BookingModule.Commands;

namespace BookingModule.Services;

public interface IBookingService
{
    
    public Task<SagaResult> StartBooking(BookingRequestCommand data);
    
    public Task ConfirmCode(ConfirmationCodeCommand data);
    
    public Task GetFreeRooms(RoomFiltresCommand data , Guid requestId);
    
    public Task<GetBookingResponce> GetUserBookings(GetBookingsRequest request);
    
    public Task RollBack(Guid sagaId);
    
    
}