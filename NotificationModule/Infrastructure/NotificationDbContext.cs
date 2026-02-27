using Microsoft.EntityFrameworkCore;
using NotificationModule.Models;

namespace NotificationModule.Infrastructure;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options) {}
    public DbSet<NotificationModel> Notifications { get; set; }
    public DbSet<NotificationTemplatesModel>  NotificationTemplates { get; set; }
    public DbSet<NotificationEventModel> NotificationEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<NotificationModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<NotificationTemplatesModel>(entity =>
        {
            entity.Property(e => e.Variables)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<NotificationEventModel>(entity =>
        {
            entity.Property(e => e.event_data)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<NotificationModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
    }
}