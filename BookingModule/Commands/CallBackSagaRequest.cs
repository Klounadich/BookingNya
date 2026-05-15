using MediatR;

namespace BookingModule.Commands;

public record CallBackSagaRequest(
    Guid sagaId) : IRequest<SagaResult>;