using BookingModule.Commands;
using BookingModule.Repositories;
using BookingModule.Services;
using InventoryModule.Commands;
using MediatR;

namespace BookingModule.Handlers;

public class CheckFreeRoomsHandler: IRequestHandler<RoomFiltresCommand , FreeRoomsResponse>
{
    private readonly IRequestTracker _requestTracker;
    private readonly IBookingService _bookingService;
    public CheckFreeRoomsHandler(IBookingService bookingService , IRequestTracker requestTracker)
    {
        _requestTracker = requestTracker;
        _bookingService = bookingService;
    }

    public async Task<FreeRoomsResponse> Handle(RoomFiltresCommand request , CancellationToken token )
    {
        var requestId = Guid.NewGuid();
        await _bookingService.GetFreeRooms(request , requestId);
        var response = await _requestTracker.WaitForResponseAsync(requestId, TimeSpan.FromSeconds(3));
    return response;
    }
}