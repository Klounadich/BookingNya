using BookingModule.Commands;
using BookingModule.Services;
using MediatR;
using Shared.Enums;

namespace BookingModule.Handlers;

public class CallBackHandler : IRequestHandler<CallBackSagaRequest, SagaResult>
{
    private readonly IBookingService  _bookingService;

    public CallBackHandler(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }
    public async Task<SagaResult> Handle(CallBackSagaRequest request, CancellationToken cancellationToken)
    {
        await _bookingService.RollBack(request.sagaId);
        return new SagaResult(request.sagaId, SagaTypes.Compensating, "Saga Compenstated");
    }
}