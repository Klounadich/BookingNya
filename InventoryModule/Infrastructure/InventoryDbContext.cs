using InventoryModule.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryModule.Infrastructure;

public class InventoryDbContext: DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
    {
        
    }
    public DbSet<RoomModel>  Rooms { get; set; }
    public DbSet<RoomReservationModel>   RoomReservations { get; set; }
    public DbSet<RoomAvailability>   RoomAvailabilities { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomModel>(entity =>
        {
            entity.Property(e => e.amenities)
                .HasColumnType("jsonb"); 
            
            
        });
        modelBuilder.Entity<RoomReservationModel>(entity =>
        {
            entity.Property(e => e.metadata)
                .HasColumnType("jsonb"); 
            
            
        });
        
        modelBuilder.Entity<RoomModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
        
        modelBuilder.Entity<RoomReservationModel>()
            .Property(e => e.status)
            .HasConversion<string>()  
            .HasColumnType("varchar(50)");
    }
    
}
