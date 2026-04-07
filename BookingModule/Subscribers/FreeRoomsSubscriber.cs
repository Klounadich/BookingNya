using BookingModule.Services;
using DotNetCore.CAP;
using InventoryModule.Commands;

namespace BookingModule.Subscribers;

public class FreeRoomsSubscriber:ICapSubscribe
{
    private readonly IRequestTracker _requestTracker;

    public FreeRoomsSubscriber(IRequestTracker requestTracker)
    {
        _requestTracker = requestTracker;
    }
    [CapSubscribe("inventory.free_rooms.list")]
    public async Task Handle(FreeRoomsResponse  response)
    {
        
        _requestTracker.SetResponse(response.RequestId, response);
    }
}