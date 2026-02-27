using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace InventoryModule.Models;

public class RoomReservationModel
{
    [Required]
    public Guid id { get; set; } = new Guid();
    [Required , MaxLength(50)]
    public string room_id { get; set; }
    [Required]
    public Guid saga_id { get; set; }
    public Guid booking_id { get; set; }
    [Required , MaxLength(200)]
    public string guest_name { get; set; }
    [Required]
    public DateTime check_in { get; set; }
    [Required]
    public DateTime check_out { get; set; }

    [Required] public ReservationStatus status { get; set; } = ReservationStatus.Reserved;
    public string reservation_reference { get; set; }
    public string cancellation_reason { get; set; }
    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime cancelled_at { get; set; } = DateTime.UtcNow;
    public Dictionary<string,object>? metadata { get; set; }
}