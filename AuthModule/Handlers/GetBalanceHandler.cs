
using AuthModule.Commands;
using AuthModule.Services;
using MediatR;

namespace BookingModule.Handlers;

public class GetBalanceHandler :IRequestHandler<GetBalanceQuery , decimal >
{
    
    private readonly IAuthService _authService;

    public GetBalanceHandler(IAuthService bookingService)
    {
        _authService = bookingService;
    }
    public async Task<decimal> Handle(GetBalanceQuery request, CancellationToken cancellationToken)
    {
        return await _authService.GetBalanceAsync(request);
    }
}