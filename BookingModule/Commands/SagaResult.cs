using Shared.Enums;

namespace BookingModule.Commands;

public record SagaResult(
    Guid SagaId,
    SagaTypes Status,
    string Message );