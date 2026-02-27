using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BookingModule.Models;

[Index(nameof(saga_id),IsUnique =true)]
public class SagaEventLogsModel
{
    [Required , Key]
    public Guid id { get; set; } = Guid.NewGuid();
    [Required]
    public Guid saga_id { get; set; }
    [Required , MaxLength(100)]
    public string event_type { get; set; }
    [Required , MaxLength(100)]
    public string event_name { get; set; }
    [Required , MaxLength(20)]
    public string level { get; set; }
    [Required]
    public string message { get; set; }
    public Dictionary<string, object >? payload { get; set; }
    public string source_module { get; set; }
    public Guid correlation_id { get; set; }
    [Required ]
    public DateTime occurred_at { get; set; } = DateTime.UtcNow;
    public DateTime processed_at { get; set; }
    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;
}