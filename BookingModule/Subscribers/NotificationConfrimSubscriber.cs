using BookingModule.Repositories;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using NotificationModule.Commands;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class NotificationConfrimSubscriber : ICapSubscribe
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHubContext<SagaProcessHub> _hubContext;

    public NotificationConfrimSubscriber(IBookingRepository bookingRepository, IHubContext<SagaProcessHub> hubContext)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
    }

    [CapSubscribe("notification.confirm.success")]
    public async Task ConfrimSuccess(ConfirmCodeCommand data)
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
                            $"Room {booking.room_id} has been received on period (data) ."
                        );
                    }
                }
            }
        }
    }

    [CapSubscribe("notification.confirm.failure")]
    public async Task ConfrimFailure(ConfirmCodeCommand data)
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

                sagaState.status = SagaTypes.Failed;
                sagaState.current_step = "ConfirmationFailed";
                sagaState.last_updated_at = DateTime.UtcNow;


                await _bookingRepository.UpdateSagaStateAsync(sagaState) ;



            }
        }
    }
}