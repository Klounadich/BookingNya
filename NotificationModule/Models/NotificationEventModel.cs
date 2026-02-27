using System.ComponentModel.DataAnnotations;

namespace NotificationModule.Models;

public class NotificationEventModel
{
    [Key]
    [Required]
    public Guid id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid notification_id { get; set; }

    public Guid? saga_id { get; set; }

    [Required]
    [MaxLength(100)]
    public string event_type { get; set; }

    public Dictionary<string, object>? event_data { get; set; }

    [MaxLength(50)]
    public string? status_before { get; set; }

    [MaxLength(50)]
    public string? status_after { get; set; }

    public string? error_message { get; set; }

    [Required]
    public DateTime occurred_at { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime created_at { get; set; } = DateTime.UtcNow;
}