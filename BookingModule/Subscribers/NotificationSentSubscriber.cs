using BookingModule.Repositories;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using NotificationModule.Commands;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class NotificationSentSubscriber : ICapSubscribe
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IHubContext<SagaProcessHub>  _hubContext;

    public NotificationSentSubscriber(IBookingRepository bookingRepository , IHubContext<SagaProcessHub> hubContext)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
    }
    [CapSubscribe("notification.sent.event.success")]
    public async Task HandleSuccessAsync(ConfirmationSentCommand command)
    {
        
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState != null)
        {
            await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
                command.SagaId,
                "Notification",
                "Completed",
                "Notification sent."
            );

            sagaState.status = SagaTypes.Completed;
            sagaState.current_step = "SendConfirmation";
            sagaState.last_updated_at = DateTime.UtcNow;

            if (await _bookingRepository.UpdateSagaStateAsync(sagaState) == true)
            {
                var booking = await _bookingRepository.GetBookingBySagaIdAsync(command.SagaId);
                if (booking != null)
                {
                    booking.status = BookingStatus.Completed;
                    booking.updated_at = DateTime.UtcNow;

                    if (await _bookingRepository.UpdateBookingAsync(booking)!= false)
                    {
                        await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaProgress",
                            command.SagaId,
                            "Booking",
                            "Completed",
                            $"Room {booking.room_id} has been received on period (data) ."
                        );
                    }
                }
            }
        }

    }
}