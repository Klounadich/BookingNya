using BookingModule.Repositories;
using BookingModule.Services;
using DotNetCore.CAP;
using Microsoft.AspNetCore.SignalR;
using PaymentModule.Commands;
using Shared.Enums;
using NotificationModule.Commands;
using Shared.SignalR;

namespace BookingModule.Subscribers;

public class PaymentProcessedSubscriber : ICapSubscribe
{
    private readonly ICapPublisher _capPublisher;
    private readonly IHubContext<SagaProcessHub> _hubContext;
    private readonly IBookingRepository _bookingRepository;
    private readonly ISagaOrchestrator _sagaOrchestrator;

    public PaymentProcessedSubscriber(
        IBookingRepository bookingRepository,
        IHubContext<SagaProcessHub> hubContext,
        ICapPublisher capPublisher,
        ISagaOrchestrator sagaOrchestrator)
    {
        _bookingRepository = bookingRepository;
        _hubContext = hubContext;
        _capPublisher = capPublisher;
        _sagaOrchestrator = sagaOrchestrator;
    }

    [CapSubscribe("payment.processed.event")]
    public async Task HandleAsync(PaymentProcessed command)
    {
        await _sagaOrchestrator.HandleStepSuccessAsync(
            command.SagaId,
            "ProcessPayment",
            "Payment authorized."
        );

        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(command.SagaId);
        if (sagaState == null || sagaState.current_step != "ProcessPayment") return;

        sagaState.current_step = "SendConfirmation";
        sagaState.last_updated_at = DateTime.UtcNow;
        await _bookingRepository.UpdateSagaStateAsync(sagaState);

        await _hubContext.Clients.Group(command.SagaId.ToString())
            .SendAsync("ReceiveSagaProgress",
                command.SagaId,
                "Notification",
                "Starting",
                "Send notification."
            );

        await _capPublisher.PublishAsync("notification.send.confirmation.command",
            new SendConfirmationCommand(command.SagaId, command.booking_id, command.email));
    }

    [CapSubscribe("payment.failed.event")]
    public async Task FailedHandleAsync(PaymentProcessed command)
    {
        await _sagaOrchestrator.HandleStepFailureAsync(
            command.SagaId,
            "ProcessPayment",
            $"Payment failed ({command.Error}).",
            $"Payment failed {command.Error}"
        );
    }
    
    [CapSubscribe("payment.writed.sucessfully")]
    public async Task PaymentWrited(Guid SagaId)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(SagaId);
        if (sagaState == null || sagaState.current_step != "ProcessPayment") return;

        sagaState.current_step = "PaymentWrited";
        sagaState.last_updated_at = DateTime.UtcNow;
        var booking = await _bookingRepository.GetBookingBySagaIdAsync(SagaId);
        if (booking != null)
        {
            booking.status = BookingStatus.Completed;
            booking.updated_at = DateTime.UtcNow;

            if (await _bookingRepository.UpdateSagaAsync(sagaState, booking))
            {
                await _hubContext.Clients.Group(SagaId.ToString()).SendAsync("ReceiveSagaProgress",
                    SagaId,
                    "Booking",
                    "Completed",
                    $"Room {booking.room_id} has been received on period from {booking.check_in} to {booking.check_out} ."
                );
            }
        }

        
    }
    
    [CapSubscribe("payment.write.failed")]
    public async Task PaymentWriteFailed(Guid SagaId)
    {
        
        await _sagaOrchestrator.HandleStepFailureAsync(
            SagaId,
            "ProcessPayment",
            $"Payment failed .",
            $"Payment failed "
        );
        
            }
        

        
    

    
}