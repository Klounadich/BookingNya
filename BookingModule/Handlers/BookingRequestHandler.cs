using BookingModule.Commands;
using BookingModule.Services;
using MediatR;

namespace BookingModule.Handlers;

public class BookingRequestHandler : IRequestHandler<BookingRequestCommand , SagaResult>
{
    private readonly IBookingService _bookingService;
    

    public BookingRequestHandler(IBookingService bookingService )
    {
        _bookingService = bookingService;
       
    }

    public async Task<SagaResult> Handle(BookingRequestCommand request, CancellationToken cancellationToken)
    {
        var saga = _bookingService.StartBooking(request);
        return saga.Result;
    }
}