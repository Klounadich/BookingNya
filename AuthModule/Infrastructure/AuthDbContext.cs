using AuthModule.Models;
using Microsoft.EntityFrameworkCore;
using PaymentModule.Models;

namespace AuthModule.Infrastructure;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
    {
        
    }
    public DbSet<UsersModel> Users { get; set; }
    public DbSet<MockBankUserDataModel> BankUsers { get; set; }
}