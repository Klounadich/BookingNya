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
    private readonly IHubContext<SagaProcessHub> _hubContext;
    private readonly ISagaOrchestrator _sagaOrchestrator;
    private readonly ICapPublisher _capPublisher;

    public NotificationConfirmSubscriber(
        IBookingRepository bookingRepository,
        IHubContext<SagaProcessHub> hubContext,
        ISagaOrchestrator sagaOrchestrator, ICapPublisher capPublisher)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
        _sagaOrchestrator = sagaOrchestrator;
        _capPublisher = capPublisher;
    }

    [CapSubscribe("notification.confirm.success")]
    public async Task ConfirmSuccess(ConfirmCodeCommand data)
    {
        

        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(data.SagaId);
        if (sagaState == null || sagaState.current_step != "WaitingConfirm") return;

        sagaState.status = SagaTypes.Completed;
        sagaState.current_step = "ProcessPayment";
        sagaState.last_updated_at = DateTime.UtcNow;

       
        if (await _bookingRepository.UpdateSagaStateAsync(sagaState))
        {
            await _sagaOrchestrator.HandleStepSuccessAsync(data.SagaId, "ProcessPayment" ," WritingMoney");
            await _capPublisher.PublishAsync("Payment.write.money",
                sagaState.saga_id);
        }
    }

    [CapSubscribe("notification.confirm.failure")]
    public async Task ConfirmFailure(ConfirmCodeCommand data)
    {
        await _hubContext.Clients.Group(data.SagaId.ToString())
            .SendAsync("ReceiveSagaError",
                data.SagaId,
                "Notification",
                "Failed",
                "Confirm Notification failed."
            );
    }

    [CapSubscribe("notification.confirm.cancel")]
    public async Task ConfirmCancel(ConfirmCodeCommand data)
    {
        await _sagaOrchestrator.HandleStepFailureAsync(
            data.SagaId,
            "ConfirmationFailed",
            "Confirm Notification cancelled.",
            "More than 3 incorrect attempts"
        );
    }
}