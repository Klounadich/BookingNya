using BookingModule.Commands;
using BookingModule.Services;
using MediatR;

namespace BookingModule.Handlers;

public class GetBookings : IRequestHandler<GetBookingsRequest, GetBookingResponce>
{
    private readonly IBookingService _bookingService;

    public GetBookings(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    public async Task<GetBookingResponce> Handle(GetBookingsRequest request, CancellationToken cancellationToken)
    {
        return await _bookingService.GetUserBookings(request);
    }
}