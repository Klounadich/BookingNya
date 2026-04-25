using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using NotificationModule.Commands;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class NotificationSentSubscriber : ICapSubscribe
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingService _service;
    private readonly IHubContext<SagaProcessHub>  _hubContext;

    public NotificationSentSubscriber(IBookingRepository bookingRepository , IHubContext<SagaProcessHub> hubContext , IBookingService service)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
        _service = service;
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

            sagaState.status = SagaTypes.Running;
            sagaState.current_step = "WaitingConfirm";
            sagaState.last_updated_at = DateTime.UtcNow;

            await _bookingRepository.UpdateSagaStateAsync(sagaState);

        }

    }
    
    [CapSubscribe("notification.sent.event.failure")]
    public async Task HandleFailedAsync(ConfirmationSentCommand command)
    {
        
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState != null)
        {
            var booking = await _bookingRepository.GetBookingBySagaIdAsync(command.SagaId);
            if (booking != null)
            {
                await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaError",
                    command.SagaId,
                    "Notification",
                    "Failed",
                    "Send Notification failed."
                );

                booking.status = BookingStatus.Cancelled;
                booking.updated_at = DateTime.UtcNow;
                booking.cancellation_reason = "Notification dont sent";
                booking.cancelled_at = DateTime.UtcNow;

                sagaState.status = SagaTypes.Failed;
                sagaState.current_step = "SendConfirmation";
                sagaState.last_updated_at = DateTime.UtcNow;
                await _service.RollBack(sagaState.saga_id);
                await _bookingRepository.UpdateSagaAsync(sagaState , booking);

            }
            else
            {
                await _hubContext.Clients.Group(command.SagaId.ToString()).SendAsync("ReceiveSagaError",
                    command.SagaId,
                    "Notification",
                    "Failed",
                    "Send Notification failed."
                );
            }

        }
            }
        }

    
