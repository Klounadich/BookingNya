using Microsoft.EntityFrameworkCore;
using PaymentModule.Models;

namespace PaymentModule.Infrastructure;

public class PaymentDbContext: DbContext
{
    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
    {
        
    }
    public  DbSet<PaymentsModel> Payments { get; set; }
     public DbSet<PaymentsMethodsModel> PaymentsMethods { get; set; }
     public DbSet<PaymentTransactionsModel> PaymentTransactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PaymentsModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        modelBuilder.Entity<PaymentsMethodsModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<PaymentsMethodsModel>(entity =>
        {
            entity.Property(e => e.billing_address)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<PaymentTransactionsModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<PaymentTransactionsModel>(entity =>
        {
            entity.Property(e => e.gateway_response)
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