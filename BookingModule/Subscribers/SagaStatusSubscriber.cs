using BookingModule.Repositories;
using DotNetCore.CAP;
using InventoryModule.Commands;
using Microsoft.AspNetCore.SignalR;
using PaymentModule.Commands;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class SagaStatusSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly IBookingRepository  _bookingRepository;
    private readonly IHubContext<SagaProcessHub> _hubContext;

    public SagaStatusSubscriber(IBookingRepository bookingRepository , ICapPublisher capPublisher, IHubContext<SagaProcessHub> hubContext)
    {
        _hubContext = hubContext;
        _bookingRepository = bookingRepository;
        _capPublisher = capPublisher;
    }
    [CapSubscribe("booking.reserve.room.started.event")]
    public async Task Handle(ReserveRoomStartedCommand command)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.saga_id);
        if (sagaState != null)
        {
            await _hubContext.Clients.Group(command.saga_id.ToString()).SendAsync("ReceiveSagaProgress",
                command.saga_id, 
                "ReserveRoom",  
                "Started",    
                "Starting Reservation." 
            ); // temp 

            
            sagaState.status = SagaTypes.Running;
            sagaState.current_step = "ReserveRoom";
            sagaState.last_updated_at = DateTime.UtcNow;
           
            await _bookingRepository.UpdateSagaStateAsync(sagaState);
         
        }
    }
}
