using BookingModule.Commands;
using BookingModule.Repositories;
using BookingModule.Services;
using MediatR;

namespace BookingModule.Handlers;

public class BookingRequestHandler : IRequestHandler<BookingRequestCommand , StartSagaResult>
{
    private readonly IBookingService _bookingService;
    private readonly IMediator _mediator;

    public BookingRequestHandler(IBookingService bookingService ,  IMediator mediator)
    {
        _bookingService = bookingService;
        _mediator = mediator;
    }

    public async Task<StartSagaResult> Handle(BookingRequestCommand request, CancellationToken cancellationToken)
    {
        var saga = _bookingService.StartBooking(request);
        return saga.Result;
    }
}