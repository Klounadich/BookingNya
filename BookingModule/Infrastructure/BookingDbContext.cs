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
    public DbSet<SagaStepsModel>   SagaSteps { get; set; }
    public DbSet<SagaEventLogsModel>  SagaEventLogs { get; set; }
    public DbSet<ProcessedCommandsModel>  ProcessedCommands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SagaStatesModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<SagaStepsModel>(entity =>
        {
            entity.Property(e => e.request_payload)
                .HasColumnType("jsonb"); 
            entity.Property(e => e.responce_payload)
                .HasColumnType("jsonb"); 
            
        });
        modelBuilder.Entity<SagaEventLogsModel>(entity =>
        {
            entity.Property(e => e.payload)
                .HasColumnType("jsonb"); 
            
        });
        
        modelBuilder.Entity<ProcessedCommandsModel>(entity =>
        {
            entity.Property(e => e.result_payload)
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
        
        modelBuilder.Entity<ProcessedCommandsModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
    }
}