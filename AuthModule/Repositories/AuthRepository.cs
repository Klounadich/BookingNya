using AuthModule.Infrastructure;
using AuthModule.Models;
using Microsoft.EntityFrameworkCore;
using PaymentModule.Models;

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
        var bankdata = new MockBankUserDataModel
        {
            balance = 10000,
            user_id = model.Id
        };
        _context.Users.Add(model);
        _context.BankUsers.Add(bankdata);
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
      return await _context.Users.Where(X => X.email == email).SingleOrDefaultAsync();
    }

    public async Task<bool> IsEmailUnique(string email , CancellationToken ct)
    {
        return !await _context.Users.AnyAsync(x => x.email == email);
    }

    public async Task<decimal> GetBalanceAsync(string id)
    {
        return await _context.BankUsers.AsNoTracking().Where(x => x.user_id.ToString() == id).Select(x=> x.balance)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> WriteFromBalance(string userId , decimal amount)
    {
        var balance =  await _context.BankUsers.AsNoTracking().Where(x => x.user_id.ToString() == userId).FirstOrDefaultAsync();
        balance.balance -= amount;
        _context.BankUsers.Update(balance);
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
    
    public async Task<bool> RefundToBalance(string userId , decimal amount)
    {
        var balance =  await _context.BankUsers.AsNoTracking().Where(x => x.user_id.ToString() == userId).FirstOrDefaultAsync();
        balance.balance += amount;
        _context.BankUsers.Update(balance);
        if (await _context.SaveChangesAsync() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }
}