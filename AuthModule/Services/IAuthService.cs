using AuthModule.Commands;

namespace AuthModule.Services;

public interface IAuthService
{
    public Task<AuthResponceCommand> RegistrationAsync(RegisterRequestCommand request);
    
    public Task<AuthResponceCommand> LoginAsync(LoginRequestCommand request);
    public Task<decimal> GetBalanceAsync(GetBalanceQuery request);
    public Task<bool> WriteMoneyFromBalance(string userId , decimal amount);
    
    public Task<bool> RefundMoneyToBalance(string userId , decimal amount);
}