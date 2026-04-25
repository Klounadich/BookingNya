using AuthModule.Commands;
using AuthModule.Services;
using MediatR;

namespace AuthModule.Handlers;

public class LoginHandler : IRequestHandler<LoginRequestCommand, AuthResponceCommand>
{
    private readonly IAuthService _authService;
    public LoginHandler(IAuthService authService)
    {
        _authService = authService;
    }
    public async Task<AuthResponceCommand> Handle(LoginRequestCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(request);
    }
}