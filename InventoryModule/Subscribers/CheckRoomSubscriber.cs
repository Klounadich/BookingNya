using DotNetCore.CAP;
using InventoryModule.Services;

namespace InventoryModule.Handlers;

public class CheckRoomSubscriber: ICapSubscribe
{
    private readonly IReserveRoomService  _reserveRoomService;
    private readonly ICapPublisher _capPublisher;

    public CheckRoomSubscriber(IReserveRoomService reserveRoomService , ICapPublisher capPublisher)
    {
        _reserveRoomService = reserveRoomService;
        _capPublisher = capPublisher;
    }
    [CapSubscribe("inventory.check.rooms")]
    public async Task CheckRoomHandle(Guid RequestId)
    {
        await _capPublisher.PublishAsync("inventory.free_rooms.list", 
            await _reserveRoomService.CheckFreeRoomsAsync(RequestId));
        
    }
}