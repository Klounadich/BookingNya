using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using NotificationModule.Commands;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class NotificationConfirmSubscriber : ICapSubscribe
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingService _service;
    private readonly IHubContext<SagaProcessHub> _hubContext;

    public NotificationConfirmSubscriber(IBookingRepository bookingRepository, IHubContext<SagaProcessHub> hubContext , IBookingService service)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
        _service = service;
    }

    [CapSubscribe("notification.confirm.success")]
    public async Task ConfirmSuccess(ConfirmCodeCommand data)
    {

        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(data.SagaId);
        if (sagaState != null)
        {
            sagaState.status = SagaTypes.Completed;
            sagaState.current_step = "NotificationConfirm";
            sagaState.last_updated_at = DateTime.UtcNow;

            if (await _bookingRepository.UpdateSagaStateAsync(sagaState) == true)
            {

                var booking = await _bookingRepository.GetBookingBySagaIdAsync(data.SagaId);
                if (booking != null)
                {
                    booking.status = BookingStatus.Completed;
                    booking.updated_at = DateTime.UtcNow;

                    if (await _bookingRepository.UpdateBookingAsync(booking) != false)
                    {
                        await _hubContext.Clients.Group(data.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
                            data.SagaId,
                            "Booking",
                            "Completed",
                            $"Room {booking.room_id} has been received on period from {booking.check_in} to {booking.check_out} ."
                        );
                    }
                }
            }
        }
    }

    [CapSubscribe("notification.confirm.failure")]
    public async Task ConfirmFailure(ConfirmCodeCommand data)
    {
        {
        
            var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(data.SagaId);
            if (sagaState != null)
            {
                
                await _hubContext.Clients.Group(data.SagaId.ToString()).SendAsync("ReceiveSagaError",
                    data.SagaId,
                    "Notification",
                    "Failed",
                    "Confirm Notification failed."
                );



                await _bookingRepository.UpdateSagaStateAsync(sagaState) ;


            }
        }
    }


    [CapSubscribe("notification.confirm.cancel")]
    public async Task ConfirmCancel(ConfirmCodeCommand data)
    {
        {
        
            var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(data.SagaId);
            if (sagaState != null)
            {
                var booking = await _bookingRepository.GetBookingBySagaIdAsync(data.SagaId);
                if (booking != null)
                {
                    await _hubContext.Clients.Group(data.SagaId.ToString()).SendAsync("ReceiveSagaError",
                        data.SagaId,
                        "Notification",
                        "Cancelled",
                        "Confirm Notification cancelled ."
                    );
                    
                    booking.status = BookingStatus.Cancelled;
                    booking.updated_at = DateTime.UtcNow;
                    booking.cancellation_reason = "More than 3 incorrect attempts";
                    booking.cancelled_at = DateTime.UtcNow;
                    sagaState.status = SagaTypes.Failed;
                    sagaState.current_step = "ConfirmationFailed";
                    sagaState.last_updated_at = DateTime.UtcNow;
                    await _service.RollBack(data.SagaId);
                    await _bookingRepository.UpdateBookingAsync(booking);
                    await _bookingRepository.UpdateSagaStateAsync(sagaState);
                }

            }
        }
    }
}