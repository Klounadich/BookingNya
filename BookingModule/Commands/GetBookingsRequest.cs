using MediatR;

namespace BookingModule.Commands;

public record GetBookingsRequest(
    string user_id) : IRequest<GetBookingResponce>;