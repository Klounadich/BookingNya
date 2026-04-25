using Microsoft.EntityFrameworkCore;
using PaymentModule.Models;

namespace PaymentModule.Infrastructure;

public class PaymentDbContext: DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
        
    }
    public  DbSet<PaymentsModel> Payments { get; set; }
     
     

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentsModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<PaymentsModel>(entity =>
        {
            entity.Property(e => e.gateway_response)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<PaymentsModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
    }
}