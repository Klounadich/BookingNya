using BookingModule.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Shared.Enums;
using Shared.SignalR;

namespace BookingModule.Services;
public interface ISagaOrchestrator
{
    Task HandleStepSuccessAsync(Guid sagaId, string stepName, string successMessage);
    Task HandleStepFailureAsync(Guid sagaId, string stepName, string errorMessage, string cancellationReason);
}

public class SagaOrchestrator : ISagaOrchestrator
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingService _bookingService;
    private readonly IHubContext<SagaProcessHub> _hubContext;
    private readonly ILogger<SagaOrchestrator> _logger;

    public SagaOrchestrator(
        IBookingRepository bookingRepository,
        IBookingService bookingService,
        IHubContext<SagaProcessHub> hubContext,
        ILogger<SagaOrchestrator> logger)
    {
        _bookingRepository = bookingRepository;
        _bookingService = bookingService;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task HandleStepSuccessAsync(Guid sagaId, string stepName, string successMessage)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(sagaId);
        if (sagaState == null)
        {
            _logger.LogWarning("Saga {SagaId} not found for success step {Step}", sagaId, stepName);
            return;
        }
        
        if (sagaState.current_step != stepName)
            return;

     
        sagaState.status = SagaTypes.Running;
        sagaState.current_step = stepName;
        sagaState.last_updated_at = DateTime.UtcNow;

        await _bookingRepository.UpdateSagaStateAsync(sagaState);
        await _hubContext.Clients.Group(sagaId.ToString())
            .SendAsync("ReceiveSagaProgress", sagaId, stepName, "Completed", successMessage);

        _logger.LogInformation("Saga {SagaId} step {Step} succeeded", sagaId, stepName);
    }

    public async Task HandleStepFailureAsync(Guid sagaId, string stepName, string errorMessage, string cancellationReason)
    {
        var sagaState = await _bookingRepository.GetSagaStateBySagaIdAsync(sagaId);
        if (sagaState == null)
        {
            _logger.LogWarning("Saga {SagaId} not found for failure step {Step}", sagaId, stepName);
            return;
        }
        if (sagaState.status == SagaTypes.Completed || sagaState.status == SagaTypes.Failed)
            return;

        var booking = await _bookingRepository.GetBookingBySagaIdAsync(sagaId);
        if (booking != null)
        {
            booking.status = BookingStatus.Cancelled;
            booking.updated_at = DateTime.UtcNow;
            booking.cancellation_reason = cancellationReason;
            booking.cancelled_at = DateTime.UtcNow;
        }

        sagaState.status = SagaTypes.Failed;
        sagaState.current_step = stepName;
        sagaState.last_updated_at = DateTime.UtcNow;
        sagaState.error_message = errorMessage;

        await _bookingRepository.UpdateSagaAsync(sagaState, booking);
        await _hubContext.Clients.Group(sagaId.ToString())
            .SendAsync("ReceiveSagaError", sagaId, stepName, "Failed", errorMessage);
        await _bookingService.RollBack(sagaId);

        _logger.LogError("Saga {SagaId} failed at step {Step}: {Error}", sagaId, stepName, errorMessage);
    }
}