using AuthModule.Models;
using Microsoft.AspNetCore.Identity.Data;

namespace AuthModule.Repositories;

public interface IAuthRepository
{
    public Task<bool> RegisterAsync(UsersModel model);
    public Task<UsersModel> GetUserByEmail(string email);
    public Task<bool> IsEmailUnique(string email , CancellationToken ct);
}