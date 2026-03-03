using Shared.Enums;

namespace BookingModule.Commands;

public record StartSagaResult(
    Guid SagaId,
    SagaTypes Status,
    string Message );