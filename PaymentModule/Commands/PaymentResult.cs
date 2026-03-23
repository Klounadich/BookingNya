namespace PaymentModule.Commands;

public record PaymentResult(
    bool Status,
    string? Error);