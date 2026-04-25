using AuthModule.Infrastructure;
using AuthModule.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _context;

    public AuthRepository(AuthDbContext context)
    {
        _context = context;
    }
    public async Task<bool> RegisterAsync(UsersModel model)
    {
        _context.Users.Add(model);
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public async Task<UsersModel> GetUserByEmail(string email)
    {
      return await _context.Users.Where(X => X.email == email).SingleAsync();
    }

    public async Task<bool> IsEmailUnique(string email , CancellationToken ct)
    {
        return !await _context.Users.AnyAsync(x => x.email == email);
    }
}