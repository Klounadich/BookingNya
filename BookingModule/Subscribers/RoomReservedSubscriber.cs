using BookingModule.Repositories;
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

    public RoomReservedSubscriber(IBookingRepository bookingrepository , ICapPublisher capPublisher ,  IHubContext<SagaProcessHub> hubContext)
    {
        _bookingRepository = bookingrepository;
        _capPublisher = capPublisher;
        _hubContext = hubContext;
    }
    [CapSubscribe("inventory.room.reserved.event")]
    public async Task HandleAsync(RoomReservedEvent command)
    {
        
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState!=null)
        {  await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
                command.SagaId, 
                "ReserveRoom",  
                "Completed",    
                $"Room has been successfully reserved  {command.ReservationId}." 
            );
            var booking = await _bookingRepository.GetBookingBySagaIdAsync(sagaState.saga_id);
            if (booking!=null)
            {
                sagaState.status = SagaTypes.Running;
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
    }

    [CapSubscribe("inventory.room.reserved.event.failed")]
    public async Task HandleAsyncFailed(RoomReservedEvent command)
    {
        
        
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState!=null)
        {
            await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaError",
                command.SagaId,
                "ReserveRoom",
                "Failed",
                "Reservation is Failed."
            );
            
            var booking = await _bookingRepository.GetBookingBySagaIdAsync(sagaState.saga_id);
            if (booking != null)
            {
                booking.status = BookingStatus.Cancelled;
                booking.updated_at = DateTime.UtcNow;
                booking.cancellation_reason = "Reservation is Cancelled";
                booking.cancelled_at = DateTime.UtcNow;
                sagaState.status = SagaTypes.Failed;
                sagaState.current_step = "ReserveRoom";
                sagaState.last_updated_at = DateTime.UtcNow;
                await _bookingRepository.UpdateSagaAsync(sagaState , booking);
            }
        }
    }
}