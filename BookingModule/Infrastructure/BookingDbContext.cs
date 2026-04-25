using BookingModule.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingModule.Infrastructure;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) :base(options)
    {
        
    }
    public DbSet<BookingModel> Bookings { get; set; }
    public DbSet<SagaStatesModel>  SagaStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SagaStatesModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        modelBuilder.Entity<SagaStatesModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
        
        modelBuilder.Entity<BookingModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
        
        
    }
}