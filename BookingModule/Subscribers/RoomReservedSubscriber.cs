using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using InventoryModule.Commands;
using Microsoft.AspNetCore.SignalR;
using Shared.Enums;
using PaymentModule.Commands;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class RoomReservedSubscriber : ICapSubscribe
{
    private readonly IBookingRepository  _bookingRepository;
    private readonly ICapPublisher _capPublisher;
    private readonly IHubContext<SagaProcessHub> _hubContext;
    private readonly ISagaOrchestrator  _sagaOrchestrator;
    public RoomReservedSubscriber(IBookingRepository bookingrepository , ICapPublisher capPublisher ,  
        IHubContext<SagaProcessHub> hubContext , ISagaOrchestrator sagaOrchestrator)
    {
        _bookingRepository = bookingrepository;
        _capPublisher = capPublisher;
        _hubContext = hubContext;
        _sagaOrchestrator = sagaOrchestrator;
    }
    [CapSubscribe("inventory.room.reserved.event")]
    public async Task HandleAsync(RoomReservedEvent command)
    {
        await _sagaOrchestrator.HandleStepSuccessAsync(command.SagaId, "ReserveRoom",
            $"Room has been successfully reserved  {command.ReservationId}.");
        
            var booking = await _bookingRepository.GetBookingBySagaIdAsync(command.SagaId);
            if (booking!=null)
            { 
                
                var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
                if (sagaState == null || sagaState.current_step != "ReserveRoom") return;

                sagaState.current_step = "ProcessPayment";
                sagaState.last_updated_at = DateTime.UtcNow;
                await _bookingRepository.UpdateSagaStateAsync(sagaState);
                
                await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
                    command.SagaId, 
                    "ProcessPayment",  
                    "Started",    
                    "Starting Payment." 
                );
                await _capPublisher.PublishAsync("payment.process.payment.command", new ProcessPaymentCommand(
                    command.SagaId,
                    booking.id,
                    booking.total_price,
                    booking.currency,
                    booking.payment_method,
                    " ", 
                    " ",
                    booking.guest_email,
                    booking.guest_phone,
                    "",
                    "",
                    sagaState.metadata
                     
                ));
            }

         
        }
    

    [CapSubscribe("inventory.room.reserved.event.failed")]
    public async Task HandleAsyncFailed(RoomReservedEvent command)
    {
        
        await _sagaOrchestrator.HandleStepFailureAsync(
            command.SagaId,
            "ReserveRoom",
            "Reservation is Failed.",
            "Reservation is Cancelled"
        );
       
    }
}