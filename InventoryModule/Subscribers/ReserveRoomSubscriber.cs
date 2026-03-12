using DotNetCore.CAP;
using InventoryModule.Commands;
using MediatR;
namespace InventoryModule.Handlers;

public class ReserveRoomSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;

    public ReserveRoomSubscriber(ICapPublisher capPublisher)
    {
        _capPublisher = capPublisher;
    }
    [CapSubscribe("inventory.reserve.room.command")]
    public async Task HandleReserveRoom(ReserveRoomCommand command)
    {
        await _capPublisher.PublishAsync("booking.reserve.room.started.event", new ReserveRoomStartedCommand(
            command.sagaId, 
            DateTime.UtcNow
            ));
        
    }
}