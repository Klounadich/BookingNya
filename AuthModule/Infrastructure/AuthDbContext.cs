using AuthModule.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthModule.Infrastructure;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
    {
        
    }
    public DbSet<UsersModel> Users { get; set; }
}