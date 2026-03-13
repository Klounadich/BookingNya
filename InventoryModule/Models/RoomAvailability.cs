using System.ComponentModel.DataAnnotations;

namespace InventoryModule.Models;

public class RoomAvailability
{
    [Required]
    public Guid id { get; set; } = Guid.NewGuid();
    [Required , MaxLength(50)]
    public string room_id { get; set; }
    [Required]
    public DateTime date { get; set; }
    [Required]
    public bool is_available { get; set; } = true;
    [Required]
    public decimal price_multiplier { get; set; }
    public string notes { get; set; } 
    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    [Required]
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    

}