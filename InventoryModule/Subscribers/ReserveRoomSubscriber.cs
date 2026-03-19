
using DotNetCore.CAP;
using InventoryModule.Commands;
using InventoryModule.Models;
using InventoryModule.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Shared.SignalR;

namespace InventoryModule.Handlers;

public class ReserveRoomSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly IReserveRoomService  _service;
    private readonly IHubContext<SagaProcessHub> _hubContext;

    public ReserveRoomSubscriber(ICapPublisher capPublisher , IReserveRoomService service ,  IHubContext<SagaProcessHub> hubContext)
    {
        _capPublisher = capPublisher;
        _service = service;
        _hubContext = hubContext;
    }
    [CapSubscribe("inventory.reserve.room.command")]
    public async Task HandleReserveRoom(ReserveRoomCommand command)
    {
        await _capPublisher.PublishAsync("booking.reserve.room.started.event", new ReserveRoomStartedCommand(
            command.sagaId, 
            DateTime.UtcNow
            ));
        
        
            
            var reservationId = await _service.ReserveRoomAsync(command);
            
         
           
            await _capPublisher.PublishAsync("inventory.room.reserved.event", new RoomReservedEvent(
                command.sagaId,
                reservationId.reservaiton_id,
                command.roomId
                
            ));
        





    }
}