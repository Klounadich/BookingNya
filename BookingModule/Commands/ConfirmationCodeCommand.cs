using MediatR;

namespace BookingModule.Commands;

public record ConfirmationCodeCommand(
    Guid SagaId,
    string Code): IRequest;