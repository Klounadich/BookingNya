using System.ComponentModel.DataAnnotations;
using Shared.Enums;

namespace InventoryModule.Models;

public class RoomModel
{
    [Required , Key , MaxLength(50)]
    public string id { get; set; }
    [Required ,  MaxLength(50)]
    public string type { get; set; }
    [Required]
    public int capacity { get; set; }
    [Required]
    public decimal price_per_night { get; set; }
    
    public int? floor  { get; set; }
    public string? description { get; set; }
    public Dictionary<string,object>? amenities { get; set; }
    [Required ]
    public RoomStatus status { get; set; } =  RoomStatus.Available;
    [Required ]
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    [Required ]
    public DateTime updated_at { get; set; } =  DateTime.UtcNow;
}