namespace PaymentModule.Commands;

public record ProcessPaymentCommand(
    Guid SagaId,
    Guid BookingId,
    decimal Amount,
    string Currency,
    string PaymentMethod,
    string? PaymentGateway,
    string? CustomerId,
    string? CustomerEmail,
    string? CustomerPhone ,
    string? CardToken,
    string? Error,
    Dictionary<string, object>? Metadata
    );