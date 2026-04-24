using AuthModule.Commands;

namespace AuthModule.Services;

public interface IAuthService
{
    public Task<AuthResponceCommand> RegistrationAsync(RegisterRequestCommand request);
    
    public Task<AuthResponceCommand> LoginAsync(LoginRequestCommand request);
}