using BookingModule.Commands;
using BookingModule.Services;
using MediatR;

namespace BookingModule.Handlers;

public class ConfirmCodeHandler : IRequestHandler<ConfirmationCodeCommand>
{
    private readonly IBookingService _bookingService;
    

    public ConfirmCodeHandler(IBookingService bookingService )
    {
        _bookingService = bookingService;
       
    }
    public async Task Handle(ConfirmationCodeCommand request, CancellationToken cancellationToken)
    {
        await _bookingService.ConfirmCode(request);
       
    }
}