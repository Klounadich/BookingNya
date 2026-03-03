using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;

namespace BookingModule.Models;
[Index(nameof(saga_id),IsUnique =true)]
public class BookingModel
{
    [Required , Key]
    public Guid id { get; set; } = Guid.NewGuid();
    [Required ] 
    public Guid saga_id { get; set; } 
    [Required, MaxLength(50)]
    public string room_id { get; set; }
    [Required, MaxLength(200)]
    public string guest_name { get; set; }
    [Required, MaxLength(255)]
    public string guest_email { get; set; }
    [Required, MaxLength(50)]
    public string guest_phone { get; set; }
    [Required] 
    public DateTime check_in { get; set; }
    [Required]
    public DateTime check_out { get; set; } // проверка из csv
    [Required ] // >0
    public decimal total_price { get; set; }

    [Required]
    public string currency { get; set; } = "USD";
    [ MaxLength(50)]
    public string payment_method { get; set; }
    [MaxLength(100)]
    public string payment_reservation_id { get; set; }
    [Required]
    public BookingStatus status { get; set; } = BookingStatus.Pending;
    [Required]
    public DateTime created_at { get; set; } =  DateTime.UtcNow;
    [Required]
    public DateTime updated_at { get; set; } =  DateTime.UtcNow;
    
    public DateTime? cancelled_at { get; set; }
    public string? cancellation_reason {get; set;}
}