namespace PaymentModule.Commands;

public record PaymentProcessed(
    Guid SagaId,
    string email,
    Guid booking_id,
    string Error);