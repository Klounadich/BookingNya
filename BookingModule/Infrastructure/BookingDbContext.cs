using BookingModule.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingModule.Infrastructure;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) :base(options)
    {
        
    }
    public DbSet<BookingModel> Bookings { get; set; }
}