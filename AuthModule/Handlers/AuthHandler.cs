using AuthModule.Commands;
using AuthModule.Services;
using MediatR;

namespace AuthModule.Handlers;

public class AuthHandler : IRequestHandler<RegisterRequestCommand, AuthResponceCommand>
{
    private readonly IAuthService _authService;
    public AuthHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<AuthResponceCommand> Handle(RegisterRequestCommand request,
        CancellationToken cancellationToken)
    {
       return  await _authService.RegistrationAsync(request);
    }
}