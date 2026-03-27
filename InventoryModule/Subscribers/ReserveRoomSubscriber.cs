
using DotNetCore.CAP;
using InventoryModule.Commands;
using InventoryModule.Services;


namespace InventoryModule.Handlers;

public class ReserveRoomSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly IReserveRoomService  _service;

    public ReserveRoomSubscriber(ICapPublisher capPublisher , IReserveRoomService service )
    {
        _capPublisher = capPublisher;
        _service = service;
        
    }
    
    [CapSubscribe("inventory.reserve.room.command")]
    public async Task HandleReserveRoom(ReserveRoomCommand command)
    {
        await _capPublisher.PublishAsync("booking.reserve.room.started.event", new ReserveRoomStartedCommand(
            command.sagaId, 
            DateTime.UtcNow
            ));
            var reservation_id = await _service.ReserveRoomAsync(command);
            if (reservation_id.reservaiton_id != Guid.Empty)
            {
                await _capPublisher.PublishAsync("inventory.room.reserved.event", new RoomReservedEvent(
                    command.sagaId,
                    reservation_id.reservaiton_id,
                    command.roomId

                ));
            }
            else
            {
                await _capPublisher.PublishAsync("inventory.room.reserved.event.failed", new RoomReservedEvent(
                    command.sagaId,
                    reservation_id.reservaiton_id,
                    command.roomId

                ));
            }






    }
}