using AuthModule.Commands;
using AuthModule.Models;
using AuthModule.Repositories;
using BCrypt.Net;
namespace AuthModule.Services;

public class AuthService: IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly JWTService _jwtService;

    public AuthService(IAuthRepository authRepository , JWTService jwtService)
    {
        _authRepository = authRepository;
        _jwtService = jwtService;
    }
    public async Task<AuthResponceCommand> RegistrationAsync(RegisterRequestCommand request)
    {
        var password_hashed = BCrypt.Net.BCrypt.HashPassword(request.password);

        UsersModel model = new UsersModel
        {
            
            DisplayName = request.dislay_name,
            email = request.email,
            password_Hashed = password_hashed,

        };
        
        if (await _authRepository.RegisterAsync(model))
        {
            var jwt = await _jwtService.GenerateToken(new JWTRequestCommand(model.Id,model.DisplayName,model.email));

            if (jwt != null)
            {
                return new AuthResponceCommand(
                    jwt,
                    true,
                    "registration completed"
                );
            }
        }
            return new AuthResponceCommand(
                "",
                false,
                "registration failed"
            );
    }

    public async Task<AuthResponceCommand> LoginAsync(LoginRequestCommand request)
    {
        var user = await _authRepository.GetUserByEmail(request.email);
        if (user != null)
        {
            if (BCrypt.Net.BCrypt.Verify(request.password_plain, user.password_Hashed))
            {
                var jwt = await _jwtService.GenerateToken(new JWTRequestCommand(user.Id,user.DisplayName,user.email));

                if (jwt != null)
                {
                    return new AuthResponceCommand(
                        jwt,
                        true,
                        "auth completed"
                    );
                }
            }
        }
        return new AuthResponceCommand(
            "",
            false,
            "auth failed"
        );
        
    }
    
    public async Task<decimal> GetBalanceAsync(GetBalanceQuery request)
    {
        return await _authRepository.GetBalanceAsync(request.userId);
    }

    public async Task<bool> WriteMoneyFromBalance(string userId , decimal amount)
    {
        return await _authRepository.WriteFromBalance(userId , amount);
    }

    public async Task<bool> RefundMoneyToBalance(string userId, decimal amount)
    {
        return await _authRepository.RefundToBalance(userId , amount);
    }
}