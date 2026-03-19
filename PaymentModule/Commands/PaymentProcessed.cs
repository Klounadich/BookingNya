namespace PaymentModule.Commands;

public record PaymentProcessed(
    Guid SagaId,
    string Error);